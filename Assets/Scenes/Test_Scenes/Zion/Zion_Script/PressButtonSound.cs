using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class PressButtonSound : MonoBehaviour
{
    [Header("Entersound List")]
    [SerializeField] private AudioClip[] enterSounds;

    private AudioSource source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // enterSounds = GetComponents<AudioClip>();
        Debug.Log(gameObject.name);
        source = GetComponent<AudioSource>();
    }

    public void PlayRandomButtonSound()
    {
        AudioClip clip = enterSounds[Random.Range(0, enterSounds.Length)];
        source.PlayOneShot(clip);
        //SoundSFXManager.instance.PlaySoundFXClip(clip, transform, 1f);
    }

    // Update is called once per frame

    /*
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            /*if (EventSystem.current.currentSelectedGameObject != null)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                {
                    PlaySound();
                }
            }
            if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
            {
                PlaySound();
            }
        }
    }*/
}
