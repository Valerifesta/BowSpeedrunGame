using UnityEngine;

public class CranWheelAnim : MonoBehaviour
{
    private CraneRotate CR;
    private Animator anim;
    private bool rotateAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        CR = GetComponentInChildren<CraneRotate>();   
    }

    private void CranAnimation()
    {
        if(CR.isRotating == true)
        {
            anim.SetBool("isRotating", true);


        }
        else if (CR.isRotating == false)
        {
            anim.SetBool("isRotating", false);
        }

    }
    // Update is called once per frame
    void Update()
    {
        CranAnimation();
    }
}
