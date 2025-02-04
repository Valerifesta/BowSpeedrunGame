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
    public GameManager GameMan;
    private bool flying;
    private float timeUntilDespawn;
    private void Start()
    {
        flying = true;
        timeUntilDespawn = 10.0f;
        rb = GetComponent<Rigidbody>();
        if (!teleportToggled)
        {
            GetComponent<ParticleSystem>().Play();
        }
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude > 2.5f)
        {
            //Vector3 nextRot = Vector3.Lerp(transform.up, rb.linearVelocity, rb.linearVelocity.magnitude / 100);
            transform.up = rb.linearVelocity.normalized;

        }
        if (timeUntilDespawn > 0)
        {
            timeUntilDespawn -= 1.0f * Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
    

    private void OnCollisionEnter(Collision collision)
    {
        tag = collision.collider.tag;
        if (teleportToggled)
        {
            if (tag == "WalkArea" || tag == "PlayerTrainWalkArea")// && sender.playerManager.ShieldTimeRemaining <= 0)
            {
                Vector3 offsettedPos = new Vector3();
                RaycastHit determineOffset = new RaycastHit();
                Vector3 point = collision.GetContact(0).point;
                /*if (Physics.Raycast(point + (Vector3.up * 2), -Vector3.up, out determineOffset, 2.0f))
                {
                    if (determineOffset.collider == collision.collider)
                    {
                        offsettedPos = new Vector3(point.x, (sender.Player.GetComponent<CapsuleCollider>().height/2) + point.y + Mathf.Abs(point.y - determineOffset.point.y), point.z);
                        Debug.Log("Teleported to the platform above");
                    }
                }*/
                sender.TeleportPlayer(point + new Vector3(0.0f, sender.Player.GetComponent<CapsuleCollider>().height / 2, 0));

                if (tag == "PlayerTrainWalkArea")
                {
                    Debug.Log("Teleported to player train");
                    if (GameMan.EnemiesRemaining <= 0)
                    {
                        MoveTrainIntro mover = GameMan.TrainMover;
                        mover.PlayerTrainExitLevel();
                        GameSettings.PlayerPosLastScene = sender.Player.transform.localPosition;

                    }
                }
                gameObject.SetActive(false);


            }
           
        }
        else
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                NewEnemyBehaviour behaviour = collision.gameObject.transform.parent.gameObject.GetComponent<NewEnemyBehaviour>();
                behaviour.EnemyOnHit();
                sender.previousEnemies.Remove(behaviour);
                //GameMan.EnemiesHit += 1;
                GameMan.OnEnemyHit();
                Debug.Log("Arrow Hit Enemy");
                gameObject.SetActive(false);
            }
            if (tag == "WalkArea" || tag == "PlayerTrainWalkArea")
            {
                rb.isKinematic = true;
                timeUntilDespawn = 10.0f;
            }
        }

        flying = false;
    }
    

}
