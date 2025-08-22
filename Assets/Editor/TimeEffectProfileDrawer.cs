#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TimeEffectProfile))]
public class TimeEffectProfileDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        bool isInPresetSO = property.serializedObject.targetObject is TimeEffectPresetSO;

        SerializedProperty effectIDProp = property.FindPropertyRelative("effectID");
        SerializedProperty layerProp = property.FindPropertyRelative("layer");
        SerializedProperty presetProp = property.FindPropertyRelative("preset");
        SerializedProperty descriptionProp = property.FindPropertyRelative("description");
        SerializedProperty effectProp = property.FindPropertyRelative("effect");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new Rect(position.x, position.y, position.width, lineHeight);

        //Hide effectID and layer if inside PresetSO
        if (!isInPresetSO)
        {
            EditorGUI.PropertyField(rect, effectIDProp);
            rect.y += lineHeight + spacing;

            EditorGUI.PropertyField(rect, layerProp);
            rect.y += lineHeight + spacing;
        }

        EditorGUI.PropertyField(rect, presetProp);
        rect.y += lineHeight + spacing;

        EditorGUI.PropertyField(rect, descriptionProp);
        rect.y += lineHeight + spacing;

        if(isInPresetSO)
        {
            EditorGUI.indentLevel++;
            DrawTimeEffect(rect, effectProp);
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel = 2;
            DrawTimeEffect(rect, effectProp);
            EditorGUI.indentLevel = 0;
        }


            EditorGUI.EndProperty();
    }

    private void DrawTimeEffect(Rect position, SerializedProperty effectProp)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new Rect(position.x, position.y, position.width, lineHeight);

        SerializedProperty timedEffectProp = effectProp.FindPropertyRelative("timedEffect.instanceValue");
        bool isTimed = timedEffectProp != null && timedEffectProp.boolValue;

        void DrawTimeEffectField(string fieldName, string labelOverride = null)
        {
            SerializedProperty prop = effectProp.FindPropertyRelative(fieldName);
            if (prop != null)
            {
                EditorGUI.PropertyField(rect, prop, new GUIContent(labelOverride ?? prop.displayName));
                rect.y += lineHeight + spacing;
            }
        }

        DrawTimeEffectField("timedEffect");

        if (isTimed)
        {
            DrawTimeEffectField("duration");
            DrawTimeEffectField("curve", "TimeScale Curve");
            DrawTimeEffectField("blendWithNext");
            DrawTimeEffectField("useCurveDuration");
            DrawTimeEffectField("priority");
            DrawTimeEffectField("canOverride");
            DrawTimeEffectField("maxActiveInstancesUsePreset");
            DrawTimeEffectField("maxActiveInstances");
        }
        else
        {
            DrawTimeEffectField("timeScale");
            DrawTimeEffectField("transitionOnExpire");
            DrawTimeEffectField("curve", "Transition Curve");
            DrawTimeEffectField("priority");
            DrawTimeEffectField("canOverride");
            DrawTimeEffectField("maxActiveInstancesUsePreset");
            DrawTimeEffectField("maxActiveInstances");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool isInPresetSO = property.serializedObject.targetObject is TimeEffectPresetSO;

        int lines = 3; 
        if (!isInPresetSO) lines += 2; 

        SerializedProperty effectProp = property.FindPropertyRelative("effect");
        SerializedProperty timedEffectProp = effectProp.FindPropertyRelative("timedEffect.instanceValue");
        bool isTimed = timedEffectProp != null && timedEffectProp.boolValue;

        int timeEffectLines = isTimed ? 7 : 7;
        lines += timeEffectLines;

        float padding = 18f;

        return lines * EditorGUIUtility.singleLineHeight + (lines - 1) * EditorGUIUtility.standardVerticalSpacing + padding;
    }
}
#endif
