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
      
        if (isPlaying == true)
        {
            //anim.SetBool("Paused", false);
            anim.SetTrigger("NonePlayer");
        }
      
     
      

    }

   

    public void startAnimation()
    {
        anim.SetBool("Paused", false);
        anim.speed = 1;
        anim.SetTrigger("NonePlayer");
    }
    public void resumeAnimation()
    {
        anim.speed = 1;
    }
    public void endAnimation()
    {
        anim.SetBool("Paused", true);
        // Pausa animationen
        anim.speed = 0;
        Debug.Log("Animation pausad");
    }
}
