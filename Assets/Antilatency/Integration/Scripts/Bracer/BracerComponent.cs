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

using Antilatency.DeviceNetwork;

namespace Antilatency.Integration {
    /// <summary>
    /// </summary>
    public enum BracerState {
        Disconnected,
        Connected
    }

    /// <summary>
    /// </summary>
    public enum BracerTouchState {
        Released,
        Pressed
    }

    /// <summary>
    /// Base abstract bracer class that implements core features.
    /// </summary>
    public abstract class BracerComponent : MonoBehaviour {
        [System.Serializable]
        public class BracerStateEvent : UnityEngine.Events.UnityEvent<BracerState> { }

        /// <summary>
        /// A link to the Device Network component.
        /// </summary>
        public DeviceNetwork Network;

        /// <summary>
        /// </summary>
        public BracerState CurrentState {
            get {
                return (_bracerCotask != null && !_bracerCotask.isTaskFinished()) ? BracerState.Connected : BracerState.Disconnected;
            }
        }

        /// <summary>
        /// A link to the bracer node.
        /// </summary>
        public Antilatency.DeviceNetwork.NodeHandle BracerNode { get; protected set; }

        /// <summary>
        /// Will be called every time bracer is connected or disconnected.
        /// </summary>
        public BracerStateEvent BracerStateChanged = new BracerStateEvent();

        protected Antilatency.Bracer.ILibrary _bracerLibrary;

        /// <summary>
        /// A link to the native bracer cotask.
        /// </summary>
        private Antilatency.Bracer.ICotask _bracerCotask;

        /// <summary>
        /// Start listening for Device Network updates and checks if any suitable tracking node exist.
        /// When you override this method in derived class, remember to call basic class method first. 
        /// </summary>
        protected virtual void OnEnable() {
            if (Network == null) {
                Debug.Log("Network is null");
                return;
            }

            if (_bracerLibrary == null) {
                _bracerLibrary = Antilatency.Bracer.Library.load();

                if (_bracerLibrary == null) {
                    Debug.LogError("Failed to create bracer library");
                    return;
                }
            }

            Network.DeviceNetworkChanged.AddListener(OnDeviceNetworkChanged);

            OnDeviceNetworkChanged();
        }

        /// <summary>
        /// Stop bracer task and listening for Device Network updates.
        /// When you override this method in derived class, remember to call basic class method first. 
        /// </summary>
        protected virtual void OnDisable() {
            if (Network == null) {
                return;
            }

            Network.DeviceNetworkChanged.RemoveListener(OnDeviceNetworkChanged);

            StopBracerTask();
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
        /// Listen for the Device Network update event.
        /// </summary>
        private void OnDeviceNetworkChanged() {
            if (_bracerCotask != null) {
                if (_bracerCotask.isTaskFinished()) {
                    StopBracerTask();
                } else {
                    return;
                }
            }

            if (_bracerCotask == null) {
                var node = GetAvailableBracerNode();
                if (node != Antilatency.DeviceNetwork.NodeHandle.Null) {
                    StartBracerTask(node);
                }
            } 
        }

        protected abstract NodeHandle GetAvailableBracerNode();

        /// <summary>
        /// Start bracer task on node.
        /// </summary>
        /// <param name="node">Node to start task on.</param>
        protected virtual void StartBracerTask(NodeHandle node) {
            var network = GetNativeNetwork();
            if (network == null) {
                return;
            }

            if (_bracerCotask != null) {
                Debug.LogWarningFormat("Bracer task already running on node {0}", BracerNode.value);
                return;
            }

            if (_bracerLibrary == null) {
                Debug.LogError("Bracer library is null");
                return;
            }

            if (network.nodeGetStatus(node) != NodeStatus.Idle) {
                Debug.LogError("Wrong node status");
                return;
            }

            using (var cotaskConstructor = _bracerLibrary.getCotaskConstructor()) {
                if (cotaskConstructor == null) {
                    Debug.LogError("Failed to get bracer cotask constructor");
                    return;
                }

                _bracerCotask = cotaskConstructor.startTask(network, node);
                if (_bracerCotask == null) {
                    StopBracerTask();
                    Debug.LogWarning("Failed to start bracer task on node " + node.value);
                    return;
                }

                BracerNode = node;
                BracerStateChanged.Invoke(BracerState.Connected);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool GetTouch(out float value) {
            value = 0.0f;

            if (_bracerCotask == null) {
                return false;
            }
            
            value = _bracerCotask.getTouch(0);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool ExecuteVibrarion(Antilatency.Bracer.Vibration[] vibrations) {
            if (_bracerCotask == null) {
                return false;
            }

            _bracerCotask.executeVibrationSequence(vibrations);
            return true;
        }

        /// <summary>
        /// Stop bracer task.
        /// When you override this method in derived class, remember to call basic class method first. 
        /// </summary>
        protected virtual void StopBracerTask() {
            BracerNode = new NodeHandle();

            if (_bracerCotask != null) {
                _bracerCotask.Dispose();
                _bracerCotask = null;

                BracerStateChanged.Invoke(BracerState.Disconnected);
            }
        }

        /// <summary>
        /// Checks for current state: if task becomes disconnected (bracer node has been disconnected), stop task.
        /// When you override this method in derived class, remember to call basic class method first. 
        /// </summary>
        protected virtual void Update() {
            if (_bracerCotask != null && _bracerCotask.isTaskFinished()) {
                StopBracerTask();
            }
        }

        private void OnDestroy() {
            if (_bracerCotask != null) {
                _bracerCotask.Dispose();
                _bracerCotask = null;
            }

            if (_bracerLibrary != null) {
                _bracerLibrary.Dispose();
                _bracerLibrary = null;
            }
        }
    }
}