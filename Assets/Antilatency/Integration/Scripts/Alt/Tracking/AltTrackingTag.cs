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
    /// The sample component that starts the tracking task on an Alt connected to a socket (bracer, tag) tagged with <paramref name="Tag"/>.
    /// </summary>
    public class AltTrackingTag : AltTracking {
        public string SocketTag;
        public Vector3 PlacementOffset;
        public Vector3 PlacementRotation;

        /// <returns>The first idle tracking node connected to the socket is tagged with <paramref name="Tag"/>.</returns>
        protected override NodeHandle GetAvailableTrackingNode() {
            return GetFirstIdleTrackerNodeBySocketTag(SocketTag);
        }

        /// <returns>The pose that is created from PlacementOffset and PlacementRotation.</returns>
        protected override Pose GetPlacement() {
            return new Pose(PlacementOffset, Quaternion.Euler(PlacementRotation));
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