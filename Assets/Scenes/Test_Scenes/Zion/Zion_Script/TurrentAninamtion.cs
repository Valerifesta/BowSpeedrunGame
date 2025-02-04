using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurrentAninamtion : MonoBehaviour
{
   
    private Animator anim = null;
    private TurrentLevel2 turrentLevel2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = this.GetComponent<Animator>();
        turrentLevel2.GetComponent<TurrentLevel2>();
    }

    // Update is called once per frame
   private void Update()
    {
        if (turrentLevel2.isRotate == true)
        {
            anim.SetBool("KillPlayer", true);
            anim.SetFloat("Speed", 1.0f);
        }
        else if (turrentLevel2.isRotate == false && turrentLevel2.isShoot == false && turrentLevel2.isCharging == false && !turrentLevel2.IsStunned)
        {
            anim.SetBool("KillPlayer", true);
            anim.SetFloat("Speed", -1.0f);
        }
        else
        {
            anim.SetBool("KillPlayer", false);
        }
    }
}
