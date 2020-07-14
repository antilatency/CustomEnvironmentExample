using System.Linq;
using Antilatency.Alt.Tracking;
using Antilatency.InterfaceContract;
using UnityEngine;

// This is stub class to illustrate and document the operation principles of custom environments.
// See ThreeMarkersEnvironment for a functional example.
public class CustomEnvironment : Antilatency.InterfaceContract.InterfacedObject, Antilatency.Alt.Tracking.IEnvironment {

    protected override void Destroy() {
        // Each interfaced object has internal reference counter to track its lifetime (it's designed to coexist
        // with C# garbage collector, though). When the reference counter goes back to zero, the Destroy() method
        // is called prior to the object deallocation. Note that this method may be called from a different thread
        // than it was created; don't forget to protect your code with a mutex if necessary.
    }

    // Environments can be mutable and immutable. The environment is called imutable if all its markers are well-fixed
    // at their positions and cannot move during tracking (or they can, but don't care implementing a runtime
    // compensation for their movements). If your environment is immutable, just return 'false' here; otherwise
    // return 'true' and make you environment additionally implement Antilatency.Alt.Tracking.IEnvironmentMutable.
    Bool IEnvironment.isMutable() {
        return false;
    }

    // Return an array of every marker's world-space position. The order markers are returned in is important: it must
    // match the way your IEnvironment.match() works.
    Vector3[] IEnvironment.getMarkers() {
        return new Vector3[0];
    }

    // User can filter out some rays before the match phase. Return 'true' if you want the given ray to be processed
    // further, or 'false' otherwise. This method can be used as a shortcut test to fast-reject rays to markers which
    // cannot be seen from the given Alt orientation *in principle* (for example, the ones with ray.z < 0).
    // Parameters:
    //   up - inertial up (the direction, opposite to the gravity force) in local space, normalized;
    //   ray - direction from Alt to a marker in local space, normalized.
    //
    // Local space is the left-handed coordinates, such that:
    //   - the origin is fixed at the Alt's position;
    //   - axis Z is directed along the line of the camera view;
    //   - axis Y is directed to the flat side of the Alt's body.
    Bool IEnvironment.filterRay(Vector3 up, Vector3 ray) {
        return true;
    }

    // Imlement actual match logic. The function takes an array of rays and tries to find: (a) their correspondence
    // to the markes, and (b) the world-space orientation of the Alt. It returns 'true' on success, or 'false' if it
    // was unable to come to a solution for any reason.
    //
    // Parameters:
    //   raysUpSpace -
    //     an array of observed rays, normalized. Unlike the ray parameter of 'filterRay', these rays are given in
    //     so-called "up space" coordinates. That we call an "up space" is just the local space coordinates system,
    //     shortest-path rotated to make local inertial up exactly (0, 1, 0).
    //
    //   markersIndices -
    //     the calculated correspondence between the rays in 'raysUpSpace' and the markers. Basically,
    //     it's just an array of unsigned ints, such that:
    //
    //         if markerIndices[i] == j, then raysUpSpace[i] is found to correspond to (getMarkers())[j];
    //         if markerIndices[i] == Unknown or Invalid, then no correspondence found for the i-th ray.
    //
    //   poseOfUpSpace -
    //     the calculated pose of the "up space" (not local space!) coordinates in world space.
    Bool IEnvironment.match(Vector3[] raysUpSpace, out MarkerIndex[] markersIndices, out Pose poseOfUpSpace) {
        markersIndices = Enumerable.Repeat(MarkerIndex.Unknown, raysUpSpace.Length).ToArray();
        poseOfUpSpace = new Pose();
        return false;
    }

    // Usually, tracking only needs to call IEnvironment.match in situations it's completely lost. Most of the
    // time, however, it can guess its approximate world space position using the IMU and asks the environment
    // to only perform the ray correspondence matching, not the whole process.
    //
    // This function should take an array of rays and the known Alt position, and try to find and return the
    // correspondence between the rays and markers. In the function fails to find the correspondence, it should
    // return an array of 'Unknown' values.
    //
    // Parameters:
    //   rays - an array of observed rays in *local* space, normalized;
    //   origin - known Alt position in world space, in meters;
    MarkerIndex[] IEnvironment.matchByPosition(Vector3[] rays, Vector3 origin) {
        return Enumerable.Repeat(MarkerIndex.Unknown, rays.Length).ToArray();
    }
}
