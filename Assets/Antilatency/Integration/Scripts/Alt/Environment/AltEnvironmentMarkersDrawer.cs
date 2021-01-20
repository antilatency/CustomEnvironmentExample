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
    /// Draws the tracking zone markers in the scene.
    /// </summary>
    public class AltEnvironmentMarkersDrawer : MonoBehaviour {
        /// <summary>
        /// A link to the Alt Environment component.
        /// </summary>
        public AltEnvironmentComponent Environment;

        private Material _markerMaterial;
        private List<GameObject> _markers = new List<GameObject>();
        private Mesh _markerMesh;
        private int _mutableEnvironmentUpdateId = 0;

        private void OnEnable() {
            SetMarkersVisible(true);
        }

        private void OnDisable() {
            SetMarkersVisible(false);
        }

        private void SetMarkersVisible(bool visible) {
            foreach (var marker in _markers) {
                marker.SetActive(visible);
            }
        }

        private void Start() {
            if (Environment == null) {
                Debug.LogError("Environment is null");
            }

            _markerMaterial = Resources.Load<Material>("AltTrackingMarkers");
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

            var envMarkers = nativeEnvironment.getMarkers();

            var markerPositionUpdateRequired = false;

            if (envMarkers.Length != _markers.Count) {
                if (envMarkers.Length > _markers.Count) {
                    for (var i = _markers.Count; i < envMarkers.Length; ++i) {
                        _markers.Add(CreateMarker());
                    }
                } else if (envMarkers.Length < _markers.Count) {
                    while (envMarkers.Length != _markers.Count) {
                        Destroy(_markers.Last());
                        _markers.RemoveAt(_markers.Count - 1);
                    }
                }
                markerPositionUpdateRequired = true;
            }

            if (nativeEnvironment.isMutable()) {
                var mutableEnvironment = nativeEnvironment.QueryInterface<Antilatency.Alt.Tracking.IEnvironmentMutable>();
                if (mutableEnvironment != null) {
                    var curUpdateId = mutableEnvironment.getUpdateId();
                    if (curUpdateId != _mutableEnvironmentUpdateId) {
                        _mutableEnvironmentUpdateId = curUpdateId;
                        markerPositionUpdateRequired = true;
                    }
                } else {
                    Debug.LogError("Failed to query IEnvironmentMutable interface while environment is marked as mutable");
                }
            }

            if (markerPositionUpdateRequired) {
                for (var i = 0; i < _markers.Count; ++i) {
                    var marker = _markers[i];

                    marker.transform.localPosition = envMarkers[i];
                }
            }
        }

        private GameObject CreateMarker() {
            var result = new GameObject("Marker");

            result.transform.SetParent(transform);
            result.transform.localRotation = Quaternion.identity;
            result.transform.localPosition = Vector3.zero;

            if (_markerMesh == null) {
                GenerateMarkerMesh();
            }
            var markerMeshFilter = result.AddComponent<MeshFilter>();
            var barMeshRenderer = result.AddComponent<MeshRenderer>();
            markerMeshFilter.sharedMesh = _markerMesh;

            barMeshRenderer.sharedMaterials = new Material[1] { _markerMaterial };

            return result;
        }

        private void GenerateMarkerMesh() {
            var markerRadius = 0.02f;
            var markerYOffset = 0.005f;
            var markerSegmentsCount = 8;

            var vertices = new Vector3[markerSegmentsCount + 1];
            var uvs = new Vector2[vertices.Length];
            var triangles = new int[markerSegmentsCount * 3];

            var centerVertexIndex = 0;
            vertices[centerVertexIndex] = Vector3.zero;
            uvs[centerVertexIndex] = new Vector2(0.5f, 0.5f);

            for (var i = 0; i < markerSegmentsCount; ++i) {
                var currentVertexIndex = centerVertexIndex + i + 1;
                var firstTriangleIndex = i * 3;

                var angle = i * 2 * Mathf.PI / markerSegmentsCount;
                vertices[centerVertexIndex + i + 1] = new Vector3(vertices[centerVertexIndex].x + markerRadius * Mathf.Cos(angle), markerYOffset, vertices[centerVertexIndex].z + markerRadius * Mathf.Sin(angle));
                uvs[currentVertexIndex] = new Vector2(0.5f + 0.5f * Mathf.Cos(angle), 0.5f + 0.5f * Mathf.Sin(angle));

                triangles[firstTriangleIndex] = centerVertexIndex;
                triangles[firstTriangleIndex + 1] = i == markerSegmentsCount - 1 ? centerVertexIndex + 1 : currentVertexIndex + 1;
                triangles[firstTriangleIndex + 2] = currentVertexIndex;
            }

            _markerMesh = new Mesh();
            _markerMesh.name = "Bar";

            _markerMesh.vertices = vertices;
            _markerMesh.uv = uvs;
            _markerMesh.subMeshCount = 1;

            _markerMesh.SetTriangles(triangles, 0);
        }
    }
}