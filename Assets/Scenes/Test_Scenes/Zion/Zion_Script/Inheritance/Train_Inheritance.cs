using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Train_Inheritance : MonoBehaviour
{
    public string TrainType;
    public float TrainShake, TrainCurve;
    public bool TrainGo;
    public Vector3 moveDirection;
    public Rigidbody rb;


    public Transform TheNextCartLockPosition;


    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FollowMeLock")) // ev error med leadertrain...
        {
            print("found some cart");
            transform.position = TheNextCartLockPosition.position;
            var rigidBody = this.gameObject.GetComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
