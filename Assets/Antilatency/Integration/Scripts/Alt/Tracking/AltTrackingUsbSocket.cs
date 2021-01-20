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
using Antilatency.DeviceNetwork;
using UnityEngine;

namespace Antilatency.Integration {
    /// <summary>
    /// The sample component that starts a tracking task on the Alt that is connected to a USB socket.
    /// </summary>
    public class AltTrackingUsbSocket : AltTracking {

        /// <returns>The first idle tracking node connected to a USB socket.</returns>
        protected override NodeHandle GetAvailableTrackingNode() {
            return GetUsbConnectedFirstIdleTrackerNode();
        }

        /// <returns>The pose which was created from AltSystem's placement that is marked as default.</returns>
        protected override Pose GetPlacement() {
            var result = Pose.identity;

            using (var localStorage = StorageClient.GetLocalStorage()) {

                if (localStorage != null) {
                    var placementCode = localStorage.read("placement", "default");

                    if (string.IsNullOrEmpty(placementCode)) {
                        Debug.LogError("Failed to get placement code, identity pose will be used.");
                    } else {
                        result = _trackingLibrary.createPlacement(placementCode);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Apply tracking data to a component's GameObject transform.
        /// </summary>
        protected override void Update() {
            base.Update();

            Antilatency.Alt.Tracking.State trackingState;

            if (!GetTrackingState(out trackingState)) {
                return;
            }

            transform.localPosition = trackingState.pose.position;
            transform.localRotation = trackingState.pose.rotation;
        }
    }
}