using UnityEngine;
using System.Collections.Generic;

public class TimeEffectCaller : MonoBehaviour
{
    [SerializeField] 
    private List<TimeEffectProfile> caller = new List<TimeEffectProfile>();

    public void AddEffect(string effectID)
    {
        var found = FindEffect(effectID);
        //TimeScaleManagerOLD.Instance.RequestTimeEffect(found);
        TimeEffectTracker.Instance.AddTimeEffect(found);
    }

    private TimeEffectProfile FindEffect(string effectName)
    {
        if(caller.Count == 0)
            return null;

        //if (caller.Count == 1)
        //    return caller[0].effect;

        foreach (var effect in caller)
        {
            if (effect == null || string.IsNullOrWhiteSpace(effect.effectID))
            {
                continue;
            }

            if(effect.effectID == effectName)
            {
                return effect;
            }
        }

        Debug.LogWarning($"No effect found with effectName {effectName}");
        return null;
    }

    private void OnValidate()
    {
        foreach(var entry in caller)
        {
            entry.effect.SetParent(this);

            if(entry.preset != TimeEffectPresetID.None)
            {
                Debug.Log("got to thing");
                ApplyPreset(entry);
            }
        }
    }

    private void ApplyPreset(TimeEffectProfile target)
    {
        //if(TimeEffectPresetManager.instance == null)
        //{
        //    Debug.LogWarning("no preset manager");
        //    return;
        //}

        var presetSO = Resources.Load<TimeEffectPresetSO>("TimeEffectPresets");
        if(presetSO == null)
        {
            Debug.LogWarning("presetSO is null");
            return;
        }

        var preset = presetSO.GetPresetEffect(target.preset);

        if (preset == null)
        {
            Debug.LogWarning("preset returned null");
            return;
        }

        target.effect.timeScale.SetPresetValue(preset.timeScale.instanceValue);
        target.effect.timeScale.SetOverrideBool(target.effect.timeScale.instanceValue != target.effect.timeScale.presetValue);

        target.effect.timedEffect.SetPresetValue(preset.timedEffect.instanceValue);
        target.effect.timedEffect.SetOverrideBool(target.effect.timedEffect.instanceValue != target.effect.timedEffect.presetValue);

        target.effect.transitionOnExpire.SetPresetValue(preset.transitionOnExpire.instanceValue);
        target.effect.transitionOnExpire.SetOverrideBool(target.effect.transitionOnExpire.instanceValue != target.effect.transitionOnExpire.presetValue);

        target.effect.duration.SetPresetValue(preset.duration.instanceValue);
        target.effect.duration.SetOverrideBool(target.effect.duration.instanceValue != target.effect.duration.presetValue);

        target.effect.curve.SetPresetValue(preset.curve.instanceValue);
        target.effect.curve.SetOverrideBool(target.effect.curve.instanceValue != target.effect.curve.presetValue);

        target.effect.useCurveDuration.SetPresetValue(preset.useCurveDuration.instanceValue);
        target.effect.useCurveDuration.SetOverrideBool(target.effect.useCurveDuration.instanceValue != target.effect.useCurveDuration.presetValue);

        target.effect.priority.SetPresetValue(preset.priority.instanceValue);
        target.effect.priority.SetOverrideBool(target.effect.priority.instanceValue != target.effect.priority.presetValue);

        target.effect.canOverride.SetPresetValue(preset.canOverride.instanceValue);
        target.effect.canOverride.SetOverrideBool(target.effect.canOverride.instanceValue != target.effect.canOverride.presetValue);

        target.effect.maxActiveInstancesUsePreset.SetPresetValue(preset.maxActiveInstancesUsePreset.instanceValue);
        target.effect.maxActiveInstancesUsePreset.SetOverrideBool(target.effect.maxActiveInstancesUsePreset.instanceValue != target.effect.maxActiveInstancesUsePreset.presetValue);

        target.effect.maxActiveInstances.SetPresetValue(preset.maxActiveInstances.instanceValue);
        target.effect.maxActiveInstances.SetOverrideBool(target.effect.maxActiveInstances.instanceValue != target.effect.maxActiveInstances.presetValue);

        Debug.Log("preset applied");
    }

    public void ReapplyPresets()
    {
        foreach (var entry in caller)
        {
            if (entry != null && entry.preset != TimeEffectPresetID.None)
            {
                ApplyPreset(entry);
            }
        }
    }
}