using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class LiftGammaGainController : MonoBehaviour
{
    private Volume volume;
    private LiftGammaGain liftGammaGain;
    private MasterMind mm;
    private WhiteBalance whiteBalance;
    private void Start()
    {
        mm = FindAnyObjectByType<MasterMind>();
        volume = GetComponent<Volume>();

        volume.profile.TryGet<WhiteBalance>(out whiteBalance);
        if (volume == null || volume.profile == null)
        {
            Debug.LogError("Missing Volume or Volume Profile!");
            return;
        }

        //trying to find LGG
        if (!volume.profile.TryGet<LiftGammaGain>(out liftGammaGain))
        {
            Debug.LogError("LiftGammaGain effect not found in profile!");
            return;
        }
    }

    public void SetLift(float r, float g, float b)
    {
        if (liftGammaGain != null)
        {
            // Behåll alpha-värdet (w) oförändrat
            Vector4 currentLift = liftGammaGain.lift.value;
            liftGammaGain.lift.Override(new Vector4(r, g, b, currentLift.w));
        }
    }

    public void SetGamma(float r, float g, float b)
    {
        if (liftGammaGain != null)
        {
            Vector4 currentGamma = liftGammaGain.gamma.value;
            liftGammaGain.gamma.Override(new Vector4(r, g, b, currentGamma.w));
        }
    }

    public void SetGain(float r, float g, float b)
    {
        if (liftGammaGain != null)
        {
            Vector4 currentGain = liftGammaGain.gain.value;
            liftGammaGain.gain.Override(new Vector4(r, g, b, currentGain.w));
        }
    }
    public void ChangeTemperature()
    {
        whiteBalance.temperature.Override(14.34f); // Varmare
        whiteBalance.tint.Override(0.92f); // Grönare
    }

    public void SetGameOver()
    {
        liftGammaGain.active = true;
        whiteBalance.active = true;
        SetLift(0.41f, 0.31f, 0.50f);
        SetGamma(0f, -0.10f, -0.06f);
        SetGain(1.95f, 2f, 2f);
        Debug.Log($"Lift: {liftGammaGain.lift.value}, Gamma: {liftGammaGain.gamma.value}, Gain: {liftGammaGain.gain.value}");
    }

    public void SetWinnerScene()
    {
        SetLift(0.89f, 0.79f, 0.98f);
        SetGamma(0.54f, 0.44f, 0.48f);
        SetGain(1.86f, 1.90f, 1.90f);
    }

    public void Setregular()
    {
        SetLift(0.76f,0.66f,0.86f);
        SetGamma(1.52f,1.42f,1.46f);
        SetGain(1.07f, 1.12f, 1.12f);
    }

    public void ResetToDefault()
    {
        SetLift(0f, 0f, 0f);
        SetGamma(1f, 1f, 1f);
        SetGain(1f, 1f, 1f);
    }

    // Gradvis övergång mellan inställningar
    public IEnumerator LerpToValues(Vector4 targetLift, Vector4 targetGamma, Vector4 targetGain, float duration)
    {
        float elapsedTime = 0f;
        Vector4 startLift = liftGammaGain.lift.value;
        Vector4 startGamma = liftGammaGain.gamma.value;
        Vector4 startGain = liftGammaGain.gain.value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            liftGammaGain.lift.Override(Vector4.Lerp(startLift, targetLift, t));
            liftGammaGain.gamma.Override(Vector4.Lerp(startGamma, targetGamma, t));
            liftGammaGain.gain.Override(Vector4.Lerp(startGain, targetGain, t));

            yield return null;
        }
    }

    private void Update()
    {
        if (mm.GameOverSceneOn == true)
        {
            SetGameOver();
            ChangeTemperature();
        }
        if (mm.WinningSceneOn == true)
        {
            SetWinnerScene();
            ChangeTemperature();
        }
    }
}