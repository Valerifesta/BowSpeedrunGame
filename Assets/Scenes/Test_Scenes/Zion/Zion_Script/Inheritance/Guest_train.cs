using UnityEngine;

public class Guest_train : Train_Inheritance
{
    public bool HaveALeader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrainType = "Guest-train";
        TrainShake = 2f;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FollowMeLock")) // ev error med leadertrain...
        {
            print("found some and I am just a guest");
            transform.position = TheNextCartLockPosition.position;
            HaveALeader = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HaveALeader == true)
        {
            TrainGo = true;
            transform.position = TheNextCartLockPosition.position;
        }
    }
}
