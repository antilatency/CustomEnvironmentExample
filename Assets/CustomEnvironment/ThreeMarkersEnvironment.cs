using System.Collections.Generic;
using System.Linq;
using Antilatency.Alt.Tracking;
using Antilatency.InterfaceContract;
using UnityEngine;

// A simplified, yet functional, example of custom environment which uses just three markers on the floor plane.
// See CustomEnvironment.cs to learn the general idea how environments work.
public class ThreeMarkersEnvironment : Antilatency.InterfaceContract.InterfacedObject, Antilatency.Alt.Tracking.IEnvironment {

    // In this example we will find the best match by simply iterating all possible permutations of marker
    // indices. So, let's cache them for performance and brievity.
    static readonly int[][] permutations = new int [][] {
        new int[] {0, 1, 2},
        new int[] {0, 2, 1},
        new int[] {1, 0, 2},
        new int[] {1, 2, 0},
        new int[] {2, 0, 1},
        new int[] {2, 1, 0}
    };

    // We're going to visualize the matching process in Unity; we have _matchVisualization
    // and _matchByPositionVisualization for that. However, calls to 'match' and 'matchByPosition' will happen from
    // the tracking optimizer thread, not Unity thread; this means we must syncronize every access to the visualizers.
    private object _visualizationLocker = new object();
    private MatchVisualization _matchVisualization = new MatchVisualization();
    private MatchByPositionVisualization _matchByPositionVisualization = new MatchByPositionVisualization();

    //
    private List<Vector2> _markers;

    public ThreeMarkersEnvironment(IList<Vector2> markers) {
        if (markers.Count != 3)
            throw new System.Exception("markers.Count != 3");

        _markers = markers.ToList();
    }

    protected override void Destroy() {
        // We have nothing to destroy here.
    }

    Bool IEnvironment.isMutable() {
        return false;
    }

    Vector3[] IEnvironment.getMarkers() {
        return _markers.Select(m => new Vector3(m.x, 0, m.y)).ToArray();
    }

    Bool IEnvironment.filterRay(Vector3 up, Vector3 ray) {
        // We assume markers lay in the floor plane, so we can filter out everything above the horizon.
        return Vector3.Dot(up, ray) < 0;
    }

    Bool IEnvironment.match(Vector3[] raysUpSpace, out MarkerIndex[] markersIndices, out Pose poseOfUpSpace) {
        const float projectionsMatchTolerance = 0.05f; 

        markersIndices = Enumerable.Repeat(MarkerIndex.Unknown, raysUpSpace.Length).ToArray();
        poseOfUpSpace = new Pose();

        // For the sake of simplicity, we won't deal with cases when more than three rays is visible.
        if(raysUpSpace.Length != 3)
            return false;

        // rays projection on X-Y plane of "up space" at height of 1 meter
        var projections = projectRaysOnFloor(raysUpSpace);

        // We have to find transform that match rays projections to markers in world space in form f(x) = SR*x + t, where
        // SR - scale and rotation matrix: s*{{cos(r), -sin(r)}, {sin(r), cos(r)}}; s - scale coefficient, r - rotation angle,
        // t - translation vector.
        // Lets solve system of linear equations {SR*ai + t = bi} for SR and t
        // System can be rewritten as M*x = b
        //   / a0.x   -a0.y   1   0 \     / s*cos(r) \   / b0.x \
        //   | a0.y    a0.x   0   1 |  *  | s*sin(r) | = | b0.y |
        //   | ...                  |     | t.x      |   | ...  |
        //   \ ...                  /     \ t.y      /   \ ...  /
        var M = new float[6, 4];
        var b = new float[6];
        for (int i = 0; i < 3; ++i) {
            M[2*i, 0] = projections[i].x;
            M[2*i, 1] = -projections[i].y;
            M[2*i, 2] = 1;
            M[2*i + 1, 0] = projections[i].y;
            M[2*i + 1, 1] = projections[i].x;
            M[2*i + 1, 3] = 1;
            b[2*i] = _markers[i].x;
            b[2*i + 1] = _markers[i].y;
        }

        // left inverse
        var invM = M.transpose().multiply(M).inverse().multiply(M.transpose());

        // Iterate over all possible permutations and pick the best one if its good enough.
        int bestPermutationId = -1;
        float bestError = float.MaxValue;
        var bestTransform2d = new Transform2d();
        for (int idPermutation = 0; idPermutation < 6; ++idPermutation) {
            // make vector of constant terms for current markers order
            var curB = new float[6];
            for (int i = 0; i < 3; ++i) {
                int j = permutations[idPermutation][i];
                curB[2*i] = b[2*j];
                curB[2*i + 1] = b[2*j + 1];
            }

            var transformParams = invM.multiply(curB); // {scale*cos(r), scale*sin(r), t.x, t.y}
            var transform2d = new Transform2d(transformParams[0], transformParams[1], transformParams[2], transformParams[3]);
            float error = 0; // sum of square distances between corresponding markers and transformed rays projections
            for (int idMarker = 0; idMarker < 3; ++idMarker) {
                error += (transform2d.apply(projections[idMarker]) - _markers[permutations[idPermutation][idMarker]]).sqrMagnitude;
            }

            if (error < bestError) {
                bestError = error;
                bestPermutationId = idPermutation;
                bestTransform2d = transform2d;
            }
        }

        if (bestError > projectionsMatchTolerance)
            return false;

        for (int i = 0; i < raysUpSpace.Length; ++i) {
            markersIndices[i].value = (uint)permutations[bestPermutationId][i];
        }

        var position = new Vector3(bestTransform2d.Translate.x, bestTransform2d.Scale, bestTransform2d.Translate.y);
        var rotation = Quaternion.AxisAngle(Vector3.up, -bestTransform2d.Angle);
        poseOfUpSpace = new Pose(position, rotation);

        lock (_visualizationLocker) {
            _matchVisualization = new MatchVisualization(
                raysUpSpace.ToList(),
                poseOfUpSpace,
                projections.Select(p => bestTransform2d.apply(p)).ToList(),
                markersIndices);
        }

        return true;
    }

