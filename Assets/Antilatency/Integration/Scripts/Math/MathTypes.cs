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

namespace Antilatency.Math {
    public partial struct doubleQ {
        public Quaternion ToQuaternion() {
            return new Quaternion((float)x, (float)y, (float)z, (float)w);
        }

        public static doubleQ FromQuaternion(Quaternion q) {
            return new doubleQ {
                x = (double)q.x,
                y = (double)q.y,
                z = (double)q.z,
                w = (double)q.w
            };
        }

        public static bool operator ==(doubleQ q1, doubleQ q2) {
            return q1.Equals(q2);
        }

        public static bool operator !=(doubleQ q1, doubleQ q2) {
            return !q1.Equals(q2);
        }

        public bool Equals(doubleQ other) {
            return Equals(other, this);
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            var objToCompare = (doubleQ)obj;

            var precision = 0.000001f;

            return System.Math.Abs(x - objToCompare.x) < precision &&
                    System.Math.Abs(y - objToCompare.y) < precision &&
                    System.Math.Abs(z - objToCompare.z) < precision &&
                    System.Math.Abs(w - objToCompare.w) < precision;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 3;
                hash = hash * 5 + x.GetHashCode();
                hash = hash * 7 + y.GetHashCode();
                hash = hash * 9 + z.GetHashCode();
                hash = hash * 11 + w.GetHashCode();
                return hash;
            }
        }

        public override string ToString() {
            return string.Format("({0}, {1}, {2}, {3})", x, y, z, w);
        }
    }
}