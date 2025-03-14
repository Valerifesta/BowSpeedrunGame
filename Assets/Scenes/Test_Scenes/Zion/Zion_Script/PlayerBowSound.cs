using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBowSound : MonoBehaviour
{
    public PlayerManager PM;
    public TeleportManager TM;
    private NewEnemyBehaviour NEB;
    [Header("Player goes back")]
    [SerializeField] private AudioClip[] teleportBack;
    [Header("Player get hit")]
    [SerializeField] private AudioClip[] hitBack;
    [Header("Player goes forward")]
    [SerializeField] private AudioClip[] teleportForward;
    [Header("Player hit something")]
    [SerializeField] private AudioClip[] hitWrong;

    [SerializeField] private AudioClip PlayingThisClip;

    private bool hasTeleportedBack = false; // for 
    private IEnumerator coroutine; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        TM = FindAnyObjectByType<TeleportManager>();
        PM = GetComponent<PlayerManager>();
        
        if (PM == null)
        {
            Debug.LogError("PlayerManager saknas!");
        }

        if (TM == null)
        {
            Debug.LogError("TeleportManager saknas!");
        }

    }
    private void OnEnable() // system.action, Zion, another nice way to send music
    {
        NewEnemyBehaviour.OnEnemyHit += PlayHitSound;
    }

    private void OnDisable()// system.action, Zion, another nice way to send music
    {
        NewEnemyBehaviour.OnEnemyHit -= PlayHitSound;
    }
    private void PlayHitSound(NewEnemyBehaviour hitEnemy)// system.action, Zion, another nice way to send music
    {
        
        //This function will make it eaiser for this script to found right enemy-script due to its a lot of them.
        PlayPlayerBowSound(hitBack, "Kill an enemy");
    }
    private void PlayPlayerBowSound(AudioClip[] soundArray, string phase)
    {
        
        if (soundArray.Length > 0)
        {
            AudioClip clip = soundArray[Random.Range(0, soundArray.Length)];
            PlayingThisClip = clip;

            if (SoundSFXManager.instance != null)
            {
                SoundSFXManager.instance.PlaySoundFXClip(clip, transform, 1f);
                
            }
            else
            {
                Debug.LogWarning("SoundSFXManager saknas!");
            }
        }
    }

    public IEnumerator toggleBooleanTeleportSound()
    {
        yield return new WaitForSeconds(1.0f);
        hasTeleportedBack = false;  
       
       

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("TeleportOn: " + TM.TeleportSoundBool);
       //Debug.Log("hasTeleportedBack: " + hasTeleportedBack);
        if (TM.TeleportSoundBool == true)
        {
          
            PlayPlayerBowSound(teleportBack, "Teleport Forward");
            
        }
        else
        {
           
        }
        if (PM.RespawnShieldActive== true && !hasTeleportedBack)
        {
            //hasTeleportedBack = true;

            print("HElLO findus");
            PlayPlayerBowSound(teleportForward, "Teleport Back");

           
            hasTeleportedBack = true; // true was the first option but false is the solution if you want hear an annoying sound.           

            StartCoroutine(toggleBooleanTeleportSound());
        }
        else
        {
          
            
        }

    }
}
