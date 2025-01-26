using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ScreenPP : MonoBehaviour
{
    private MasterMind mm;
    private TeleportManager TM;

    private Volume volume;
    private LiftGammaGain LGG;//gameover and winn effect
    private ChromaticAberration CA;// teleport effect
    private Vignette vignette;//gameover and winn effect
    private WhiteBalance WB;//gameover and winn effect








    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);

        volume.profile.TryGet<LiftGammaGain>(out LGG);

        volume.profile.TryGet<ChromaticAberration>(out CA);

        volume.profile.TryGet<WhiteBalance>(out WB);


        mm = FindObjectOfType<MasterMind>();
        if (mm == null)
        {
            Debug.LogError("Could not find MasterMind component!");
            return;
        }
        if (!TryGetComponent(out volume))
        {
            Debug.LogError("No Volume component found!");
            return;
        }


    }



    private void GameOverFilter()
    {
        vignette.active = true;
        vignette.intensity.value = 0.36f;
        vignette.smoothness.value = 0.56f;
        vignette.rounded.value = false;
        vignette.center.Override(new Vector2(0.5f, 0.5f));


        WB.active = true;
        WB.tint.value = 0.92f;
        WB.temperature.value = 14.34f;

        LGG.active = true;
        LGG.gamma.Override(new Vector4(0f, -0.10f, -0.06f));
        LGG.gain.Override(new Vector4(1.95f, 2f, 2f));
        LGG.lift.Override(new Vector4(0.41f, 0.31f, 0.50f));
    }
    private void WinnerFilter()
    {
        vignette.active = true;
        vignette.intensity.value = 0.36f;
        vignette.smoothness.value = 0.56f;
        vignette.rounded.value = false;
        vignette.center.Override(new Vector2(0.5f, 0.5f));


        WB.active = true;
        WB.tint.value = 0.92f;
        WB.temperature.value = 14.34f;

        LGG.active = true;
        LGG.gamma.Override(new Vector4(0.54f, 0.44f, 0.48f));
        LGG.gain.Override(new Vector4(1.86f, 1.90f, 1.90f));
        LGG.lift.Override(new Vector4(0.89f, 0.79f, 0.98f));
    }


    private IEnumerator TransitionFilter(float duration)
    {
        float elapsedTime = 0;
        float startIntensity = vignette.intensity.value;
        float targetIntensity = 0.36f; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }
    }



    private void TeleportFilter()
    {
        CA.intensity.value = 0f;
    }

    private void RegularFilter()
    {
        vignette.active = false;
        vignette.intensity.value = 1;
        LGG.gamma.Override(new Vector4(1.54f, 1.42f, 1.46f));
        LGG.gain.Override(new Vector4(1.07f, 1.12f, 1.12f));
        LGG.lift.Override(new Vector4(0.76f, 0.66f, 0.86f));
    }

  


    private void CheckFilter()
    {
        if(mm.GameOverSceneOn == true)
        {
            GameOverFilter();
        }
        if(mm.WinningSceneOn== true)
        {
            WinnerFilter();
        }
        if(mm.PlayModeSceneOn == true)
        {
            RegularFilter();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckFilter();
    }
}
