using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurrentAninamtion : MonoBehaviour
{

    private Animator anim;
    //private TurrentLevel2 turrentLevel2;
    private NewEnemyBehaviour NEB;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        anim = GetComponent<Animator>();
       // NEB = GetComponent<TurrentLevel2>();
        NEB = GetComponentInParent<NewEnemyBehaviour>();
      


        if (NEB == null)
        {
            Debug.LogError(" NEB-komponenten kunde inte hittas!");
        }


    }

    // Update is called once per frame
    private void Update()
    {
        if (NEB.isShoot == false && NEB.isCharging == false && !NEB.IsStunned)
        {
            anim.SetBool("KillPlayer", false);
            //anim.SetFloat("Speed", 1.0f);
        }
        /*else if (turrentLevel2.isRotate == false && turrentLevel2.isShoot == false && turrentLevel2.isCharging == false && !turrentLevel2.IsStunned)
        {
            anim.SetBool("KillPlayer", true);
            //anim.SetFloat("Speed", -1.0f);
        }*/
        else if (NEB.isCharging == true && !NEB.IsStunned)
        {
            anim.SetBool("KillPlayer", true);
            //anim.SetFloat("Speed", -1.0f);
        }
        else if (NEB.isShoot == true && !NEB.IsStunned)
        {
            anim.SetBool("KillPlayer", true);
            //anim.SetFloat("Speed", -1.0f);
        }
        else
        {
            anim.SetBool("KillPlayer", false);
        }
    }
}

