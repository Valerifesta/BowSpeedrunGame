using Microsoft.Win32;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;
    public bool teleportToggled;
    public TestBowBehaviour sender;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude > 10)
        {
            //Vector3 nextRot = Vector3.Lerp(transform.up, rb.linearVelocity, rb.linearVelocity.magnitude / 100);
            transform.up = rb.linearVelocity.normalized;

        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("WalkArea"))
        {
            Vector3 offsettedPos = new Vector3();
            RaycastHit determineOffset = new RaycastHit();
            Vector3 point = collision.GetContact(0).point;
            if (Physics.Raycast(point + (Vector3.up * 2), -Vector3.up, out determineOffset, 2.0f))
            {
                if (determineOffset.collider == collision.collider)
                {
                    offsettedPos = new Vector3(point.x, (sender.Player.GetComponent<CapsuleCollider>().height/2) + point.y + Mathf.Abs(point.y - determineOffset.point.y), point.z);
                    Debug.Log("Teleported to the platform above");
                }
            }
           sender.TeleportPlayer(offsettedPos);
            gameObject.SetActive(false);
        }
    }
    

}
