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
    bool settingsActive;
    public GameObject TV;
    public GameObject VideoScreen;
    public GameObject wholeCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSettings.CameraSensitivity = sliderVector;

    }

    // Update is called once per frame
    void Update()
    {
        sliderVector = new Vector3(SenseSliders[0].value, SenseSliders[1].value, SenseSliders[2].value);
        if (settingsActive)
        {
            if (GameSettings.CameraSensitivity != sliderVector)
            {
                GameSettings.CameraSensitivity = sliderVector;
               
                Debug.Log(GameSettings.CameraSensitivity);
            }
            
            _thresholdScrollValue = Threshold.value * 9;
            if (GameSettings.DegSecReleaseThreshold != _thresholdScrollValue)
            {
                GameSettings.DegSecReleaseThreshold = _thresholdScrollValue;
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
        settingsActive = !settingsActive;
        if (settingsActive)
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
