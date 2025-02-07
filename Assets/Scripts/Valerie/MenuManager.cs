using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Threading;
using System.Collections;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _settingsUI;
    [SerializeField] private GameObject[] _nonSettingsUI;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private Slider[] SenseSliders;
    [SerializeField] private Scrollbar Threshold;
    private Vector3 sliderVector;
    private float _thresholdScrollValue;
    public bool SettingsActive;
    public GameObject TV;
    public GameObject VideoScreen;
    public GameObject wholeCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 currentSense = GameSettings.CameraSensitivity;
        if (currentSense == Vector3.zero)
        {
            GameSettings.CameraSensitivity = Vector3.one;
            currentSense = GameSettings.CameraSensitivity;
             
        }

        if (SenseSliders.Length > 0)
        {
            foreach (Slider slider in SenseSliders)
            {
                slider.minValue = 1;
                slider.maxValue = 10;
            }

            SenseSliders[0].value = currentSense.x;
            SenseSliders[1].value = currentSense.y;
            SenseSliders[2].value = currentSense.z;

            
        }
        if (GameSettings.DegSecReleaseThreshold == 0)
        {
            GameSettings.DegSecReleaseThreshold = 1;
        }
        _thresholdScrollValue = GameSettings.DegSecReleaseThreshold;
        if (Threshold)
        {
            Threshold.value = _thresholdScrollValue / 10;
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        if (SenseSliders.Length > 0)
        {
            sliderVector = new Vector3(SenseSliders[0].value, SenseSliders[1].value, SenseSliders[2].value);
        }
        if (SettingsActive)
        {
            if (GameSettings.CameraSensitivity != sliderVector)
            {
                GameSettings.CameraSensitivity = sliderVector;
               
                Debug.Log(GameSettings.CameraSensitivity);
            }
            
            if (Threshold != null)
            {
                _thresholdScrollValue = Threshold.value * 10.0f;
                if (GameSettings.DegSecReleaseThreshold != _thresholdScrollValue)
                {
                    GameSettings.DegSecReleaseThreshold = _thresholdScrollValue;
                }
                if (_thresholdScrollValue == 0)
                {
                    Threshold.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.red;
                }
                else
                {
                    Threshold.transform.GetChild(0).GetComponentInChildren<Image>().color = Color.white;

                }
            }
            
            
        }

    }
    public void StartMoveTowardsTV()
    {
        wholeCanvas.SetActive(false);
        VideoScreen.SetActive(false);
        
        StartCoroutine(MoveTowardsTV());
    }
    public IEnumerator MoveTowardsTV()
    {
        Camera main = Camera.main;
        Vector3 startPos = main.transform.position;
        float t = 0;
        while (t < 1)
        {
            t += 1.0f * Time.deltaTime;
            main.transform.position = Vector3.Lerp(startPos, TV.transform.position, t);
            yield return null;
        }
        SceneManager.LoadScene("IntroTutorial");
        yield return null;

    }
    public void ToggleSetting()
    {
        SettingsActive = !SettingsActive;
        if (SettingsActive)
        {
            foreach (GameObject nonSetting in _nonSettingsUI)
            {
                nonSetting.SetActive(false);
            }
            foreach (GameObject settingUI in _settingsUI)
            {
                settingUI.SetActive(true);

            }
        }
        else
        {
            foreach (GameObject nonSetting in _nonSettingsUI)
            {
                nonSetting.SetActive(true);
            }
            foreach (GameObject settingUI in _settingsUI)
            {
                settingUI.SetActive(false);

            }
        }
    }
}
