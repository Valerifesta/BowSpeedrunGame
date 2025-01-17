using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemySoundList : MonoBehaviour
{
    [Header("Enemy Talk List")]
    [SerializeField] private AudioClip[] ESL;
    [Header("Turrent lodaing sound List")]
    [SerializeField] private AudioClip[] turrentLoadingSound;
    [Header("Turrent hit sound List")]
    [SerializeField] private AudioClip[] turrentHitSound;
    //[Header("Turrent hit sound List")]
    //[SerializeField] private AudioClip[] turrentBeamSound;
    [Header("Turrent rotate sound List")]
    [SerializeField] private AudioClip turrentRotateSound;



    [Header("Enemy sounds States")]
    [SerializeField] private bool isTurrentLoading;
    [SerializeField] private bool isTurrentHit;
    [SerializeField] private bool isTurrentRotate;
    [SerializeField] private bool EnemyTalking;



    private NewEnemyBehaviour EnemybehaviourScript;


    private Dictionary<string, float> soundTimers = new Dictionary<string, float>();
    [SerializeField] private float minTimeBetweenSounds = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemybehaviourScript = GetComponent<NewEnemyBehaviour>();
        if (EnemybehaviourScript == null)
        {
            Debug.LogError("NewEnemyBehaviour saknas!");
        }

        // Initiera alla timers
        soundTimers["talk"] = 0f;
        soundTimers["loading"] = 0f;
        soundTimers["hit"] = 0f;
        soundTimers["rotate"] = 0f;

        // Lägg till lyssnare för ljuden
        EnemybehaviourScript.OnStartRotating += PlayRotateSound;
        EnemybehaviourScript.OnStartCharging += PlayLoadingSound;
        EnemybehaviourScript.OnShoot += PlayShootSound;
        EnemybehaviourScript.OnHit += PlayHitSound;
    }

    public void PlayRotateSound()
    {
        if (SoundSFXManager.instance != null && turrentRotateSound != null)
        {
            SoundSFXManager.instance.PlaySoundFXClip(turrentRotateSound, transform, 1f);
        }
    }

    public void PlayLoadingSound()
    {
        PlayEnemySound(turrentLoadingSound, "loading");
    }

    public void PlayHitSound()
    {
        PlayEnemySound(turrentHitSound, "hit");
    }

    public void PlayShootSound()
    {
        // Om du har specifika skjutljud, lägg till dem här
        PlayEnemySound(ESL, "talk"); // Använder ESL tills du har specifika skjutljud
    }

   

    // Städa upp när objektet förstörs
    private void OnDestroy()
    {
        if (EnemybehaviourScript != null)
        {
            EnemybehaviourScript.OnStartRotating -= PlayRotateSound;
            EnemybehaviourScript.OnStartCharging -= PlayLoadingSound;
            EnemybehaviourScript.OnShoot -= PlayShootSound;
            EnemybehaviourScript.OnHit -= PlayHitSound;
        }
    }


    private void PlayEnemySound(AudioClip[] soundArray, string soundType)
    {
        // Uppdatera timer för denna ljudtyp
        if (soundTimers[soundType] <= 0 && soundArray.Length > 0)
        {
            AudioClip clip = soundArray[Random.Range(0, soundArray.Length)];
            if (SoundSFXManager.instance != null)
            {
                SoundSFXManager.instance.PlaySoundFXClip(clip, transform, 1f);
                soundTimers[soundType] = minTimeBetweenSounds; 
            }
            else
            {
                Debug.LogWarning("SoundSFXManager saknas!");
            }
        }
    }

    private void Update()
    {
        UpdateSoundTimers();
    }

    private void UpdateSoundTimers()
    {
        string[] timerKeys = { "talk", "loading", "hit", "rotate" };
        foreach (var key in timerKeys)
        {
            if (soundTimers.ContainsKey(key))
            {
                soundTimers[key] = Mathf.Max(0, soundTimers[key] - Time.deltaTime);
            }
        }
    }

}
