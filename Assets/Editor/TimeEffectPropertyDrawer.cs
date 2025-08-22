#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

//To highlight overridden fields
[CustomPropertyDrawer(typeof(TimeEffectProperty<>))]
public class TimeEffectPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var target = property.serializedObject.targetObject;
        SerializedProperty instanceValueProp = property.FindPropertyRelative("instanceValue");

        if (target is TimeEffectPresetSO)
        {
            EditorGUI.PropertyField(position, instanceValueProp, label, true);
            return;
        }

        SerializedProperty presetValueProp = property.FindPropertyRelative("presetValue");
        SerializedProperty overridden = property.FindPropertyRelative("isOverridden");

        //bool isOverridden = instanceValueProp.Equals(presetValueProp);
        bool isOverridden = overridden.boolValue;

        //Rect fieldRect = new Rect(position.x, position.y, position.width - 60, position.height);
        //EditorGUI.PropertyField(fieldRect, instanceValueProp, label);

        //Rect presetRect = new Rect(position.x + position.width - 60, position.y, 50, position.height);
        //EditorGUI.LabelField(presetRect, isOverridden ? "preset" : "not", isOverridden ? EditorStyles.boldLabel : EditorStyles.label);

        //Rect buttonRect = new Rect(position.x + position.width - 60, position.y + 18, 50, position.height);
        //if (isOverridden && GUI.Button(buttonRect, "Reset"))
        //{
        //    instanceValueProp.objectReferenceValue = presetValueProp.objectReferenceValue;
        //    instanceValueProp.serializedObject.ApplyModifiedProperties();
        //}

        float buttonWidth = 50f;
        float spacing = 5f;
        float presetLabelWidth = 130f;

        Rect instanceRect = new Rect(position.x, position.y,
    position.width - presetLabelWidth - (isOverridden ? buttonWidth + spacing : 0) - spacing, position.height);
        Rect presetLabelRect = new Rect(instanceRect.xMax + spacing, position.y,
            presetLabelWidth, position.height);
        Rect buttonRect = new Rect(presetLabelRect.xMax + spacing, position.y,
            buttonWidth, position.height);

        //Curve is always marked as overridden, but still need Reset button since it does work
        if (isOverridden && instanceValueProp.propertyType != SerializedPropertyType.AnimationCurve)
        {
            EditorGUI.DrawRect(instanceRect, Color.green * 0.5f);
            EditorGUI.PropertyField(instanceRect, instanceValueProp, label);
        }
        else
        {
            EditorGUI.PropertyField(instanceRect, instanceValueProp, label);
        }

        string presetDisplay = GetPropertyValueString(presetValueProp);
        string statusText = isOverridden ? "Override" : "Default";
        GUIStyle labelStyle = isOverridden ? EditorStyles.boldLabel : EditorStyles.label;

        EditorGUI.LabelField(presetLabelRect, $"Preset: ({presetDisplay})", labelStyle);

        if (isOverridden)
        {
            if (GUI.Button(buttonRect, "Reset"))
            {
                //CopyPropertyValue(presetValueProp, instanceValueProp);
                //instanceValueProp.objectReferenceValue = presetValueProp.objectReferenceValue;
                CopyPropertyValue(presetValueProp, instanceValueProp);
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }

    private string GetPropertyValueString(SerializedProperty prop)
    {
        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                return prop.intValue.ToString();
            case SerializedPropertyType.Boolean:
                return prop.boolValue.ToString();
            case SerializedPropertyType.Float:
                return prop.floatValue.ToString("F2");
            case SerializedPropertyType.Enum:
                return prop.enumNames[prop.enumValueIndex];
            case SerializedPropertyType.AnimationCurve:
                var curve = prop.animationCurveValue;
                return curve != null ? $"Curve ({curve.keys.Length} keys)" : "null";
            default:
                return prop.propertyType.ToString();
        }
    }

    private void CopyPropertyValue(SerializedProperty source, SerializedProperty target)
    {
        switch (source.propertyType)
        {
            case SerializedPropertyType.Float:
                target.floatValue = source.floatValue;
                break;
            case SerializedPropertyType.Integer:
                target.intValue = source.intValue;
                break;
            case SerializedPropertyType.Boolean:
                target.boolValue = source.boolValue;
                break;
            case SerializedPropertyType.AnimationCurve:
                target.animationCurveValue = new AnimationCurve(source.animationCurveValue.keys);
                break;
            case SerializedPropertyType.Enum:
                target.enumValueIndex = source.enumValueIndex;
                break;
            case SerializedPropertyType.ObjectReference:
                target.objectReferenceValue = source.objectReferenceValue;
                break;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var target = property.serializedObject.targetObject;

        if (target is TimeEffectPresetSO)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        return EditorGUIUtility.singleLineHeight;
    }
}
#endif
