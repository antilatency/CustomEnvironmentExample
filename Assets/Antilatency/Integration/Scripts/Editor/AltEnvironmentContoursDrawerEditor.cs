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

using UnityEngine;
using UnityEditor;

namespace Antilatency.Integration {
    [CustomEditor(typeof(AltEnvironmentContoursDrawer))]
    public class AltEnvironmentContoursDrawerEditor : Editor {
        private SerializedProperty _environment;
        private SerializedProperty _contoursTarget;
        private SerializedProperty _enableContourOffset;
        private SerializedProperty _contourOffset;
        private SerializedProperty _useCustomContours;
        private SerializedProperty _customContours;

        private GUIStyle _headerStyle;

        private void OnEnable() {
            _environment = serializedObject.FindProperty("Environment");
            _contoursTarget = serializedObject.FindProperty("Target");
            _enableContourOffset = serializedObject.FindProperty("EnableContourOffset");
            _contourOffset = serializedObject.FindProperty("ContourOffset");
            _useCustomContours = serializedObject.FindProperty("UseCustomContours");
            _customContours = serializedObject.FindProperty("CustomContours");
        }

        private void InitStyles() {
            _headerStyle = new GUIStyle(GUI.skin.label);
            _headerStyle.fontStyle = FontStyle.Bold;
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            if (_headerStyle == null) {
                InitStyles();
            }

            EditorGUILayout.PropertyField(_environment);

            EditorGUILayout.PropertyField(_contoursTarget);
            EditorGUILayout.PropertyField(_enableContourOffset);
            if (_enableContourOffset.boolValue) {
                EditorGUILayout.PropertyField(_contourOffset);
            }

            GUILayout.Label("Currently only custom contours are supported", _headerStyle);
            EditorGUILayout.PropertyField(_useCustomContours);
            if (_useCustomContours.boolValue) {
                EditorGUILayout.PropertyField(_customContours, true);

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}