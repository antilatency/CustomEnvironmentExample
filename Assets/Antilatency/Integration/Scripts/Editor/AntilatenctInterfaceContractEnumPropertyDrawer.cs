using UnityEditor;
using UnityEngine;
using System.Linq;

public class AntilatenctInterfaceContractEnumPropertyDrawer<T> : PropertyDrawer {

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
