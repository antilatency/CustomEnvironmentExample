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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using Antilatency.DeviceNetwork;

namespace Antilatency.Integration {
    /// <summary>
    /// Antilatency.DeviceNetwork.INetwork wrapper for Unity
    /// </summary>
    public class DeviceNetwork : MonoBehaviour {
        /// <summary>
        /// Array of UsbDeviceType's that will be used by this DeviceNetwork. Changing this field at runtime will have no effect.
        /// </summary>
        public UsbDeviceType[] SupportedDeviceTypes = new UsbDeviceType[] {
                new UsbDeviceType { vid = UsbVendorId.Antilatency, pid = 0x0000 },
                new UsbDeviceType { vid = UsbVendorId.AntilatencyLegacy, pid = 0x0000 }
            };

        /// <summary>
        /// Pointer to native INetwork object.
        /// </summary>
        public INetwork NativeNetwork {
            get {
                if (_nativeNetwork == null) {
                    Init();
                }

                return _nativeNetwork;
            }
        }

        /// <summary>
        /// If any supported device has been connected or disconnected, this event will be invoked.
        /// </summary>
        public UnityEvent DeviceNetworkChanged = new UnityEvent();

        private ILibrary _library;
        private INetwork _nativeNetwork;
        private uint _lastUpdateId = 0;

        private void Awake() {
            if (_nativeNetwork == null) {
                Init();
            }
        }

        private void Init() {
            var otherNetworks = FindObjectsOfType<DeviceNetwork>().Where(v => v != this).ToArray();
            foreach (var network in otherNetworks) {
                foreach (var usbType in SupportedDeviceTypes) {
                    if (network.SupportedDeviceTypes.Contains(usbType)) {
                        Debug.LogErrorFormat("DeviceNetwork with {0} has been already created", usbType);
                        return;
                    }
                }
            }

            _library = Antilatency.DeviceNetwork.Library.load();
            if (_library == null) {
                Debug.LogError("Failed to load Antilatency Device Network library");
                return;
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            var jni = _library.QueryInterface<AndroidJniWrapper.IAndroidJni>();
            using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity")) {
                    jni.initJni(IntPtr.Zero, activity.GetRawObject());
                }
            }
            jni.Dispose();
#endif
            _library.setLogLevel(LogLevel.Info);
            _nativeNetwork = _library.createNetwork(SupportedDeviceTypes);

            if (_nativeNetwork == null) {
                Debug.LogError("Failed to create Antilatency Device Network");
            }
        }

        private void Update() {
            if (_nativeNetwork == null) {
                return;
            }

            var updateId = _nativeNetwork.getUpdateId();
            if (updateId != _lastUpdateId) {
                _lastUpdateId = updateId;
                DeviceNetworkChanged.Invoke();
            }
        }

        private void OnDestroy() {
            if (_nativeNetwork != null) {
                _nativeNetwork.Dispose();
                _nativeNetwork = null;
            }

            if (_library != null) {
                _library.Dispose();
                _library = null;
            }
        }
    }
}
