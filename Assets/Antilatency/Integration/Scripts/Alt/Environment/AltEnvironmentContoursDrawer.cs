// Copyright 2020, ALT LLC. All Rights Reserved.
// This file is part of Antilatency SDK.
// It is subject to the license terms in the LICENSE file found in the top-level directory
// of this distribution and at http://www.antilatency.com/eula
// You may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Antilatency.Integration {
    /// <summary>
    /// Draws tracking zone contours in the scene. Currently, only custom contours are supported, we will add the ability to get contours from IEnvironment in future releases.
    /// </summary>
    public class AltEnvironmentContoursDrawer : MonoBehaviour {
        /// <summary>
        /// A link to the Alt Environment component.
        /// </summary>
        public AltEnvironmentComponent Environment;

        /// <summary>
        /// A link to the GameObject's transform that will be used to determine the visibility of contours.
        /// </summary>
        public Transform Target;

        /// <summary>
        /// Controls if #ContourOffset will be applied to contours.
        /// </summary>
        [Tooltip("Enables offset for contours")]
        public bool EnableContourOffset = false;

        /// <summary>
        /// Offset that will be applied to contours. A positive value will move the contours inside the playable area and vice versa.
        /// </summary>
        [Tooltip("Offset that will be applied to contours. A positive value will move the contours inside the playable area and vice versa.")]
        public float ContourOffset;

        /// <summary>
        /// Overrides the default zone borders.
        /// </summary>
        public bool UseCustomContours = false;

        /// <summary>
        /// List of custom contours.
        /// </summary>
        public List<AltEnvironmentContour> CustomContours = new List<AltEnvironmentContour>();

        private const float _contoursHeight = 2.0f;
        private Material _contoursMaterial;
        private string _propName = "_UserPosition";
        private List<List<Vector2>> _contourPoints = new List<List<Vector2>>();
        private List<GameObject> _contours = new List<GameObject>();
        private List<Mesh> _contourMeshes = new List<Mesh>();

        private void OnEnable() {
            SetContoursVisible(true);
        }

        private void OnDisable() {
            SetContoursVisible(false);
        }

        private void SetContoursVisible(bool visible) {
            foreach (var contour in _contours) {
                contour.SetActive(visible);
            }
        }

        private void Start() {
            if (Environment == null) {
                Debug.LogError("Environment is null");
            }

            if (_contoursMaterial == null) {
                _contoursMaterial = Resources.Load<Material>("AltTrackingBoundaries");
            }
        }

        private void Update() {
            if (Environment == null) {
                return;
            }

            var nativeEnvironment = Environment.GetEnvironment();
            if (nativeEnvironment == null) {
                Debug.LogError("Native environment is null");
                return;
            }

            if (nativeEnvironment != null) {
                var contoursCount = GetContoursCount();

                if (contoursCount > 0) {
                    var equal = true;

                    var contourPoints = GetContoursPoints();

                    if (_contourPoints.Count == contoursCount) {
                        for (var i = 0; i < contourPoints.Count; i++) {
                            if (_contourPoints[i].Count == contourPoints[i].Count) {
                                equal = !_contourPoints[i].Except(contourPoints[i]).Any() && equal;
                            } else {
                                equal = false;
                            }
                        }
                    } else {
                        equal = false;
                    }

                    if (_contoursMaterial != null) {
                        if (!equal) {
                            CreateContoursMesh(contourPoints);
                        }
                    }
                }

                if (Target != null ) {
                    _contoursMaterial.SetVector(_propName, Target.position);
                }
            }
        }

        private int GetContoursCount() {
            int result = 0;

            if (UseCustomContours) {
                return CustomContours.FindAll(v => v != null).Count;
            }

            return result;
        }

        private List<List<Vector2>> GetContoursPoints() {
            var result = new List<List<Vector2>>();
            if (UseCustomContours) {
                for (var i = 0; i < CustomContours.Count; ++i) {
                    if (CustomContours[i] == null) {
                        continue;
                    }

                    result.Add(CustomContours[i].GetPoints().ToList());
                }
            }

            return result;
        }

        private void CreateContoursMesh(List<List<Vector2>> contourPoints) {
            if (EnableContourOffset) {
                ApplyContourOffset(ref contourPoints);
            }

            //add more
            if (contourPoints.Count > _contours.Count) {
                for (var i = _contours.Count; i < contourPoints.Count; ++i) {

                    var contour = new GameObject("Contour #" + i);
                    var meshFilter = contour.AddComponent<MeshFilter>();

                    var mesh = new Mesh();
                    _contourMeshes.Add(mesh);
                    meshFilter.mesh = mesh;

                    var rnd = contour.AddComponent<MeshRenderer>();
                    rnd.sharedMaterial = _contoursMaterial;
                    _contours.Add(contour);
                    contour.transform.SetParent(transform);
                    contour.transform.localRotation = Quaternion.identity;
                    contour.transform.localPosition = Vector3.zero;
                }
            }

            //remove
            while (_contours.Count > contourPoints.Count) {
                Destroy(_contourMeshes[0]);
                _contourMeshes.RemoveAt(0);
                Destroy(_contours[0]);
                _contours.RemoveAt(0);
            }

            //generate borders
            for (var i = 0; i < contourPoints.Count; i++) {
                _contourMeshes[i].name = "Contour #" + i + " mesh";

                var vertices = new List<Vector3>();
                var uvs = new List<Vector2>();
                var uvs2 = new List<Vector2>();
                var normals = new List<Vector3>();
                var tris = new List<int>();

                for (var j = 0; j < contourPoints[i].Count; ++j) {
                    var pointA = contourPoints[i][j];
                    var pointB = contourPoints[i][(j + 1) % contourPoints[i].Count];
                    ConstructQuad(pointA, pointB, ref vertices, ref uvs, ref uvs2, ref normals, ref tris);
                }

                _contourMeshes[i].vertices = vertices.ToArray();
                _contourMeshes[i].uv = uvs.ToArray();
                _contourMeshes[i].uv2 = uvs2.ToArray();
                _contourMeshes[i].triangles = tris.ToArray();
                _contourMeshes[i].normals = normals.ToArray();
            }

            _contourPoints = contourPoints;
        }

        private void ConstructQuad(Vector2 pointA, Vector2 pointB, ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<Vector2> uvs2, ref List<Vector3> normals, ref List<int> tris) {
            var firstVertex = vertices.Count;

            vertices.Add(new Vector3(pointA.x, 0.0f, pointA.y));
            vertices.Add(new Vector3(pointA.x, _contoursHeight, pointA.y));
            vertices.Add(new Vector3(pointB.x, 0.0f, pointB.y));
            vertices.Add(new Vector3(pointB.x, _contoursHeight, pointB.y));

            var normal = Vector3.Cross(vertices[firstVertex + 1] - vertices[firstVertex], vertices[firstVertex + 2] - vertices[firstVertex]).normalized;
            for (var i = 0; i < 4; ++i) {
                normals.Add(normal);
            }

            var quadLength = Vector3.Distance(vertices[firstVertex], vertices[firstVertex + 2]);
            var uvOffset = quadLength / _contoursHeight;

            uvs.Add(new Vector2(0.0f, 0.0f));
            uvs.Add(new Vector2(0.0f, 1.0f));
            uvs.Add(new Vector2(uvOffset, 0.0f));
            uvs.Add(new Vector2(uvOffset, 1.0f));

            uvs2.Add(new Vector2(quadLength, _contoursHeight));
            uvs2.Add(new Vector2(quadLength, -_contoursHeight));
            uvs2.Add(new Vector2(-quadLength, _contoursHeight));
            uvs2.Add(new Vector2(-quadLength, -_contoursHeight));

            tris.AddRange(new int[3] { firstVertex, firstVertex + 1, firstVertex + 3 });
            tris.AddRange(new int[3] { firstVertex, firstVertex + 3, firstVertex + 2 });
        }

        private void ApplyContourOffset(ref List<List<Vector2>> contourPoints) {
            for (int i = 0; i < contourPoints.Count; i++) {
                var movedPoints = new List<Vector2>(contourPoints[i]);
                for (int j = 0; j < contourPoints[i].Count; j++) {
                    var p1 = contourPoints[i][j];
                    var p2 = contourPoints[i][(j + 1) % contourPoints[i].Count];
                    var p3 = contourPoints[i][(j + 2) % contourPoints[i].Count];

                    var v1 = (p2 - p1).normalized;
                    v1 = new Vector2(v1.y, -v1.x);
                    var v2 = (p3 - p2).normalized;
                    v2 = new Vector2(v2.y, -v2.x);

                    var p1Moved = p1 + v1 * ContourOffset;
                    var p2Moved = p2 + v1 * ContourOffset;
                    var p3Moved = p2 + v2 * ContourOffset;
                    var p4Moved = p3 + v2 * ContourOffset;

                    var result = new Vector3(p2.x, 0.0f, p2.y);

                    GetLinesIntersection(p1Moved, p2Moved, p3Moved, p4Moved, ref result);
                    movedPoints[(j + 1) % contourPoints[i].Count] = result;

                }
                contourPoints[i] = movedPoints;
            }
        }

        private bool GetLinesIntersection(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, ref Vector3 result) {
            if (Vector3.Angle(v2 - v1, v4 - v3) < 0.01f) {
                return false;
            }

            var p1 = new Vector3(v2.x - v1.x, v2.y - v1.y, v2.z - v1.z);
            var p2 = new Vector3(v4.x - v3.x, v4.y - v3.y, v4.z - v3.z);

            var x = (v1.x * p1.y * p2.x - v3.x * p2.y * p1.x - v1.y * p1.x * p2.x + v3.y * p1.x * p2.x) / (p1.y * p2.x - p2.y * p1.x);
            var y = (v1.y * p1.x * p2.y - v3.y * p2.x * p1.y - v1.x * p1.y * p2.y + v3.x * p1.y * p2.y) / (p1.x * p2.y - p2.x * p1.y);
            var z = (v1.z * p1.y * p2.z - v3.z * p2.y * p1.z - v1.z * p1.z * p2.z + v3.y * p1.z * p2.z) / (p1.y - p2.z - p2.y * p1.z);

            result = new Vector3(x, y, z);

            return true;
        }
    }
}

