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