using UnityEngine;

[System.Serializable]
public class TimeEffect
{
    //[Header("Effect Settings")]
    public TimeEffectProperty<float> timeScale;
    public TimeEffectProperty<bool> timedEffect;
    public TimeEffectProperty<bool> transitionOnExpire;
    public TimeEffectProperty<float> duration;           //Depending on "IsTimed" and TransitionOnExpire, duration is either how long it is, or how long the transition is
    public TimeEffectProperty<AnimationCurve> curve;     //Depending on IsTimed, curve is either for the curve of the time effect, or for the transition return curve
    public TimeEffectProperty<bool> blendWithNext;
    public TimeEffectProperty<bool> useCurveDuration;
    public TimeEffectProperty<TimeEffectPriority> priority;
    public TimeEffectProperty<bool> canOverride;
    public TimeEffectProperty<bool> maxActiveInstancesUsePreset;    //If true, uses the maxamount set in the preset shared globally
    public TimeEffectProperty<int> maxActiveInstances;

    //[Header("Effect State")]
    [HideInInspector]
    public float timeElapsed = 0f;
    public bool isExpired => timeElapsed >= GetDuration();

    private UnityEngine.Object parent;

    public void SetParent(UnityEngine.Object newParent)
    {
        this.parent = newParent;
    }

    public float GetDuration()
    {
        if (!timedEffect.instanceValue)
            return 0f;

        if (!useCurveDuration.instanceValue)
        {
            if(duration.instanceValue > 0)
            {
                return duration.instanceValue;
            }
            else
            {
                Debug.LogWarning($"No time effect duration set.", parent);
                return 0;
            }
        }
        else
        {
            if (curve.instanceValue != null && curve.instanceValue.length > 0)
            {
                return curve.instanceValue.keys[curve.instanceValue.length - 1].time;
            }
            else
            {
                Debug.LogWarning($"CurveDuration chosen for TimeEffect but no curve or keys found.", parent);
                return 0;
            }
        }
    }

    public float GetCurrentTimeScale()
    {
        if (!timedEffect.instanceValue)
            return timeScale.instanceValue;

        float duration = GetDuration();
        if (duration <= 0f || curve.instanceValue == null) 
            return timeScale.instanceValue;

        float t = Mathf.Clamp(timeElapsed, 0f, duration);
        return curve.instanceValue.Evaluate(t / duration) * timeScale.instanceValue;
    }

    public void EffectUpdate(float deltaTime)
    {
        timeElapsed += deltaTime;
    }

    public float GetRemainingTime()
    {
        return Mathf.Max(0, GetDuration() - timeElapsed);
    }

    public void ResetTimeElapsed()
    {
        timeElapsed = 0f;
    }
}