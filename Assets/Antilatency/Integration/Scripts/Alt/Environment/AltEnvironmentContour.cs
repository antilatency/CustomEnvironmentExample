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

using UnityEngine;

namespace Antilatency.Integration {
    /// <summary>
    /// Represents the environment contour (tracking zone border).
    /// </summary>
    public class AltEnvironmentContour : MonoBehaviour {
        /// <summary>
        /// Contour offset. If you change this field at runtime, call #UpdateMatrix after it to apply changes.
        /// </summary>
        public Vector3 Offset;

        /// <summary>
        /// Contour rotation. If you change this field at runtime, call #UpdateMatrix after it to apply changes.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Contour points list. 
        /// </summary>
        public List<Vector2> Points = new List<Vector2>(_defaultPoints);

        private Vector2 _offset;
        private float _rotation;

        private Matrix4x4 _transformmatrix;
        private static Vector2[] _defaultPoints = { new Vector2(1.0f, 1.0f),
                                                new Vector2(1.0f, -1.0f),
                                                new Vector2(-1.0f, -1.0f),
                                                new Vector2(-1.0f, 1.0f)};

        private void Awake() {
            UpdateMatrix();
        }

        /// <summary>
        /// Updates the transformation matrix of contour, call it after changing #Offset or #Rotation fields at runtime.
        /// </summary>
        public void UpdateMatrix() {
            var rotationMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis(Rotation, Vector3.up));
            var translationMatrix = Matrix4x4.Translate(new Vector3(Offset.x, Offset.y, Offset.z));

            _transformmatrix = translationMatrix * rotationMatrix;
        }

        public Vector2[] GetPoints() {
            var result = new Vector2[Points.Count];

            for (var i = 0; i < result.Length; ++i) {
                var point = _transformmatrix.MultiplyPoint(new Vector3(Points[i].x, 0.0f, Points[i].y));
                result[i] = new Vector2(point.x, point.z);
            }

            return result;
        }
    }
}