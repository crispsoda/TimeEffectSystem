using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeEffectTracker : MonoBehaviour
{

    //[SerializeField] private EffectLayerOrder timeEffectLayerOrder = new EffectLayerOrder();

    //Lower index = higher priority
    [SerializeField]
    private TimeEffectLayer[] layerOrder;

    [SerializeField]
    private TimeEffectPresetSO effectProfile;

    private Dictionary<TimeEffectLayer, List<TimeEffectProfile>> activeEffects = new Dictionary<TimeEffectLayer, List<TimeEffectProfile>>();

    [SerializeField]
    private TimeEffectProfile currentDominantEffect;

    [SerializeField]
    private TimeEffectProfile nextDominantEffect;
    [SerializeField] private float nextScale;

    [SerializeField] private float currentTimeElapsed;
    [SerializeField] private float currentTimeLeft;
    [SerializeField] private AnimationCurve currentCurve;

    [SerializeField] private float nextTimeElapsed;
    [SerializeField] private float nextTimeLeft;
    [SerializeField] private AnimationCurve nextCurve;

    public event Action OnEffectAdded;
    public event Action OnEffectExpired;


    //Singleton
    public static TimeEffectTracker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnValidate()
    {
        ValidateLayerOrder();
    }

    private void OnEnable()
    {
        OnEffectExpired += HandleListChange;
        OnEffectAdded += HandleListChange;
    }

    private void OnDisable()
    {
        OnEffectExpired -= HandleListChange;
        OnEffectAdded -= HandleListChange;
    }

    private void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;

        UpdateEffects(deltaTime);
        UpdateTimeScale();
    }

    //Make sure every enum value is in the array and that every item is distinct
    private void ValidateLayerOrder()
    {
        if (layerOrder == null)
            layerOrder = new TimeEffectLayer[0];

        //Remove duplicates
        layerOrder = layerOrder.Distinct().ToArray();

        var enumValues = Enum.GetValues(typeof(TimeEffectLayer));


        //Create array if layerorder is null or not the same length as the enum 
        if (layerOrder.Length != enumValues.Length)
        {
            layerOrder = new TimeEffectLayer[enumValues.Length];
            Array.Copy(enumValues, layerOrder, enumValues.Length);
        }
    }

    public void AddTimeEffect(TimeEffectProfile effect)
    {
        //var profile = effectProfile.GetProfile(effect.effectID);
        TimeEffectLayer layer = effect.layer;

        //Check if list for this layer exists
        if (!activeEffects.TryGetValue(layer, out var effects))
        {
            effects = new List<TimeEffectProfile>();
            activeEffects[layer] = effects;
        }

        //Check if within chain limit for list
        if (effect.effect.maxActiveInstancesUsePreset.instanceValue)
        {
            //TimeEffectTracker.Instance.CheckPresetCanAdd();
        }
        else
        {
            if (!CanAddEffect(effect, activeEffects[layer]))
            {
                return;
            }
        }

        effect.effect.ResetTimeElapsed();
        effects.Add(effect);
        OnEffectAdded?.Invoke();
    }


    private TimeEffectProfile GetDominantEffect()
    {
        //Check dominant layer
        foreach (var layer in layerOrder)
        {
            if (!activeEffects.TryGetValue(layer, out var effects) || effects.Count == 0)
            {
                continue;
            }

            return PickHighestEffectInLayer(effects);
        }

        return null;
    }

    private TimeEffectProfile GetNextDominantEffect()
    {
        bool foundCurrentLayer = false;

        foreach(var layer in layerOrder)
        {
            if(!activeEffects.TryGetValue(layer, out var effects) || effects.Count == 0)
            {
                var orderedList = effects.OrderByDescending(e => e.effect.priority.instanceValue).ThenByDescending(e => e.effect.GetRemainingTime()).ToList();

                if (!foundCurrentLayer)
                {
                    //Highest will be in the same layer as the dominant effect
                    foundCurrentLayer = true;

                    //If that layer has more than one effect, the next dominant effect will be on here
                    if(orderedList.Count > 1)
                    {
                        return orderedList[1];
                    }
                }
                //Otherwise the next dominant effect is on the next layer
                else
                {
                    return orderedList[0];
                }
            }
        }

        return null;
    }

    private TimeEffectProfile PickHighestEffectInLayer(List<TimeEffectProfile> effects)
    {
        if (effects == null || effects.Count == 0)
        {
            return null;
        }

        if (effects.Count == 1)
        {
            return effects[0];
        }

        return effects.OrderByDescending(e => e.effect.priority.instanceValue).ThenByDescending(e => e.effect.GetRemainingTime()).First();
    }

    //If per-instance and not global
    public bool CanAddEffect(TimeEffectProfile effect, List<TimeEffectProfile> activeEffects)
    {
        if (effect == null || activeEffects == null) 
            return false;

        int sameTypeAmount = 0;

        if (effect.effect.maxActiveInstancesUsePreset.instanceValue)
        {
            //Check using preset ID
            sameTypeAmount = activeEffects.Count(e => e.preset == effect.preset);
        }
        else
        {
            //check using string name
            sameTypeAmount = activeEffects.Count(e => e.effectID == effect.effectID);
        }

        int maxAllowed = effect.effect.maxActiveInstancesUsePreset.instanceValue
        ? effectProfile.GetMaxInstanceAmountFromPreset(effect.preset)
        : effect.effect.maxActiveInstances.instanceValue;

        if (sameTypeAmount < maxAllowed)
        {
            return true;
        }

        Debug.LogWarning($"Cannot add effect {effect.effectID} because Max Chain Amount reached for this list");
        return false;
    }

    private void UpdateEffects(float deltaTime)
    {
        currentDominantEffect.effect.EffectUpdate(deltaTime);
        currentTimeElapsed = currentDominantEffect.effect.timeElapsed;
        currentTimeLeft = currentDominantEffect.effect.GetRemainingTime();

        nextDominantEffect.effect.EffectUpdate(deltaTime);
        nextTimeElapsed = nextDominantEffect.effect.timeElapsed;
        nextTimeLeft = nextDominantEffect.effect.GetRemainingTime();

        foreach (var layer in layerOrder)
        {
            //Skip layers without active effects
            if (!activeEffects.TryGetValue(layer, out var effects) || effects.Count == 0)
            {
                continue;
            }

            for (int i = effects.Count - 1; i >= 0; i--)
            {
                var effect = effects[i];

                if (!effect.effect.timedEffect.instanceValue)
                {
                    continue;
                }

                effect.effect.EffectUpdate(deltaTime);

                if (effect.effect.isExpired)
                {
                    effects.RemoveAt(i);
                    effect.effect.ResetTimeElapsed();
                    OnEffectExpired?.Invoke();
                }
            }
        }
    }

    private void HandleListChange()
    {
        //Do the dominant timescale calculations here
        //if transition, lerp, else apply instant

        //Time.timeScale = GetDominantTimeScale();
        currentDominantEffect = GetDominantEffect();
        //currentCurve = currentDominantEffect.effect.curve.instanceValue;

        if(currentDominantEffect != null)
        {
            if(currentDominantEffect.effect.blendWithNext.instanceValue || currentDominantEffect.effect.transitionOnExpire.instanceValue)
            {
                nextDominantEffect = GetNextDominantEffect();
            }
        }

        nextCurve = nextDominantEffect.effect.curve.instanceValue;

        HandleCurveBlend();
    }

    private void UpdateTimeScale()
    {
        if (currentDominantEffect == null)
        {
            Time.timeScale = 1f;
            return;
        }

        var effect = currentDominantEffect;

        if (effect != null)
        {
            if (!effect.effect.timedEffect.instanceValue)
            {
                Time.timeScale = effect.effect.timeScale.instanceValue;
            }
            else
            {
                Time.timeScale = EvaluateCurrentEffectCurve();
            }
        }
    }

    private void HandleCurveBlend()
    {
        if (nextTimeLeft < currentTimeLeft)
        {
            return;
        }

        float nextValue = 0f;
        var blend = currentDominantEffect.effect.blendWithNext.instanceValue;
        var effectCurve = currentDominantEffect.effect.curve.instanceValue;
        if (blend && nextDominantEffect != null)
        {

            AnimationCurve blendedCurve = new AnimationCurve(effectCurve.keys);
            Keyframe lastKey = blendedCurve[blendedCurve.length - 1];

            float predictedBlendTime = nextTimeElapsed + currentTimeLeft;

            if (nextDominantEffect.effect.timedEffect.instanceValue)
            {
                nextValue = EvaluateNextEffectCurve(predictedBlendTime);
            }
            else
            {
                nextValue = nextDominantEffect.effect.timeScale.instanceValue;
            }

            lastKey.value = nextValue;
            blendedCurve.MoveKey(blendedCurve.length - 1, lastKey);


            currentCurve = blendedCurve;
        }
        else
        {
            currentCurve = effectCurve;
        }
    }

    private float EvaluateCurrentEffectCurve()
    {
        var duration = currentDominantEffect.effect.GetDuration();
        if (duration <= 0f || currentCurve == null)
            return currentDominantEffect.effect.timeScale.instanceValue;

        float t = Mathf.Clamp(currentTimeElapsed, 0f, duration);
        return currentCurve.Evaluate(t / duration);
    }

    private float EvaluateNextEffectCurve(float predictedBlend)
    {
        var duration = nextDominantEffect.effect.GetDuration();
        if (duration <= 0f || nextCurve == null)
            return nextDominantEffect.effect.timeScale.instanceValue;

        float t = Mathf.Clamp(predictedBlend, 0f, duration);
        return nextDominantEffect.effect.curve.instanceValue.Evaluate(t / duration);
    }
}
