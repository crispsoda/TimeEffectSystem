#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeEffectPreset", menuName = "ScriptableObjects/TimeEffectPreset")]
public class TimeEffectPresetSO : ScriptableObject
{
    public List<TimeEffectProfile> effectPresets = new List<TimeEffectProfile>();

    //Retrieve the correct hitstop profile based on its intensity
    public TimeEffect GetPresetEffect(TimeEffectPresetID presetID)
    {
        if(effectPresets == null || effectPresets.Count == 0)
        {
            Debug.LogWarning("TimeEffects list empty in TimeEffectProfile");
            return null;
        }

        foreach(var entry in effectPresets)
        {
            if(entry.preset == presetID)
            {
                return entry.effect;
            }
        }

        Debug.LogWarning($"HitstopProfile of effectID '{presetID}' not found!");
        return null;
    }

    public int GetMaxInstanceAmountFromPreset(TimeEffectPresetID presetID)
    {
        if (effectPresets == null || effectPresets.Count == 0)
        {
            Debug.LogWarning("TimeEffects list empty in TimeEffectProfile");
            return 0;
        }

        foreach (var entry in effectPresets)
        {
            if (entry.preset == presetID)
            {
                return entry.effect.maxActiveInstances.instanceValue;
            }
        }

        Debug.LogWarning($"No MaxInstanceAmount found for preset {presetID}. Defaulting to 3");
        return 3;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        //PopulateList();

        var callers = FindObjectsByType<TimeEffectCaller>(FindObjectsSortMode.None);
        foreach (var caller in callers)
        {
            caller.ReapplyPresets();
        }

        Debug.Log("list validated");
    }

    private void PopulateList()
    {
        //Array enumValues = Enum.GetValues(typeof(TimeEffectPresetID));
        //var validEnumValues = enumValues.Cast<TimeEffectPresetID>()
        //    .Where(id => id != TimeEffectPresetID.None)
        //    .ToList();

        ////Remove entries with preset = None
        //effectPresets.RemoveAll(e => e.preset == TimeEffectPresetID.None);

        ////Remove duplicates by keeping only the first occurrence of each preset
        //var uniquePresets = new List<TimeEffectProfile>();
        //var seenPresets = new HashSet<TimeEffectPresetID>();

        //foreach (var preset in effectPresets)
        //{
        //    if (seenPresets.Add(preset.preset))
        //    {
        //        uniquePresets.Add(preset);
        //    }
        //}
        //effectPresets = uniquePresets;

        ////Sort list by enum order
        //effectPresets.Sort((a, b) => a.preset.CompareTo(b.preset));




    //    if (effectPresets == null)
    //        effectPresets = new List<TimeEffectProfile>();

    //    //Remove duplicates
    //    effectPresets = effectPresets
    //.GroupBy(e => e.preset)
    //.Select(g => g.First()) // or choose Last() if you want newest to win
    //.ToList();


    }
#endif

}