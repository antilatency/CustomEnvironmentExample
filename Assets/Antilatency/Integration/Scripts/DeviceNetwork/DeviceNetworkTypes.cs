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

using System;
using System.Collections.Generic;

namespace Antilatency.DeviceNetwork {
    public partial struct UsbDeviceType {

        public static bool operator ==(UsbDeviceType u1, UsbDeviceType u2) {
            return u1.Equals(u2);
        }

        public static bool operator !=(UsbDeviceType u1, UsbDeviceType u2) {
            return !u1.Equals(u2);
        }

        public bool Equals(UsbDeviceType other) {
            return Equals(other, this);
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            var objToCompare = (UsbDeviceType)obj;

            return vid == objToCompare.vid && pid == objToCompare.pid;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 3;
                hash = hash * 5 + vid.GetHashCode();
                hash = hash * 7 + pid.GetHashCode();
                return hash;
            }
        }

        public override string ToString() {
            return string.Format("Vid: {0}, pid: {1}", vid, pid);
        }
    }
}

