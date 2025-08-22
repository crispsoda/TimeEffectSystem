using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TimeScaleManagerOLD : MonoBehaviour
{
    //[SerializeField] 
    //private TimeEffectProfileSO timeEffectProfile;

    //public float ActiveTimeScale => currentActiveTimeScale;
    //private float currentActiveTimeScale = 1f;
    //public bool IsPaused => isPaused;
    //private bool isPaused = false;

    //[SerializeField]
    //private float minTimeScale = 0.01f;
    //[SerializeField]
    //private float maxTimeScale = 2f;

    //private TimeEffect currentTimeEffect;
    //private float currentTimeEffectRemainder;
    //private TimeEffectPriority currentEffectPriority;
    //private List<TimeEffect> requestedEffects = new List<TimeEffect>();

    //public Dictionary<TimeEffectPresetID, int> maxChainAmounts = new Dictionary<TimeEffectPresetID, int>();

    //private int chainIndex;

    //private bool isChainTransitioning;

    //public bool showDebugLog = false;



    ////Hitstop values
    //private int currentHitstopPriority;

    //public bool IsHitstopActive => isHitstopActive;
    //private bool isHitstopActive;

    //public static event Action OnHitstopStarted;
    //public static event Action OnHitstopEnded;

    //public static event Action OnPaused;
    //public static event Action OnUnpaused;



    ////NEW
    //private Queue<TimeEffect> queuedEffects = new Queue<TimeEffect>();
    //private Stack<TimeEffect> overriddenEffects = new Stack<TimeEffect>();

    ////Singleton
    //public static TimeScaleManagerOLD Instance { get; private set; }

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

    //private void Update()
    //{
    //    if (requestedEffects.Count == 0)
    //        return;

    //    //If its just one, just apply that 

    //    //IF its more than one, apply the highest

    //    //Make sure to reset elapsed duration just before applying one!

    //    //Also counts down during game paused, which is not a bad thing
    //    //requestedEffects.RemoveAll(effect => effect.DurationPassed(Time.unscaledDeltaTime));
    //}

    //public void RequestTimeEffect(TimeEffect request)
    //{
    //    if (request.queuePriority < currentEffectPriority)
    //    {
    //        DebugMessage($"TimeEffect {request.effectID} ignored due to lower priority");
    //        return;
    //    }

    //    if(request.queuePriority == currentEffectPriority)
    //    {
    //        //Check if can chain
    //        //if yes, interrupt current and replace with new of same
    //    }

    //    requestedEffects.Add(request);
    //}

    //private void DebugMessage(string message)
    //{
    //    if(showDebugLog)
    //        Debug.LogWarning(message);
    //}

    //private void QueueEffect()
    //{

    //}

    //private void InterruptEffect()
    //{

    //}

    //private TimeEffect PickHighestEffect()
    //{
    //    return requestedEffects.OrderByDescending(e => e.queuePriority).ThenByDescending(e => e.GetRemainingTime()).First();

    //    //Only necessary if 2 or 3 or more requests?
    //    //If one request, obvs just queue that, if 2 requests, just get higher priority

    //    TimeEffect highestPriority = new TimeEffect();
    //    float longestDuration = 0f;

    //    foreach (var effect in requestedEffects)
    //    {
    //        //If higher priority, make it the highest
    //        if (effect.queuePriority > highestPriority.queuePriority)
    //        {
    //            highestPriority = effect;
    //        }
    //        //If same priority, check which is longer
    //        //Although shouldn't I check for remaining time?
    //        else if(effect.queuePriority == highestPriority.queuePriority)
    //        {
    //            if(effect.GetDuration() >= longestDuration)
    //            {
    //                highestPriority = effect;
    //            }
    //        }
    //    }

    //    return highestPriority;
    //}

    //private float GetChainedDurationTime()
    //{
    //    return 0f;
    //}

    //private bool CanChain(TimeEffect effect)
    //{
    //    if(effect.effectID == currentTimeEffect.effectID)
    //    {
    //        if (chainIndex >= effect.maxEffectInstances)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //    //Can chain if not the same ID
    //    else
    //    {
    //        return true;
    //    }
    //}

    ////When changing timescale, have split second of normal time before activating new time
    ////If chaining, that split second becomes shorter and shorter

    ////To avoid infinite spam, make sure only set amount of consecutive slowdowns can occur before forcing normal time
    ////How to set amount per slowdown priority? Or per slowdown type?
    ////Maybe if slowdown has been active for a certain time and effect is set to certain effect, a multiplier of that effect's duration or just a float number after which to break?

    //private void ClearLowerEffects(List<TimeEffect> requestedTimeEffects)
    //{
    //    foreach(var effect in requestedTimeEffects)
    //    {

    //    }
    //}

    //private void SetCurrentTimeEffect(TimeEffect newEffect)
    //{
    //    currentTimeEffect = newEffect;
    //}

    //private void ClearRequestedEffects()
    //{
    //    requestedEffects.Clear();
    //}

    //private bool IsValidTimeScale(float timeScale)
    //{
    //    if (timeScale >= minTimeScale &&
    //        timeScale <= maxTimeScale &&
    //        !float.IsNaN(timeScale) &&
    //        !float.IsInfinity(timeScale))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //private IEnumerator HandleEffectDuration(TimeEffect effect)
    //{
    //    yield return new WaitForSecondsRealtime(effect.GetDuration());
    //    requestedEffects.Remove(effect);
    //}



    ////Retrieve values from profile and apply hitstop based on its intensity
    //public void ApplyTimeEffect(TimeEffectPresetID intensity)
    //{
    //    var profile = timeEffectProfile.GetProfile(intensity);

    //    if (profile == null)
    //    {
    //        Debug.LogWarning($"Hitstop Profile of effectID '{intensity}' not found!");
    //        return;
    //    }

    //    //If the just-requested hitstop is not higher priority than the currently active one, don't apply it
    //    if (IsHitstopActive && profile.queuePriority.GetHashCode() <= currentHitstopPriority)
    //        return;

    //    var curve = profile.curve;
    //    var duration = profile.duration;

    //    //Check if there is an animation curve for the requested hitstop intensity
    //    if (curve == null)
    //    {
    //        Debug.LogWarning($"No AnimationCurve found for effectID '{intensity}'");
    //        return;
    //    }

    //    //If true, use the duration of the animation curve, rather than the duration value of profile
    //    if (profile.useCurveDuration)
    //    {
    //        if (curve != null && curve.length > 0)
    //        {
    //            duration = curve.keys[curve.length - 1].time;
    //        }
    //    }

    //    //If a hitstop is currently active, stop its coroutine to make way for the new one
    //    if (isHitstopActive)
    //        StopAllCoroutines();

    //    //Debug.Log($"Hitstop of intensity '{intensity}' applied for duration of '{duration}'");

    //    currentHitstopPriority = profile.queuePriority.GetHashCode();
    //    OnHitstopStarted?.Invoke();
    //    StartCoroutine(EffectCoroutine(duration, curve));
    //}

    //private IEnumerator EffectCoroutine(float duration, AnimationCurve curve)
    //{
    //    float elapsedTime = 0f;
    //    bool wasPaused = IsPaused;

    //    isHitstopActive = true;

    //    while (elapsedTime < duration)
    //    {
    //        while (IsPaused)
    //        {
    //            yield return null;
    //        }

    //        float curveValue = curve.Evaluate(elapsedTime / duration);
    //        float timeScaleCurve = curveValue * ActiveTimeScale;

    //        //Only change timescale if the hitstop was not requested from a paused game, as it could override the frozen timescale 
    //        if (!wasPaused)
    //        {
    //            SetRealTimeScale(timeScaleCurve);
    //        }

    //        elapsedTime += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    if (!wasPaused)
    //    {
    //        SetRealTimeScale(ActiveTimeScale);
    //    }

    //    isHitstopActive = false;
    //    currentHitstopPriority = 0;
    //    OnHitstopEnded?.Invoke();
    //}

    //#region old
    ////Set the time scale the game reverts to when finishing a hitstop or coming back from paused state
    //public void SetReturnTimeScale(float value)
    //{
    //    currentActiveTimeScale = Mathf.Clamp(value, 0.01f, 1f);

    //    //Hitstops take precedence
    //    if (!HitStopManager.Instance.IsHitstopActive)
    //        Time.timeScale = ActiveTimeScale;
    //}

    ////Reset actual time scale to normal time
    //public void ResetReturnTimeScale()
    //{
    //    SetReturnTimeScale(1f);
    //}

    ////Set the real time scale to custom value
    //public void SetRealTimeScale(float value)
    //{
    //    Time.timeScale = value;
    //    //Debug.Log($"Timescale force set to '{value}'!");
    //}

    ////Freeze time and any tweens when paused
    //public void PauseTime()
    //{
    //    //DOTween.PauseAll();
    //    Time.timeScale = 0f;
    //    isPaused = true;
    //}

    ////Unfreeze any tweens and time back to previous active time scale when unpaused
    //public void UnpauseTime()
    //{
    //    //DOTween.PlayAll();
    //    Time.timeScale = currentActiveTimeScale;
    //    isPaused = false;
    //}
    //#endregion


}
