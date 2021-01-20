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

using UnityEditor;
using UnityEngine;
using System.Linq;

public class AntilatencyInterfaceContractEnumPropertyDrawer<T> : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        const int intEditWidth = 32;
        var popupPosition = position;
        popupPosition.xMax -= intEditWidth;
        var intPosition = position;
        intPosition.xMin = popupPosition.xMax;
        var target = property.serializedObject.targetObject;
        var targetType = target.GetType();

        var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
            .Where(x => x.FieldType == typeof(T)).ToArray();

        var staticFieldsStructs = fields.Select(x => x.GetValue(null)).ToArray();
        var staticFieldsValues = staticFieldsStructs.Select(x => System.Convert.ToInt64(x.GetType().GetField("value").GetValue(x))).ToArray();

        long value = property.FindPropertyRelative("value").longValue;

        if (staticFieldsValues.Any(x => x == value)) {
            var staticFieldsNames = fields.Select(x => x.Name).ToArray();

            var selectedValue = System.Array.FindIndex(staticFieldsValues,x => x == value);


            selectedValue = EditorGUI.Popup(position, label.text, selectedValue, staticFieldsNames);
            value = staticFieldsValues[selectedValue];
            property.FindPropertyRelative("value").longValue = value;
        }
        else {
            var staticFieldsNames = fields.Select(x => x.Name).Concat(new string[] { "out of range" }).ToArray();

            var selectedValue = EditorGUI.Popup(popupPosition, label.text, staticFieldsNames.Length-1, staticFieldsNames);
            if (selectedValue != (staticFieldsNames.Length-1)) {
                value = staticFieldsValues[selectedValue];
            }
            EditorGUI.indentLevel = 0;
            value = EditorGUI.LongField(intPosition,value);
            property.FindPropertyRelative("value").longValue = value;
        }

    }
}
