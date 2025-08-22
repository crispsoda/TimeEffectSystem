using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TimeEffectPresetManager : ScriptableSingleton<TimeEffectPresetManager>
{
    //Contains the presets and finds based on ID, then applies to whatever script is calling them

    //public so Editor script can access to display values in inspector
    //public TimeEffectProfileSO scriptable;

    ////Singleton
    //public static TimeEffectPresetManager Instance { get; private set; }

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

    public List<TimeEffectProfile> effectPresets = new List<TimeEffectProfile>();

    //Retrieve the correct hitstop profile based on its intensity
    public TimeEffect GetPreset(TimeEffectPresetID presetID)
    {
        if (effectPresets == null || effectPresets.Count == 0)
        {
            Debug.LogWarning("TimeEffects list empty in TimeEffectProfile");
            return null;
        }

        foreach (var entry in effectPresets)
        {
            if (entry.preset == presetID)
            {
                return entry.effect;
            }
        }

        Debug.LogWarning($"HitstopProfile of effectID '{presetID}' not found!");
        return null;
    }

}
