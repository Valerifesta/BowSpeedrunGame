using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessManager : MonoBehaviour
{
    private MasterMind mm;
    private ArrowBehaviour AB;
    private TeleportManager TM;
    private TestBowBehaviour TBB;

    private Volume volume;
    private LiftGammaGain LGG;//gameover and winn effect
    private ChromaticAberration CA;// teleport effect
    private Vignette vignette;//gameover and winn effect
    private WhiteBalance WB;//gameover and winn effect

    private LensDistortion LD;
    private ShadowsMidtonesHighlights SMH;

    //[SerializeField] private AnimationCurve IntensitySmoothStartAnimation;
    //[SerializeField] private AnimationCurve IntensitySmoothLastTimeAnimation;
    //private float IntensitySmoothStart;
    //private float IntensitySmoothLastTime;


    // varibles for LensDistortion

    // varibles for ShadowsMidtonesHighlights






    // varibles for Vignette-Z
    public Color color = new Color(0.00f, 0.0f, 0.00f);
    public Vector2 center = new Vector2(0.50f, 0.50f);
    //varibles for fades into to darkmode
    public float darknessSpeed = 0.5f;
    private float currentDarkness = 0f;


    // Variabler för ChromaticAberration
    //public float chromaticAberrationIntensity = 0f;
    public int duration = 2;
    public int timeRemaining;
    public bool isCountingDown = false;


    // Variabler för WhiteBalance
    //public float whiteTint = 0.92f;
    //public float whiteTemperature = 14.34f;

    // Variabler för LiftGammaGain
    //public Vector4 lggGamma = new Vector4(1.54f, 1.42f, 1.46f,0);
    //public Vector4 lggGain = new Vector4(1.07f, 1.12f, 1.12f,0);
    //public Vector4 lggLift = new Vector4(0.76f, 0.66f, 0.86f,0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mm = FindObjectOfType<MasterMind>();
        AB = FindAnyObjectByType<ArrowBehaviour>();
        TBB = FindAnyObjectByType<TestBowBehaviour>();
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);

        volume.profile.TryGet<LiftGammaGain>(out LGG);

        volume.profile.TryGet<ChromaticAberration>(out CA);

        volume.profile.TryGet<WhiteBalance>(out WB);

        volume.profile.TryGet<LensDistortion>(out LD);
        volume.profile.TryGet<ShadowsMidtonesHighlights>(out SMH);


        RegularFilter();

    }

    

    

   


   

    private void GameOverFilter()
    {
        vignette.active = true;
        vignette.intensity.value = 0.325f;
        vignette.smoothness.value = 0.56f;
        vignette.rounded.value = false;
        vignette.center.Override(new Vector2(0.5f, 0.5f));


        WB.active = true;
        WB.tint.value = 0.92f;
        WB.temperature.value = 14.34f;

        LGG.active = true;
        LGG.gamma.Override(new Vector4(0.26f, 0.16f, 0.20f,-0.74f));
        LGG.gain.Override(new Vector4(1.27f, 1.19f, 1.28f, 0.2828005f));
        LGG.lift.Override(new Vector4(0.76f, -0.11f, 0.06f, -0.2422627f));
        vignette.color.Override(new Color(0f, 0f, 0.98f));
    }
    private void WinnerFilter()
    {
        vignette.active = true;
        vignette.intensity.value = 0.36f;
        vignette.smoothness.value = 0.56f;
        vignette.rounded.value = false;
        vignette.center.Override(new Vector2(0.5f, 0.5f));
        vignette.color.Override (new Color(1.0f, 0.9040225f, 0.07861614f));


        WB.active = true;
        WB.tint.value = 0.92f;
        WB.temperature.value = 14.34f;

        LGG.active = true;
        LGG.gamma.Override(new Vector4(0.54f, 0.44f, 0.48f,1f));
        LGG.gain.Override(new Vector4(1.86f, 1.90f, 1.90f,0.9f));
        LGG.lift.Override(new Vector4(0.89f, 0.79f, 0.98f,-0.2f));
    }

    private void TeleportFilter()
    {
       
        CA.intensity.value = 1f;




        if (!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
            Invoke("_tick", 1f);
        }

    }

    private void ShieldFas()
    {

        CA.intensity.value = 1f;
        LGG.gamma.Override(new Vector4(1.30f, 1.44f, 1.25f, 0.347986f));
        LGG.gain.Override(new Vector4(1.15f, 1.20f, 1.20f, 0.1857141f));
        LGG.lift.Override(new Vector4(0.83f, 0.71f, 0.94f, -0.14650202f));

        vignette.active = true;
        vignette.intensity.value = 0.145f;
        vignette.smoothness.value = 1f;
        vignette.rounded.value = true;
        vignette.center.Override(new Vector2(0.5f, 0.5f));
        vignette.color.Override(new Color(0.1482f, 0.9245f, 0.8956f));


        SMH.shadows.Override(new Vector4(2.52f, 2.52f, 2.52f, 0.32234444f));
        SMH.midtones.Override(new Vector4(4.56f, 4.56f, 4.56f, 0.8351648f));
        SMH.highlights.Override(new Vector4(1f, 1f, 1f, 1.724992E-09f));

        LGG.active = true;
        LGG.gamma.Override(new Vector4(0.54f, 0.44f, 0.48f, 1f));
        LGG.gain.Override(new Vector4(1.86f, 1.90f, 1.90f, 0.9f));
        LGG.lift.Override(new Vector4(0.89f, 0.79f, 0.98f, -0.2f));

        WB.active = true;
        WB.tint.value = -0.38f;
        WB.temperature.value = -100f;

        LD.active = true;
        LD.center.Override(new Vector2(0.5f, 0.5f));
        LD.intensity.value = 0.106f;
        LD.xMultiplier.value = 1f;
        LD.yMultiplier.value = 1f;
        LD.scale.value = 1.1f;


       

    }
    private void blueFace()
    {

        CA.intensity.value = 1f;

        if (!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
            Invoke("_tick", 1f);
        }

    }
    private void _tick()
    {
        timeRemaining--;
        if (timeRemaining > 0)
        {
            Invoke("_tick", 0.5f);
        }
        else
        {
            isCountingDown = false;
            CA.intensity.value = 0f;
           


        }
    }

    private void RegularFilter()
    {
        vignette.active = true;
        vignette.intensity.value = 0.319f;//0.119f;//0.319f;
        vignette.smoothness.value = 1f;
        LGG.gamma.Override(new Vector4(1.54f, 1.42f, 1.46f, -0.09793615f));
        LGG.gain.Override(new Vector4(1.07f, 1.12f, 1.12f, 0.3322843f));
        LGG.lift.Override(new Vector4(0.76f, 0.66f, 0.86f, -0.05596373f));
        vignette.color.Override(new Color(0f, 0f, 0f));
    }


    private void CheckFilter()
    {
        if (mm.GameOverSceneOn == true)
        {
            GameOverFilter();
        }
        if (mm.WinningSceneOn == true)
        {
            WinnerFilter();
        }
        if (mm.PlayModeSceneOn == true)
        {
            RegularFilter();

        }
    }



    // Update is called once per frame
    public void Update()
    {
        CheckFilter();
        if (TBB.tempInputs == false)
        {
            Invoke("TeleportFilter", 0);
        }
       
/*

        Debug.Log($"Lift W-värde: {LGG.lift.value.w}");
        Debug.Log($"Gain W-värde: {LGG.gain.value.w}");
        Debug.Log($"Gamma W-värde: {LGG.gamma.value.w}");

        Debug.Log($"Shadow W-värde: {SMH.shadows.value.w}");
        Debug.Log($"midtones W-värde: {SMH.midtones.value.w}");
        Debug.Log($"highlights W-värde: {SMH.highlights.value.w}");

        */
    }
}
