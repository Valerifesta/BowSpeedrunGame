using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VFXTrainMoveAnimation : MonoBehaviour
{
   [SerializeField] private Animator anim;
   
    public bool isPlaying = true;


    private void Start()
    {
        isPlaying = true;

        anim = GetComponent<Animator>();
        if(anim == null)
        {
            Debug.Log("no animator in this Train");
        }
       // anim.speed = 0;
       

       
        // Starta NonePlayer-animationen vid start
        //anim.SetTrigger("NonePlayer");
        if (isPlaying == true)
        {
            anim.SetBool("Paused", false);
            anim.SetTrigger("NonePlayer");
        }
      
     
      

    }

    private void OnTriggerEnter(Collider other)
    {
        
        
        if(other.CompareTag("Player"))
        {
            isPlaying = false;
            print("Player on bridge");
           
            if(isPlaying == false)
            {
               
                //isPlaying = false;
                anim.SetBool("Paused", true);
                // Pausa animationen
                anim.speed = 0;
                Debug.Log("Animation pausad");
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            //isPlaying = false;
            isPlaying = true;
            if (isPlaying == true)
            {
                //isPlaying = true;
                anim.SetBool("Paused", false);
                // Återuppta animationen
                anim.speed = 1;
                
                Debug.Log("Animation återupptagen");
            }
            
        }
    }
}
