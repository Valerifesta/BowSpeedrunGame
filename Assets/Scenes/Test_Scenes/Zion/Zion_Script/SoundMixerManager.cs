using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider MusicSlider;




    public void Start()
    {
       //audioMixer = GetComponent<AudioMixer>();

       /* if (PlayerPrefs.HasKey("masterVolume"))
        {
            loadVolume();
        }
        else
        {

            SetMasterVolume();
            SetSoundFXVolume();
            SetMusicVolume();
        }*/
    }
    public void SetMasterVolume(float level)
    {
        float volume = MasterSlider.value;
        
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level)/*(volume)*/ * 20f);
        PlayerPrefs.GetFloat("masterVolume", volume);
        loadVolume();
    }


    public void SetSoundFXVolume(float level)
    {
        float volume = SFXSlider.value;
      
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level)/*(volume)*/ * 20f);
        PlayerPrefs.GetFloat("sfxVolume", volume);
        loadVolume();
    }

    public void SetMusicVolume(float level)
    {
       
        float volume = MusicSlider.value;
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level)/*(volume)*/ * 20f);
        PlayerPrefs.GetFloat("musicVolume", volume);
        loadVolume();
    }

    public void loadVolume()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("masterVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("soundFXVolume");
    }

}
