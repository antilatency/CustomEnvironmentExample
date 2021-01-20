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
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using Antilatency.DeviceNetwork;

namespace Antilatency.Integration {
    /// <summary>
    /// Provides base tracking functionality.
    /// </summary>
    public abstract class AltTracking : MonoBehaviour {
        [Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        /// <summary>
        /// A link to the DeviceNetwork component.
        /// </summary>
        public DeviceNetwork Network;

        /// <summary>
        /// A link to the Alt Environment component.
        /// </summary>
        public AltEnvironmentComponent Environment;

        /// <summary>
        /// Pose prediction time.
        /// </summary>
        public float ExtrapolationTime = 0.0f;

        /// <summary>
        /// This event will be invoked every time when tracking task starts or stops, sending true on start and false on stop.
        /// </summary>
        public BoolEvent TrackingTaskStateChanged = new BoolEvent();

        /// <summary>
        /// Has the tracking task been started.
        /// </summary>
        public bool TrackingTaskState {
            get {
                return _trackingCotask != null;
            }
        }

        /// <summary>
        /// A link to the Alt Tracking ILibrary native object.
        /// </summary>
        protected Alt.Tracking.ILibrary _trackingLibrary;

        /// <summary>
        /// Alt tracker to center eye point transformation.
        /// </summary>
        protected UnityEngine.Pose _placement;

        /// <summary>
        /// Current tracking node (Alt tracker device).
        /// </summary>
        protected NodeHandle _trackingNode;

        private Alt.Tracking.ITrackingCotask _trackingCotask;

        /// <summary>
        /// This method is used to call the initialization function.
        /// When you override this method in a derived class, remember to call the basic class method first.
        /// </summary>
        protected virtual void Awake() {
            Init();
        }

        private void Init() {
            _trackingNode = new NodeHandle();

            if (Network == null) {
                Debug.LogError("Network is null");
                return;
            }

            _trackingLibrary = Antilatency.Alt.Tracking.Library.load();

            if (_trackingLibrary == null) {
                Debug.LogError("Failed to create tracking library");
                return;
            }
        }

        /// <summary>
        /// Start the Device Network listening and check if any suitable tracking node exists.
        /// When you override this method in a derived class, remember to call the basic class method first.
        /// </summary>
        protected virtual void OnEnable() {
            if (Network == null) {
                Debug.Log("Network is null");
                return;
            }

            Network.DeviceNetworkChanged.AddListener(OnDeviceNetworkChanged);

            OnDeviceNetworkChanged();
        }

        /// <summary>
        /// Stop tracking and listening for Device Network updates.
        /// When you override this method in a derived class, remember to call the basic class method first.
        /// </summary>
        protected virtual void OnDisable() {
            if (Network == null) {
                return;
            }

            Network.DeviceNetworkChanged.RemoveListener(OnDeviceNetworkChanged);

            StopTracking();
        }

        private void OnDeviceNetworkChanged() {
            if (_trackingCotask != null) {
                if (_trackingCotask.isTaskFinished()) {
                    StopTracking();
                } else {
                    return;
                }
            }

            if (_trackingCotask == null) {
                var node = GetAvailableTrackingNode();
                if (node != Antilatency.DeviceNetwork.NodeHandle.Null) {
                    StartTracking(node);
                }
            } 
        }

        /// <summary>
        /// Checks the state of the tracking task and performs cleanup if the tracking task has finished.
        /// When you override this method in a derived class, remember to call the basic class method first.
        /// </summary>
        protected virtual void Update() {
            if (_trackingCotask != null && _trackingCotask.isTaskFinished()) {
                StopTracking();
                return;
            }
        }

        /// <summary>
        /// Cleanup at component destroy. 
        /// When you override this method in a derived class, remember to call the basic class method first.
        /// </summary>
        protected virtual void OnDestroy() {
            if (_trackingCotask != null) {
                _trackingCotask.Dispose();
                _trackingCotask = null;
            }

            if (_trackingLibrary != null) {
                _trackingLibrary.Dispose();
                _trackingLibrary = null;
            }
        }

        /// <returns>Native IFactory object.</returns>
        protected INetwork GetNativeNetwork() {
            if (Network == null) {
                Debug.LogError("Network is null");
                return null;
            }

            if (Network.NativeNetwork == null) {
                Debug.LogError("Native network is null");
                return null;
            }

            return Network.NativeNetwork;
        }

        /// <summary>
        /// Start tracking task on node.
        /// </summary>
        /// <param name="node">Node to start tracking task.</param>
        protected void StartTracking(NodeHandle node) {
            var network = GetNativeNetwork();
            if (network == null) {
                return;
            }

            if (network.nodeGetStatus(node) != NodeStatus.Idle) {
                Debug.LogError("Wrong node status");
                return;
            }

            if (Environment == null) {
                Debug.LogError("Environment is null");
                return;
            }

            var nativeEnvironment = Environment.GetEnvironment();
            if (nativeEnvironment == null) {
                Debug.LogError("Native environment is null");
                return;
            }

            _placement = GetPlacement();

            using (var cotaskConstructor = _trackingLibrary.createTrackingCotaskConstructor()) {
                _trackingCotask = cotaskConstructor.startTask(network, node, nativeEnvironment);

                if (_trackingCotask == null) {
                    StopTracking();
                    Debug.LogWarning("Failed to start tracking task on node " + node.value);
                    return;
                }

                _trackingNode = node;
                TrackingTaskStateChanged.Invoke(true);

            }
        }

        /// <summary>
        /// Get the current raw tracking state without extrapolation and placement correction applied.
        /// </summary>
        /// <param name="state"> [out] result tracking state.</param>
        /// <returns>True if tracking is running, otherwise false.</returns>
        public bool GetRawTrackingState(out Antilatency.Alt.Tracking.State state) {
            state = new Antilatency.Alt.Tracking.State();
            if (_trackingCotask == null) {
                return false;
            }

            state = _trackingCotask.getState(Antilatency.Alt.Tracking.Constants.DefaultAngularVelocityAvgTime);
            return true;
        }

        /// <summary>
        /// Get the tracking state extrapolated and corrected by placement pose.
        /// </summary>
        /// <param name="state"> [out] result tracking state.</param>
        /// <returns>True if tracking is running, otherwise false.</returns>
        public bool GetTrackingState(out Antilatency.Alt.Tracking.State state) {
            state = new Antilatency.Alt.Tracking.State();
            if (_trackingCotask == null) {
                return false;
            }

            state = _trackingCotask.getExtrapolatedState(_placement, ExtrapolationTime);
            return true;
        }

        /// <summary>
        /// Stop the tracking task.
        /// </summary>
        protected void StopTracking() {
            if (_trackingCotask == null) {
                return;
            }

            _trackingCotask.Dispose();
            _trackingCotask = null;
            _trackingNode = new NodeHandle();

            TrackingTaskStateChanged.Invoke(false);
        }

        /// <summary>
        /// Get a node (ALT tracker device) to start tracking task.
        /// </summary>
        /// <returns>The node to start tracking if it exists, otherwise an invalid node.</returns>
        protected abstract NodeHandle GetAvailableTrackingNode();

        /// <returns>The pose that will be applied to tracking data when using the GetTrackingState method.</returns>
        protected abstract Pose GetPlacement();

        #region Usefull methods to search nodes

        /// <summary>
        /// Searches for all idle tracker nodes.
        /// </summary>
        /// <returns>The array of currently idle tracker nodes.</returns>
        protected NodeHandle[] GetIdleTrackerNodes() {
            var nativeNetwork = GetNativeNetwork();

            if (nativeNetwork == null) {
                return new NodeHandle[0];
            }

            using (var cotaskConstructor = _trackingLibrary.createTrackingCotaskConstructor()) {
                var nodes = cotaskConstructor.findSupportedNodes(nativeNetwork).Where(v =>
                        nativeNetwork.nodeGetStatus(v) == NodeStatus.Idle
                    ).ToArray();

                return nodes;
            }
        }

        /// <summary>
        /// Searches for the first idle tracking node.
        /// </summary>
        /// <returns>The first idle tracking node if it exists, otherwise NodeHandle with value = -1 (InvalidNode).</returns>
        protected NodeHandle GetFirstIdleTrackerNode() {
            var nodes = GetIdleTrackerNodes();
            if (nodes.Length == 0) {
                return new NodeHandle();
            }
            return nodes[0];
        }

        /// <summary>
        /// Searches for all idle tracker nodes that are directly connected to USB socket.
        /// </summary>
        /// <returns>The array of idle tracker nodes connected directly to USB sockets.</returns>
        protected NodeHandle[] GetUsbConnectedIdleTrackerNodes() {
            var nativeNetwork = GetNativeNetwork();

            if (nativeNetwork == null) {
                return new NodeHandle[0];
            }

            using (var cotaskConstructor = _trackingLibrary.createTrackingCotaskConstructor()) {
                var nodes = cotaskConstructor.findSupportedNodes(nativeNetwork).Where(v =>
                        nativeNetwork.nodeGetParent(nativeNetwork.nodeGetParent(v)) == Antilatency.DeviceNetwork.NodeHandle.Null &&
                        nativeNetwork.nodeGetStatus(v) == NodeStatus.Idle
                        ).ToArray();

                return nodes;
            }
        }

        /// <summary>
        /// Get the first idle tracker node connected directly to USB socket.
        /// </summary>
        /// <returns>The first idle tracker node connected directly to USB socket if exists, otherwise NodeHandle with value = -1 (InvalidNode).</returns>
        protected NodeHandle GetUsbConnectedFirstIdleTrackerNode() {
            var nodes = GetUsbConnectedIdleTrackerNodes();

            if (nodes.Length == 0) {
                return new NodeHandle();
            }

            return nodes[0];
        }

        /// <summary>
        /// Searches for idle tracking nodes which socket is marked with <paramref name="socketTag"/>.
        /// </summary>
        /// <param name="socketTag">Socket "tag" property value.</param>
        /// <returns>The array of idle tracking nodes connected to sockets marked with <paramref name="socketTag"/>.</returns>
        protected NodeHandle[] GetIdleTrackerNodesBySocketTag(string socketTag) {
            var nativeNetwork = GetNativeNetwork();

            if (nativeNetwork == null) {
                return new NodeHandle[0];
            }

            using (var cotaskConstructor = _trackingLibrary.createTrackingCotaskConstructor()) {
                var nodes = cotaskConstructor.findSupportedNodes(nativeNetwork).Where(v =>
                        nativeNetwork.nodeGetStringProperty(nativeNetwork.nodeGetParent(v), "Tag") == socketTag &&
                        nativeNetwork.nodeGetStatus(v) == NodeStatus.Idle
                        ).ToArray();

                return nodes;
            }
        }

        /// <summary>
        /// Searches for the idle tracking node which socket is marked with <paramref name="socketTag"/>.
        /// </summary>
        /// <param name="socketTag">Socket "tag" property value.</param>
        /// <returns>The first idle tracking nodes connected to socket marked with <paramref name="socketTag"/>.</returns>
        protected NodeHandle GetFirstIdleTrackerNodeBySocketTag(string socketTag) {
            var nodes = GetIdleTrackerNodesBySocketTag(socketTag);

            if (nodes.Length == 0) {
                return new NodeHandle();
            }

            return nodes[0];
        }

#endregion
    }
}