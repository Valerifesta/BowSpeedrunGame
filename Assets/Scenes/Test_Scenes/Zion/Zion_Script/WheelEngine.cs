using UnityEngine;

public class WheelEngine : MonoBehaviour
{
    private Animator anim;
    private bool GameOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        GameOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOn == true)
        {
            anim.SetBool("WheelsGo", true);

        }
       
    }
}
