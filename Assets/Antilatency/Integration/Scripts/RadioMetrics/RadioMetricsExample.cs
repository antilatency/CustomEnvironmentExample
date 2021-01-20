// Copyright (c) 2020 ALT LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of source code located below and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Antilatency.Integration {

    /// <summary>
    /// Example how to use the Radio Metrics task. Works only at editor in play mode.
    /// </summary>
    public class RadioMetricsExample : MonoBehaviour {

        public class DeviceInfo {
            public string HardwareType = null;
            public string SerialNumber = null;
            public Antilatency.DeviceNetwork.NodeHandle Node = Antilatency.DeviceNetwork.NodeHandle.Null;

            public DeviceInfo(string hardwareType, string serialNumber, Antilatency.DeviceNetwork.NodeHandle node) {
                HardwareType = hardwareType;
                SerialNumber = serialNumber;
                Node = node;
            }

            public override string ToString() {
                return string.Format("Type: {0} | SN: {1} | Handle: {2}", HardwareType, SerialNumber, Node.value);
            }
        }

        /// <summary>
        /// A link to the DeviceNetwork component.
        /// </summary>
        public Antilatency.Integration.DeviceNetwork Network;

        /// <summary>
        /// Show simplified or extended metrics.
        /// </summary>
        public bool ShowExtendedMetrics;

        /// <summary>
        /// List of currently connected USB Radio Sockets.
        /// </summary>
        public List<DeviceInfo> UsbRadioSockets {
            get; private set;
        }

        /// <summary>
        /// List of currently connected Radio Sockets (Tags or Bracers) to the selected USB Radio Socket.
        /// </summary>
        public List<DeviceInfo> TargetSockets {
            get; private set;
        }

        /// <summary>
        /// Index of the selected USB Radio Socket, that shall be used to run Radio Metrics task.
        /// </summary>
        public int CurrentUsbSocketIndex {
            get {
                return _currentUsbSocketIndex;
            }
            set {
                if (_currentUsbSocketIndex != value) {
                    _currentUsbSocketIndex = value;
                    if (IsTaskRunning) {
                        Stop();
                    }
                    UpdateTargetSocketsList();
                }
            }
        }

        /// <summary>
        /// Index of the selected Radio Socket (Tag or Bracer), that shall be used to get Radio Metrics.
        /// </summary>
        public int CurrentTargetSocketIndex;

        public bool IsTaskRunning {
            get {
                return _radioMetricsCotask != null;
            }
        }

        private Antilatency.RadioMetrics.ILibrary _radioMetricsLibrary;
        private Antilatency.RadioMetrics.ICotask _radioMetricsCotask;

        private int _currentUsbSocketIndex;

        private void Start() {
            if (Network == null) {
                Debug.LogError("Device Network is null");
                this.enabled = false;
                return;
            }

            _radioMetricsLibrary = Antilatency.RadioMetrics.Library.load();
            if (_radioMetricsLibrary == null) {
                Debug.LogError("Failed to get RadioMetrics ILibrary");
                this.enabled = false;
                return;
            }

            Network.DeviceNetworkChanged.AddListener(OnDeviceNetworkChanged);
            UpdateUsbSocketsLists();
        }

        private void OnDeviceNetworkChanged() {
            UpdateUsbSocketsLists();
            UpdateTargetSocketsList();
        }

        private void UpdateUsbSocketsLists() {
            string currentSocketSn = null;

            // Dispose cotask if task has been already finished.
            if (IsTaskRunning && _radioMetricsCotask.isTaskFinished()) {
                Stop();
            }

            if (UsbRadioSockets == null) {
                UsbRadioSockets = new List<DeviceInfo>();
            } else {
                // Save currently selected USB Radio Socket serial number.
                if (UsbRadioSockets.Count > CurrentUsbSocketIndex) {
                    currentSocketSn = UsbRadioSockets[CurrentUsbSocketIndex].SerialNumber;
                }

                UsbRadioSockets.Clear();
            }

            using (var cotaskConstructor = _radioMetricsLibrary.getCotaskConstructor()) {
                if (cotaskConstructor == null) {
                    Debug.LogError("Failed to get Radio Metrics Cotask Constructor");
                    return;
                }

                var nativeNetwork = Network.NativeNetwork;
                if (nativeNetwork == null) {
                    Debug.LogError("Native network is null");
                    return;
                }

                // Find all nodes that supports Radio Metrics task.
                var usbSockets = cotaskConstructor.findSupportedNodes(nativeNetwork);
                foreach (var usbSocket in usbSockets) {
                    var hardwareType = nativeNetwork.nodeGetStringProperty(usbSocket, Antilatency.DeviceNetwork.Interop.Constants.HardwareNameKey);
                    var serialNumber = nativeNetwork.nodeGetStringProperty(usbSocket, Antilatency.DeviceNetwork.Interop.Constants.HardwareSerialNumberKey);

                    UsbRadioSockets.Add(new DeviceInfo(hardwareType, serialNumber, usbSocket));
                }
            }

            // Try to find previously selected node in the updated list.
            if (!string.IsNullOrEmpty(currentSocketSn)) {
                var currentUsbSocketId = UsbRadioSockets.FindIndex(v => v.SerialNumber == currentSocketSn);
                if (currentUsbSocketId == -1) {
                    _currentUsbSocketIndex = 0;
                } else {
                    // Node found, use it's index as a default value.
                    _currentUsbSocketIndex = currentUsbSocketId;
                }
            } else {
                // Node not found, just use the first one from the list.
                _currentUsbSocketIndex = 0;
            }

            ForceUiUpdate();
        }

        private void UpdateTargetSocketsList() {
            string currentSocketSn = null;

            if (TargetSockets == null) {
                TargetSockets = new List<DeviceInfo>();
            } else {
                if (TargetSockets.Count > CurrentTargetSocketIndex) {
                    // Save currently selected target Radio Socket serial number.
                    currentSocketSn = TargetSockets[CurrentTargetSocketIndex].SerialNumber;
                }

                TargetSockets.Clear();
            }

            // If there are no USB Radio Sockets connected, skip searching.
            if (UsbRadioSockets == null || UsbRadioSockets.Count == 0) {
                return;
            }

            var nativeNetwork = Network.NativeNetwork;
            if (nativeNetwork == null) {
                Debug.LogError("Native network is null");
                return;
            }

            if (UsbRadioSockets.Count <= CurrentUsbSocketIndex) {
                Debug.LogError("UsbRadioSockets index error.");
                return;
            }

            var socketNode = UsbRadioSockets[CurrentUsbSocketIndex].Node;

            if (socketNode == Antilatency.DeviceNetwork.NodeHandle.Null) {
                return;
            }

            // Search for child node, that is connected to the selected USB Radio Socket.
            var childNodes = nativeNetwork.getNodes().Where(v => nativeNetwork.nodeGetParent(v) == socketNode).ToArray();
            foreach (var node in childNodes) {
                var hardwareType = nativeNetwork.nodeGetStringProperty(node, Antilatency.DeviceNetwork.Interop.Constants.HardwareNameKey);
                // Filter all child nodes by their hardware type.
                if (hardwareType != "AltTag" && hardwareType != "AltBracer") {
                    continue;
                }

                var serialNumber = nativeNetwork.nodeGetStringProperty(node, Antilatency.DeviceNetwork.Interop.Constants.HardwareSerialNumberKey);
                TargetSockets.Add(new DeviceInfo(hardwareType, serialNumber, node));
            }

            // Try to find previously selected node in the updated list.
            if (!string.IsNullOrEmpty(currentSocketSn)) {
                var currentTargetSocketId = TargetSockets.FindIndex(v => v.SerialNumber == currentSocketSn);
                if (currentTargetSocketId == -1) {
                    CurrentTargetSocketIndex = 0;
                } else {
                    // Node found, use it's index as a default value.
                    CurrentTargetSocketIndex = currentTargetSocketId;
                }
            } else {
                // Node not found, just use the first one from the list.
                CurrentTargetSocketIndex = 0;
            }

            ForceUiUpdate();
        }

        private void ForceUiUpdate() {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Start Radio Metrics task on the selected USB Radio Socket.
        /// </summary>
        public void Run() {
            if (_radioMetricsLibrary == null) {
                return;
            }

            if (UsbRadioSockets == null || UsbRadioSockets.Count == 0) {
                return;
            }

            if (TargetSockets == null || TargetSockets.Count == 0) {
                return;
            }

            var cotaskConstructor = _radioMetricsLibrary.getCotaskConstructor();
            if (cotaskConstructor == null) {
                return;
            }

            var network = Network.NativeNetwork;

            _radioMetricsCotask = cotaskConstructor.startTask(network, UsbRadioSockets[CurrentUsbSocketIndex].Node);
        }

        /// <summary>
        /// Stop the Radio Metrics task.
        /// </summary>
        public void Stop() {
            if (_radioMetricsCotask != null) {
                _radioMetricsCotask.Dispose();
                _radioMetricsCotask = null;
            }
        }

        private void Update() {
            if (!IsTaskRunning) {
                return;
            }

            if (TargetSockets.Count == 0) {
                return;
            }

            var targetNode = TargetSockets[CurrentTargetSocketIndex].Node;

            if (ShowExtendedMetrics) {
                RadioMetrics.Interop.ExtendedMetrics metrics;
                try {
                    metrics = _radioMetricsCotask.getExtendedMetrics(targetNode);
                    Debug.Log($"Tx (bytes): {metrics.txBytes}, Tx packets count: {metrics.txPacketsCount}, " +
                        $"Rx bytes: {metrics.rxBytes}, Rx packets count: {metrics.rxPacketsCount}, flow count: {metrics.flowCount}, " +
                        $"average RSSI: {metrics.averageRssi} dBm, min RSSI: {metrics.minRssi} dBm, max RSSI: {metrics.maxRssi} dBm, " +
                        $"missed packets: {metrics.missedPacketsCount}, failed packets: {metrics.failedPacketsCount}");
                }
                catch (Exception e) {
                    Debug.Log($"{e.Message}");
                    return;
                }
            } else {
                RadioMetrics.Metrics metrics;
                try {
                    metrics = _radioMetricsCotask.getMetrics(targetNode);
                    Debug.Log($"Average RSSI: {metrics.averageRssi} dBm, packet loss rate: {metrics.packetLossRate.ToString("0.00")}%");
                }
                catch (Exception e) {
                    Debug.Log($"{e.Message}");
                    return;
                }
            }
        }

        /// <summary>
        /// Cleanup on component destroy.
        /// </summary>
        private void OnDestroy() {
            Stop();

            if (_radioMetricsLibrary != null) {
                _radioMetricsLibrary.Dispose();
                _radioMetricsLibrary = null;
            }
        }
    }
}