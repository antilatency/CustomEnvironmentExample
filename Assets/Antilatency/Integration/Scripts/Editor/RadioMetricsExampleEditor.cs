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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace Antilatency.Integration {
    [CustomEditor(typeof(RadioMetricsExample))]
    public class RadioMetricsExampleEditor : Editor {
        private RadioMetricsExample _radioMetrics;

        private SerializedProperty _network;
        private SerializedProperty _showExtendedInfo;
        private void OnEnable() {
            _radioMetrics = (RadioMetricsExample)target;

            _network = serializedObject.FindProperty("Network");
            _showExtendedInfo = serializedObject.FindProperty("ShowExtendedMetrics");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_network);
            EditorGUILayout.PropertyField(_showExtendedInfo);
            serializedObject.ApplyModifiedProperties();

            if (!Application.isPlaying) {
                return;
            }

            if (_radioMetrics.UsbRadioSockets != null && _radioMetrics.UsbRadioSockets.Count != 0) {
                var usbSockets = _radioMetrics.UsbRadioSockets.Select(v => v.ToString()).ToArray();
                _radioMetrics.CurrentUsbSocketIndex = EditorGUILayout.Popup(label: "USB Radio Socket", selectedIndex: _radioMetrics.CurrentUsbSocketIndex, displayedOptions: usbSockets);
            } else {
                return;
            }

            if (_radioMetrics.TargetSockets != null && _radioMetrics.TargetSockets.Count != 0) {
                var targetSockets = _radioMetrics.TargetSockets.Select(v => v.ToString()).ToArray();
                _radioMetrics.CurrentTargetSocketIndex = EditorGUILayout.Popup(label: "Target Radio Socket", selectedIndex: _radioMetrics.CurrentTargetSocketIndex, displayedOptions: targetSockets);
            } else {
                return;
            }

            if (_radioMetrics.IsTaskRunning) {
                if (GUILayout.Button("Stop")) {
                    _radioMetrics.Stop();
                }
            } else {
                if (GUILayout.Button("Run")) {
                    _radioMetrics.Run();
                }
            }
        }
    }
}

