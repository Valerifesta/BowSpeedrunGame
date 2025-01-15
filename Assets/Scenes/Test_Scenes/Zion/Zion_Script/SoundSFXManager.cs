using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundSFXManager : MonoBehaviour
{
    public static SoundSFXManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        // assign the audioClips
        audioSource.clip = audioClip;
        // assign volume
        audioSource.volume = volume;
        // play sound
        audioSource.Play();
        // get length or sound FX clip    
        float clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenght);
    }
}
