using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.SpeedTree.Importer;
using UnityEditor.Splines;
using UnityEngine;

public class NewEnemyBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject EnemyRotatingObj; //Will always rotate the assigned object on the horizontal axis. 
    private float rotatedAngles;
    [SerializeField] private float RotationTime;
    [SerializeField] private float RotTimeScale = 1;
    public bool CanTargetPlayer;


    //[SerializeField] private float _BurstFragmentInterval;
    //[SerializeField] private float _WholeBurstDelays;
    //[SerializeField] private int _BurstBulletAmount;
    [SerializeField] private float _BulletBeamDelay;
    [SerializeField] private float _BeamChargeUpTime = 2;
    [SerializeField] private float _LowerLimRotDistance = 2;
    //[SerializeField] private Vector3 currentEuler;

    private void Start()
    {
        //currentEuler = EnemyRotatingObj.transform.eulerAngles;
        //StartCoroutine(RotateTowardsPlayer(EnemyRotatingObj.transform.rotation));
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TargetPlayer(Vector3.Distance(EnemyRotatingObj.transform.position, Player.transform.position));
        }
    }
    public void TargetPlayer(float linearDistance)
    {
        if (linearDistance > _LowerLimRotDistance)
        {
            RotTimeScale = _LowerLimRotDistance / linearDistance;
            Debug.Log("Distance between player and enemy is above Lower Distance Limit and is therefore affecting rotation time.");
        }
        StartCoroutine(RotateTowardsPlayer(EnemyRotatingObj.transform.rotation, RotationTime));
    }
    public IEnumerator RotateTowardsPlayer(Quaternion startRot, float timeToRotate)
    {
        if (!CanTargetPlayer)
        {
            Debug.Log("Cannot target, therefore not rotate towards player");
            yield break;
        }
        Debug.Log("Started Rotating Enemy" );

        Vector3 dir = Player.transform.position - EnemyRotatingObj.transform.position;
        dir.y = 0;

        float horizontalAngle = Vector3.SignedAngle(EnemyRotatingObj.transform.forward, dir, Vector3.up);
        rotatedAngles += horizontalAngle;
        Debug.Log(horizontalAngle);
        if (horizontalAngle != 0)
        {
            float t = new float();
            float elapsedTime = new float();
            Quaternion endRot = Quaternion.AngleAxis(rotatedAngles, Vector3.up); //to finish+
            Quaternion tRot = new Quaternion();

            while (t < 1) //Play turning SFX that lasts for the duration of "timeToRotate"
            {
                elapsedTime += 1.0f * Time.deltaTime * RotTimeScale;
                t = elapsedTime / timeToRotate;
                tRot = Quaternion.Lerp(startRot, endRot, t);
                //float rotateAngle = Mathf.Lerp(0, horizontalAngle, t);
                //EnemyRotatingObj.transform.Rotate(Vector3.up, horizontalAngle/rotateAngle);
                EnemyRotatingObj.transform.rotation = tRot;
                yield return null;
                if (!CanTargetPlayer)
                {
                    break;
                }
            }
            if (CanTargetPlayer)
            {
                if (EnemyRotatingObj.transform.rotation != tRot)
                {
                    EnemyRotatingObj.transform.rotation = tRot;
                }
                StartCoroutine(ChargeUp());
                Debug.Log("Finished Rotating Enemy");
            }
            

            

        }
        yield return null;
    }
    public void EnemyOnHit()
    {
        Debug.Log("Enemy got hit by bow");
        gameObject.SetActive(false);

        //add enemy death effet here

    }
    public IEnumerator ChargeUp()
    {
        float elapsedChargeTime = new float();
        float elapsedDelayTime = new float();
        while (CanTargetPlayer)
        {
            if (elapsedDelayTime <= 0)
            {
                elapsedChargeTime += 1.0f * Time.deltaTime;

                if (elapsedChargeTime >= _BeamChargeUpTime)
                {
                    elapsedDelayTime = _BulletBeamDelay;
                    //shoot
                    ShootBeam();
                }
                else
                {
                    Debug.Log("Charging up");
                }
            }
            else
            {
                elapsedDelayTime -= 1.0f * Time.deltaTime;
            }

            yield return null;
           

        }
        yield return null;
    }
    public void ShootBeam()
    {
        RaycastHit hit = new RaycastHit();
        Debug.Log("Shot beam");

        if (Physics.Raycast(EnemyRotatingObj.transform.position, EnemyRotatingObj.transform.forward, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;                                                       
            if (hit.collider.gameObject == Player)
            {
                Debug.Log("Hit player!");
                Player.GetComponent<PlayerManager>().OnPlayerHit();
            }
            else
            {
                Debug.Log("Didnt hit player. Instead hit " + hit.collider.gameObject.name);
            }
        }
        else
        {
            Debug.Log("didnt hit anything");
        }

    }
    public IEnumerator PauseEnemy(float pauseTime)
    {
        CanTargetPlayer = false;
        Debug.Log("Paused Enemy");
        yield return new WaitForSeconds(pauseTime);
        CanTargetPlayer = true;
        Debug.Log("Paused Enemy");
        yield return null;
    }
}
