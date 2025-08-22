using UnityEngine;
using UnityEngine.Rendering;

public class AimSlowdownManager : MonoBehaviour
{
    [Header("Aiming Slowdown Settings")]
    public float aimingTimeScale = 0.3f;
    public float volumeFadeInDuration = 0.25f;
    public float lingerDuration = 0.5f;
    public float slowReturnDuration = 1.5f;
    public Volume slowMoVolume;
    public AnimationCurve returnCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


    private bool isAimingSlowMoActive = false;

    private float lingerElapsed = 0f;
    private bool isReturning = false;
    private float returnElapsed = 0f;

    private float volumeMax = 1f;
    private float volumeMid = 0.5f;
    private bool volumeIsFadingIn = false;
    private float volumeFadeInElapsed = 0f;
    private bool volumeIsFadingOut = false;
    private float volumeFadeOutElapsed = 0f;

    public bool IsAimingSlowMoActive => isAimingSlowMoActive;

    private void Update()
    {
        //if (TimeScaleManagerOLD.Instance.IsPaused || HitStopManager.Instance?.IsHitstopActive == true)
        //    return;

        if (isAimingSlowMoActive)
        {
            HandleVolumeFadeIn();
            HandleSlowMoReturn();
        }
        else if (!isAimingSlowMoActive && volumeIsFadingOut)
        {
            HandleVolumeFadeOut();
        }
        else if (!isReturning && slowMoVolume != null)
        {
            slowMoVolume.weight = 0f;
        }
    }

    //Start the slow down from aiming
    public void EnableAimingSlowMo()
    {
        if (isAimingSlowMoActive || isReturning)
            return;

        isAimingSlowMoActive = true;
        isReturning = false;
        volumeIsFadingIn = true;

        lingerElapsed = 0f;
        returnElapsed = 0f;
        //TimeScaleManagerOLD.Instance.SetReturnTimeScale(aimingTimeScale);

        slowMoVolume.enabled = true;

        //if (!HitStopManager.Instance.IsHitstopActive)
        //{
        //    //Time.timeScale = aimingTimeScale;
        //    //TimeScaleManagerOLD.Instance.SetReturnTimeScale(aimingTimeScale);
        //}
    }

    //Stop and reset values when disabling aiming slowdown
    public void DisableAimingSlowMo()
    {
        isAimingSlowMoActive = false;
        isReturning = false;
        volumeIsFadingIn = false;
        volumeIsFadingOut = false;

        volumeFadeInElapsed = 0f;
        volumeFadeOutElapsed = 0f;
        lingerElapsed = 0f;
        returnElapsed = 0f;

        //TimeScaleManagerOLD.Instance.ResetReturnTimeScale();

        if (slowMoVolume != null)
        {
            slowMoVolume.weight = 0f;
            slowMoVolume.enabled = true;
        }
    }

    //Fade in slow motion post-process volume
    private void HandleVolumeFadeIn()
    {
        if (volumeIsFadingIn)
        {
            volumeFadeInElapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(volumeFadeInElapsed / volumeFadeInDuration);

            if (slowMoVolume != null)
                slowMoVolume.weight = Mathf.Lerp(0f, volumeMax, t);

            if (t >= 1f)
                volumeIsFadingIn = false;
        }
    }

    //Fade out slow motion post-process volume
    private void HandleVolumeFadeOut()
    {
        volumeFadeOutElapsed += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(volumeFadeOutElapsed / volumeFadeInDuration);

        if (slowMoVolume != null)
            slowMoVolume.weight = Mathf.Lerp(volumeMid, 0f, t);

        if (t >= 1f)
            volumeIsFadingOut = false;
    }

    //Fade back to normal timescale after linger duration
    private void HandleSlowMoReturn()
    {
        lingerElapsed += Time.unscaledDeltaTime;

        if (lingerElapsed >= lingerDuration)
        {
            isReturning = true;
            returnElapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(returnElapsed / slowReturnDuration);
            float curveT = returnCurve.Evaluate(t);

            //currentActiveTimeScale = Mathf.Lerp(aimingTimeScale, 1f, curveT);
            //Time.timeScale = currentActiveTimeScale;
            //TimeScaleManagerOLD.Instance.SetReturnTimeScale(Mathf.Lerp(aimingTimeScale, 1f, curveT));

            if (slowMoVolume != null)
                slowMoVolume.weight = Mathf.Lerp(volumeMax, volumeMid, curveT);

            if (t >= 1f && !volumeIsFadingOut)
            {
                isAimingSlowMoActive = false;
                isReturning = false;
                volumeFadeOutElapsed = 0f;
                volumeIsFadingOut = true;
            }
        }
        else
        {
            if (!volumeIsFadingIn && slowMoVolume != null)
                slowMoVolume.weight = volumeMax;
        }
    }
}