    MarkerIndex[] IEnvironment.matchByPosition(Vector3[] rays, Vector3 origin) {
        float sqrTolerance = 0.01f;
        var res = Enumerable.Repeat(MarkerIndex.Unknown, rays.Length).ToArray();

        // project rays on floor plane
        Vector2 shift = new Vector2(origin.x, origin.z);
        var projections = projectRaysOnFloor(rays, origin.y).Select(p => p + shift).ToList();

        for (int idMarker = 0; idMarker < 3; ++idMarker) {
            for (int idProjection = 0; idProjection < rays.Length; ++idProjection) {
                int numMatches = 0;
                if ((_markers[idMarker] - projections[idProjection]).sqrMagnitude < sqrTolerance){
                    res[idProjection].value = (uint)idMarker;
                    ++numMatches;
                }

                if(numMatches > 1)
                    res[idProjection] = MarkerIndex.Invalid;
            }
        }

        lock (_visualizationLocker) {
            _matchByPositionVisualization = new MatchByPositionVisualization(rays.ToList(), origin, res);
        }
        return res;
    }

    static List<Vector2> projectRaysOnFloor(IList<Vector3> rays, float height = 1) {
        return rays
            .Select(r => -height * new Vector2(r.x, r.z) / r.y)
            .ToList();
    }

    public MatchVisualization getMatchVisualization() {
        lock (_visualizationLocker) {
            return new MatchVisualization(_matchVisualization);
        }
    }

    public MatchByPositionVisualization getMatchByPositionVisualization() {
        lock (_visualizationLocker) {
            return new MatchByPositionVisualization(_matchByPositionVisualization);
        }
    }

    public class MatchByPositionVisualization {
        public List<Vector3> rays;
        public List<MarkerIndex> markersIndices;
        public Vector3 origin;

        public MatchByPositionVisualization() {
            rays = new List<Vector3>();
            markersIndices = new List<MarkerIndex>();
            origin = Vector3.zero;
        }

        public MatchByPositionVisualization(MatchByPositionVisualization other) {
            rays = other.rays.ToList();
            origin = other.origin;
            markersIndices = other.markersIndices.ToList();
        }

        public MatchByPositionVisualization(IList<Vector3> rays_, Vector3 origin_, IList<MarkerIndex> markersIndices_) {
            rays = rays_.ToList();
            origin = origin_;
            markersIndices = markersIndices_.ToList();
        }

        public void Draw(IList<Vector3> markers) {
            for (int i = 0; i < rays.Count; ++i) {
                var r = rays[i];
                r *= -origin.y / r.y;
                r += origin;
                var markerId = markersIndices[i];
                if (markerId == MarkerIndex.Unknown || markerId == MarkerIndex.Invalid) {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(origin, r);
                } else {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(origin, r);
                }
            }
        }
    }

    public class MatchVisualization {
        public List<Vector3> raysUpSpace;
        public List<MarkerIndex> markersIndices;
        public List<Vector2> projectedRays;
        public Pose poseOfUpSpace;

        public MatchVisualization() {
            raysUpSpace = new List<Vector3>();
            projectedRays = new List<Vector2>();
            markersIndices = new List<MarkerIndex>();
            poseOfUpSpace = new Pose();
        }

        public MatchVisualization(MatchVisualization other) {
            raysUpSpace = other.raysUpSpace.ToList();
            poseOfUpSpace = other.poseOfUpSpace;
            projectedRays = other.projectedRays.ToList();
            markersIndices = other.markersIndices.ToList();
        }

        public MatchVisualization(IList<Vector3> raysUpSpace_, Pose poseOfUpSpace_, IList<Vector2> projectedRays_, IList<MarkerIndex> markersIndices_) {
            raysUpSpace = raysUpSpace_.ToList();
            poseOfUpSpace = poseOfUpSpace_;
            projectedRays = projectedRays_.ToList();
            markersIndices = markersIndices_.ToList();
        }

        public void Draw(IList<Vector3> markers) {
            for (int i = 0; i < raysUpSpace.Count; ++i) {
                var r = poseOfUpSpace.rotation * raysUpSpace[i];
                r *= -poseOfUpSpace.position.y / r.y;
                r += poseOfUpSpace.position;
                var markerId = markersIndices[i];
                if (markerId == MarkerIndex.Unknown || markerId == MarkerIndex.Invalid) {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(poseOfUpSpace.position, r);
                } else {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(poseOfUpSpace.position, r);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(r, markers[(int)markerId.value]);
                }
            }

            Gizmos.color = Color.blue;
            for (int i = 0; i < projectedRays.Count; ++i) {
                DrawCross(projectedRays[i], 0.2f);
            }
        }

        public static void DrawCross(Vector2 position, float size) {
            Gizmos.DrawLine(new Vector3(position.x - size, 0, position.y), new Vector3(position.x + size, 0, position.y));
            Gizmos.DrawLine(new Vector3(position.x, 0, position.y - size), new Vector3(position.x, 0, position.y + size));
        }
    }
}
