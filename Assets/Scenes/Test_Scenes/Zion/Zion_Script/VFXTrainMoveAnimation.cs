using UnityEngine;

public class VFXTrainMoveAnimation : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("NonePlayer");
        }
        else
        {
           
            anim.SetTrigger("PlayerOnRoof");
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
