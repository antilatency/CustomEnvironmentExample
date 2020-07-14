using System.Collections;
using System.Collections.Generic;
using Antilatency.DeviceNetwork;
using UnityEngine;

namespace Antilatency.Integration {
    /// <summary>
    /// The Basic %Antilatency ALT tracking implementation searches for the first free ALT tracker and starts the tracking task. A pose will be applied to the GameObject with that component.
    /// </summary>
    public class AltTrackingDirect : AltTracking {

        /// <returns>The first idle tracking node if one exists, otherwise an invalid node.</returns>
        protected override NodeHandle GetAvailableTrackingNode() {
            return GetFirstIdleTrackerNode();
        }

        /// <returns>The placement pose marked as default in the AltSystem.</returns>
        protected override Pose GetPlacement() {
            var result = Pose.identity;

            using (var localStorage = StorageClient.GetLocalStorage()) {

                if (localStorage == null) {
                    return result;
                }

                var placementCode = localStorage.read("placement", "default");

                if (string.IsNullOrEmpty(placementCode)) {
                    Debug.LogError("Failed to get placement code");
                } else {
                    result = _trackingLibrary.createPlacement(placementCode);
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

