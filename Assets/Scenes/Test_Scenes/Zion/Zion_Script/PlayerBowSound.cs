using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBowSound : MonoBehaviour//By ZION
{
    public PlayerManager PM;
    public TeleportManager TM;
    private NewEnemyBehaviour NEB;
    private TestBowBehaviour TBB;
   // private SoundSFXManager SFX;

    [Header("Arrow sounds")]
    [SerializeField] private AudioClip switchToElArrow;
    [SerializeField] private AudioClip switchToTPArrow;
    [SerializeField] private AudioClip FlyLittleBird;
    //private bool hasSwitch;

    [Header("Player goes back")]
    [SerializeField] private AudioClip[] teleportBack;
    [Header("Player get hit")]
    [SerializeField] private AudioClip[] hitBack;
    [Header("Player goes forward")]
    [SerializeField] private AudioClip[] teleportForward;
    [Header("Player hit something")]
    [SerializeField] private AudioClip[] hitWrong;

    [Header("Playing rn")]
    [SerializeField] private AudioClip PlayingThisClip;

    private bool hasTeleportedBack = false; // for 
    private bool previousSwitchArrowState = false;
    private IEnumerator coroutine; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        TM = FindAnyObjectByType<TeleportManager>();
        PM = GetComponent<PlayerManager>();
        TBB = FindAnyObjectByType<TestBowBehaviour>();
        if (TBB == null)
        {
            Debug.LogError("TestBowBehaviour saknas!"); //Will complain if this script is not available but the will game will not crash, thanks to debug.logError
        }

        if (PM == null)
        {
            Debug.LogError("PlayerManager saknas!");//Will complain if this script is not available but the will game will not crash, thanks to debug.logError
        }

        if (TM == null)
        {
            Debug.LogError("TeleportManager saknas!");//Will complain if this script is not available but the will game will not crash, thanks to debug.logError
        }
        if (TBB != null)
        {
            previousSwitchArrowState = TBB._switchArrow;
        }
    }
    private void OnEnable() //
    {
        NewEnemyBehaviour.OnEnemyHit += PlayHitSound;
    }

    private void OnDisable()// 
    {
        NewEnemyBehaviour.OnEnemyHit -= PlayHitSound;
    }
    private void PlayHitSound(NewEnemyBehaviour hitEnemy)// system.action, 
    {
        
                                                                                                                                                                                                                       //This function will make it eaiser for this script to found right enemy-script due to its a lot of them.
        PlayPlayerBowSound(hitBack, "Kill an enemy");
    }
    private void PlayPlayerBowSound(AudioClip[] soundArray, string phase)
    {
        
        if (soundArray.Length > 0)
        {
            AudioClip clip = soundArray[Random.Range(0, soundArray.Length)]; // This choose a random sound in the sound-list
            PlayingThisClip = clip;

            if (SoundSFXManager.instance != null)
            {
                SoundSFXManager.instance.PlaySoundFXClip(clip, transform, 1f); //Based On "SoundSFXManager"-script
                
            }
            else
            {
                Debug.LogWarning("SoundSFXManager saknas!");//Will complain if there´s not "SoundSFXManager"-script initiated
            }
        }
    }

    public IEnumerator toggleBooleanTeleportSound() //This function help because it reset the boolean value
    {
        yield return new WaitForSeconds(1.0f);
        hasTeleportedBack = false;  
       
       

    }

   
 
    public void switchArrowSound()// make sound when you switch arrow-type 
    {
        if (TBB == null) return;

        print("whats going on");
        if (TBB._switchArrow)
        {
            SoundSFXManager.instance.PlaySoundFXClip(switchToTPArrow, transform, 1f); //Make sound
            print("Bytte till TP-pil");
        }
        else
        {
            SoundSFXManager.instance.PlaySoundFXClip(switchToElArrow, transform, 1f); //Make sound
            print("Bytte till EL-pil"); 
        }

    }

    public void FlyBird()
    {
        
        SoundSFXManager.instance.PlaySoundFXClip(FlyLittleBird, transform, 1f); //Make sound

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("TBB._switchArrow: " + TBB._switchArrow); // Explain when the boolean TBB._switchArrow have changed

        if (TBB.shoot== true)
        {
            FlyBird();
        }
        //Debug.Log("TeleportOn: " + TM.TeleportSoundBool);
        //Debug.Log("hasTeleportedBack: " + hasTeleportedBack);
        //GBY
        if (TBB != null && TBB._switchArrow != previousSwitchArrowState)
        {
            switchArrowSound();
            previousSwitchArrowState = TBB._switchArrow;
        }

        if (TM.TeleportSoundBool == true)
        {
          
            PlayPlayerBowSound(teleportBack, "Teleport Forward"); //Make sound
            
        }
        else
        {
           
        }
        if (PM.RespawnShieldActive== true && !hasTeleportedBack)
        {
            //hasTeleportedBack = true;

           
            PlayPlayerBowSound(teleportForward, "Teleport Back"); //Make sound


            hasTeleportedBack = true; //                                                                                                                                                                                                true was the first option but false is the solution if you want hear an annoying sound.           

            StartCoroutine(toggleBooleanTeleportSound()); 
        }
        else
        {
          
            
        }

    }
}
