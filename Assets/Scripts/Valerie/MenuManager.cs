using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _settingsUI;
    [SerializeField] private GameObject[] _nonSettingsUI;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private Slider[] SenseSliders;
    private Vector3 sliderVector;
    bool settingsActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSettings.CameraSensitivity = sliderVector;

    }

    // Update is called once per frame
    void Update()
    {
        sliderVector = new Vector3(SenseSliders[0].value, SenseSliders[1].value, SenseSliders[2].value);
        if (GameSettings.CameraSensitivity != sliderVector)
        {
            GameSettings.CameraSensitivity = sliderVector;
            Debug.Log(GameSettings.CameraSensitivity);
        }

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
