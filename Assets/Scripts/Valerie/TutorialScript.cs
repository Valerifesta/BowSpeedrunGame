using System.Collections;
using System.Drawing;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TutorialScript : MonoBehaviour
{
    [SerializeField] private Camera Main;
    [SerializeField] private RawImage screenFadeImage;
    private float elapsedFadeTime;
    private bool timerStarted;
    private bool bowEnabled;

    [SerializeField] private GameObject BowObj;
    [SerializeField] private GameObject BowPullbackSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Fade(new Color32(0, 0, 0, 255), new Color32(0, 0, 0, 0), 5, false, 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            elapsedFadeTime += 1.0f * Time.deltaTime;
        }
    }
    void ToggleBow()
    {
        bowEnabled = !bowEnabled;

        BowObj.SetActive(bowEnabled);
        BowPullbackSlider.SetActive(bowEnabled);
        
    }

    IEnumerator Fade(Color32 colorA, Color32 colorB, float time,  bool enviorment, float delay = 0)
    {
        if (delay != 0)
        {
            Debug.Log("Delayed " + delay + " seconds before starting fade.");
            if (enviorment)
            {
                Main.backgroundColor = colorA;
            }
            else
            {
                screenFadeImage.color = colorA;
            }
            yield return new WaitForSeconds(delay);
        }
        UnityEngine.Debug.Log("Started Fade");
        while (true)
        {
            elapsedFadeTime += 1.0f * Time.deltaTime;
            float t = Mathf.InverseLerp(0, time, elapsedFadeTime);
            if (t >= 1)
            {
                elapsedFadeTime = 0;
                UnityEngine.Debug.Log("Ended Fade");
                break;
            }
            else
            {
                Color32 newColor = Color32.Lerp(colorA, colorB, t);
                if (enviorment)
                {
                    Main.backgroundColor = newColor;
                }
                else
                {
                    screenFadeImage.color = newColor;
                }

            }

            yield return null;
        }
        
    }
    
}
