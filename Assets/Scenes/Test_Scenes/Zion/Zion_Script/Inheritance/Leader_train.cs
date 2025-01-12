using UnityEngine;

public class Leader_train : Train_Inheritance
{
    public GameObject Leader;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Leader = gameObject.GetComponent<GameObject>();
        TrainType = "Leader-train";
        TrainShake = 0f;
        TrainGo = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FollowMeLock")) // ev error med leadertrain...
        {
            print("found some cart but I wont follow");
            //transform.position = TheNextCartPosition.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(TrainGo == true)
        {
            rb.AddForce(moveDirection.normalized * 100f, ForceMode.Force);

        } 
    }
}
