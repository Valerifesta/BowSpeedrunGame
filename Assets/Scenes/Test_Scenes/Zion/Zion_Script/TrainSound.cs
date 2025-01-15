using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class TrainSound : MonoBehaviour
{
    [Header("Music List")]
    [SerializeField] private AudioClip[] TrainIsBehind;
    [SerializeField] private AudioClip[] TrainIsHere;
    [SerializeField] private AudioClip[] TrainIsInComing;
    [SerializeField] private AudioClip[] TrainIsFront;
    [SerializeField] private AudioClip PlayingThisClip;

    [Header("Train Position States")]
    [SerializeField] private bool isBehind;
    [SerializeField] private bool isHere;
    [SerializeField] private bool inComing;
    [SerializeField] private bool inFront;

    [Header("Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceBetweenObjects;
    [SerializeField] private float TuffBehind = 5f;
    [SerializeField] private float TuffHere = 10f;
    [SerializeField] private float TuffIncoming = 15f;
    [SerializeField] private float TuffInfront = 20f;

    
    private string currentPhase = "";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Kunde inte hitta spelaren! Se till att spelaren har taggen 'Player'");
        }
    }

    private void PlayTrainSound(AudioClip[] soundArray, string phase)
    {
        // Spela bara om vi är i en ny fas
        if (phase != currentPhase && soundArray.Length > 0)
        {
            AudioClip clip = soundArray[Random.Range(0, soundArray.Length)];
            PlayingThisClip = clip;

            if (SoundSFXManager.instance != null)
            {
                SoundSFXManager.instance.PlaySoundFXClip(clip, transform, 1f);
                currentPhase = phase; // Uppdatera nuvarande fas
            }
            else
            {
                Debug.LogWarning("SoundSFXManager saknas!");
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        distanceBetweenObjects = Vector3.Distance(transform.position, player.transform.position);

        // Återställ alla states
        isBehind = isHere = inComing = inFront = false;

       
        if (distanceBetweenObjects <= TuffBehind)
        {
            isBehind = true;
            PlayTrainSound(TrainIsBehind, "behind");
        }
        else if (distanceBetweenObjects <= TuffHere)
        {
            isHere = true;
            PlayTrainSound(TrainIsHere, "here");
        }
        else if (distanceBetweenObjects <= TuffIncoming)
        {
            inComing = true;
            PlayTrainSound(TrainIsInComing, "incoming");
        }
        else if (distanceBetweenObjects > TuffInfront)
        {
            inFront = true;
            PlayTrainSound(TrainIsFront, "front");
        }
    }
}

