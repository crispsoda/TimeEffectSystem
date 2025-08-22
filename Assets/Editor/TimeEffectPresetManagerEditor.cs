using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeEffectPresetManager))]
public class TimeEffectPresetManagerEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();

    //    TimeEffectPresetManager manager = (TimeEffectPresetManager)target;

    //    if (manager.scriptable == null)
    //    {
    //        return;
    //    }

    //    EditorGUILayout.Space();
    //    EditorGUILayout.LabelField("Presets in Profile", EditorStyles.boldLabel);

    //    var profile = manager.scriptable;
    //    foreach (var preset in profile.effectPresets)
    //    {
    //        EditorGUILayout.BeginVertical("box");
    //        EditorGUILayout.LabelField(preset.preset.ToString(), EditorStyles.boldLabel);
    //        EditorGUILayout.LabelField("Description: " + preset.description);
    //        EditorGUILayout.LabelField("Layer: " + preset.layer.ToString());
    //        if (preset.effect != null)
    //        {
    //            EditorGUILayout.LabelField("TimeScale: " + preset.effect.timeScale.instanceValue);
    //            EditorGUILayout.LabelField("Timed Effect?: " + preset.effect.timedEffect.instanceValue);
    //            EditorGUILayout.LabelField("Transition On Expire?: " + preset.effect.transitionOnExpire.instanceValue);
    //            EditorGUILayout.LabelField("Duration: " + preset.effect.duration.instanceValue);
    //            EditorGUILayout.LabelField("Curve: " + preset.effect.curve.instanceValue);
    //            EditorGUILayout.LabelField("Use Curve Duration?: " + preset.effect.useCurveDuration.instanceValue);
    //            EditorGUILayout.LabelField("Priority: " + preset.effect.priority.instanceValue);
    //            EditorGUILayout.LabelField("Can Be Overridden?: " + preset.effect.canOverride.instanceValue);
    //            EditorGUILayout.LabelField("Max Active Instances Across All Presets?: " + preset.effect.maxActiveInstancesUsePreset.instanceValue);
    //            EditorGUILayout.LabelField("Max Active Instances: " + preset.effect.maxActiveInstances.instanceValue);
    //        }
    //        EditorGUILayout.EndVertical();
    //    }
    //}
}
