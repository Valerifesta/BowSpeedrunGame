using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
//using UnityEditor.ShaderKeywordFilter;
//using UnityEditor.SpeedTree.Importer;
//using UnityEditor.Splines;
using UnityEngine;

public class NewEnemyBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject EnemyRotatingObj; //Will always rotate the assigned object on the horizontal axis. 
    [SerializeField] private ParticleSystem[] ChargeAndShoot;
    private ParticleSystem[] chargeBall;
    private ParticleSystem[] plasmaBeam;

    [SerializeField] private ParticleSystem DetectEffect;
    [SerializeField] private ParticleSystem StunnedEffect;
    
    private float rotatedAngles;
    private float degreesAwayFromPrev;
    //private float lastRotatedDegs;
    [SerializeField] private float RotationTime;
    [SerializeField] private float RotTimeScale = 1;
    //public bool CanTargetPlayer;


    //Zion
    [SerializeField] private EnemySoundList ESL;
    [SerializeField] private DectedAlarm[] DA;
    [SerializeField] private Explosion Ex;

    public System.Action OnStartRotating;
    public System.Action OnStartCharging;
    public System.Action OnShoot;
    public System.Action OnHit;

    [SerializeField] public bool isRotate;
    [SerializeField] public bool isCharging;
    [SerializeField] public bool isShoot;
    [SerializeField] public bool isKillingEnemy;
    //[SerializeField] public bool isHiting;


    //[SerializeField] private float _BurstFragmentInterval;
    //[SerializeField] private float _WholeBurstDelays;
    //[SerializeField] private int _BurstBulletAmount;
    [SerializeField] private float _BulletBeamDelay;
    [SerializeField] private float _BeamChargeUpTime = 2;
    [SerializeField] private float _LowerLimRotDistance = 2;

    [SerializeField] private float RotationStartDelay;
    [SerializeField] private float DetectionRange; //Should be Individual between variants.
    
    //[SerializeField] private Vector3 currentEuler;
    //private Coroutine runningCoroutine;

    public bool IsStunned;
    private float _stunRemaining;
    private void Start()
    {
        Ex = GetComponent<Explosion>();
        ESL = GetComponent<EnemySoundList>();
        //DA = transform.GetChild(2).GetComponentInChildren<DectedAlarm>();
        Player = FindFirstObjectByType<PlayerManager>().gameObject;
        //currentEuler = EnemyRotatingObj.transform.eulerAngles;
        //StartCoroutine(RotateTowardsPlayer(EnemyRotatingObj.transform.rotation));
        ResetRot();
        int length = ChargeAndShoot.Length;
        chargeBall = new ParticleSystem[length];
        plasmaBeam = new ParticleSystem[length];

        for (int i = 0; i < length; i++)
        {
            float chargeScale = 2 / _BeamChargeUpTime; //2 is the default value and which matches 
            if (ChargeAndShoot != null)
            {
                var mainCharge = ChargeAndShoot[i].main;
                mainCharge.simulationSpeed = 2.2f * chargeScale;

                chargeBall[i] = ChargeAndShoot[i].GetComponentsInChildren<ParticleSystem>()[1];
                var chargeBallMain = chargeBall[i].main;
                chargeBallMain.simulationSpeed = 2.2f * chargeScale;

                plasmaBeam[i] = ChargeAndShoot[i].GetComponentsInChildren<ParticleSystem>()[1].GetComponentsInChildren<ParticleSystem>()[1];
                var plasmaMain = plasmaBeam[i].main;
                plasmaMain.startDelay = 1.9f / chargeScale;
            }
            else
            {
                Debug.Log("Charge and shoot was null");
            }
            
        }
        

        

        

    }
    public void ResetRot()
    {
        EnemyRotatingObj.transform.eulerAngles = Vector3.zero;
    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TargetPlayer(Vector3.Distance(EnemyRotatingObj.transform.position, Player.transform.position));
        }
        
        /*
        if (Input.GetKeyDown(KeyCode.H))
        {
            Quaternion fixedRot = Quaternion.AngleAxis(rotatedAngles - lastRotatedDegs, Vector3.up);

            EnemyRotatingObj.transform.rotation = fixedRot;
        }*/
        if (_stunRemaining > 0)
        {
            _stunRemaining -= 1.0f * Time.deltaTime;
        }
        else if (IsStunned == true)
        {
            IsStunned = false;
            _stunRemaining = 0;
            Debug.Log("Enemy is no longer stunned");
        }
    }
    public void StartIdle()
    {
        StopAllCoroutines();
        //ChargeAndShoot.Stop();
        isShoot = false;
        isRotate = false;
        isCharging = false;
        Debug.Log("Made " + gameObject + " idle");
    }
    public void StunEnemy(float remainingStunTime)
    {
        StopAllCoroutines();
        var main = StunnedEffect.main;
        main.duration = main.simulationSpeed * remainingStunTime;
        StunnedEffect.Play();

        IsStunned = true;
        _stunRemaining = remainingStunTime;
        isRotate = false;
        isCharging = false;
        isShoot = false;
        Debug.Log("Stunned Enemy");
    }
    public void TargetPlayer(float linearDistance)
    {
        if (linearDistance < DetectionRange)
        {
            if (linearDistance > _LowerLimRotDistance)
            {
                RotTimeScale = _LowerLimRotDistance / linearDistance;
                //CanTargetPlayer = true;
                Debug.Log("Distance between player and enemy is above Lower Distance Limit and is therefore affecting rotation time.");
            }


            StopAllCoroutines();
            foreach (ParticleSystem chargeUp in ChargeAndShoot)
            {
                chargeUp.Stop();
            }

            // ChargeAndShoot.Stop();
            if (!isRotate && !isCharging && !isShoot)
            {
                DetectEffect.Play();
            }
            // if (CanTargetPlayer)
            {
                StartCoroutine(RotateTowardsPlayer(EnemyRotatingObj.transform.rotation, RotationTime));
            }
        }
        

    }
    public IEnumerator RotateTowardsPlayer(Quaternion startRot, float timeToRotate)
    {
        if (RotationStartDelay != 0.0f)
        {
            yield return new WaitForSeconds(RotationStartDelay);
        }
        OnStartRotating?.Invoke();//Zion (ljud)
        isRotate = true;//Zion (material)
        isCharging = false;
        isShoot = false;

        /*
        if (!CanTargetPlayer)
        {
            Debug.Log("Cannot target, therefore not rotate towards player");
            //runningCoroutine = null;
            yield break;
        }*/
        Debug.Log("Started Rotating Enemy" );
        
        Vector3 dir = Player.transform.position - EnemyRotatingObj.transform.position;
        dir.y = 0;

        Debug.Log("prev was " + degreesAwayFromPrev);
        float horizontalAngle = Vector3.SignedAngle(EnemyRotatingObj.transform.forward, dir, Vector3.up) - degreesAwayFromPrev; //- (lastRotatedDegs); //Forward can sometimes be wrong depending on the model orientation
        rotatedAngles += horizontalAngle;
        //lastRotatedDegs = 0;
        Debug.Log("angle between to player is " + horizontalAngle);
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
                EnemyRotatingObj.transform.rotation = tRot;
                degreesAwayFromPrev = Vector3.SignedAngle(EnemyRotatingObj.transform.forward,dir, Vector3.up); //I have no idea why this fixes the rotation offset bug, but it does. For some reason it only works if its constantly updated..???
               
                yield return null;
            }

            //if (CanTargetPlayer)
            {
                if (EnemyRotatingObj.transform.rotation != tRot)
                {
                    EnemyRotatingObj.transform.rotation = tRot;
                }
                //runningCoroutine = null;
                StartCoroutine(ChargeUp());
                Debug.Log("Finished Rotating Enemy");
            }
        }
        yield return null;
    }
    public void EnemyOnHit()
    {
        OnHit?.Invoke();
       // isHiting = true;//Zion (material)
        Debug.Log("Enemy got hit by bow");
        //Player.GetComponent<>
        isKillingEnemy = true;
        gameObject.SetActive(false);
        Ex.explode();
        //add enemy death effet here

    }
    public IEnumerator ChargeUp()
    {
        OnStartCharging?.Invoke();// zion (ljud)
        isCharging = true;//Zion (material)
        isRotate = false;
        isShoot = false;
        Vector3 orgin = EnemyRotatingObj.transform.position + EnemyRotatingObj.transform.forward;
        foreach (ParticleSystem chargeUp in ChargeAndShoot)
        {
            chargeUp.transform.forward = (Player.transform.position - orgin).normalized;
            chargeUp.Play();


        }

        float elapsedChargeTime = new float();
        float elapsedDelayTime = new float();
        while (true)
        {
            if (elapsedDelayTime <= 0)
            {
                elapsedChargeTime += 1.0f * Time.deltaTime;

                if (elapsedChargeTime >= _BeamChargeUpTime)
                {
                    elapsedDelayTime = _BulletBeamDelay;
                    //shoot
                    ShootBeam();
                    isShoot = true;//Zion (material)
                    isRotate = false;
                    isCharging = false;
                }
                else
                {
                    //Debug.Log("Charging up");
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
        OnShoot?.Invoke();// Zion(ljud)
        RaycastHit hit = new RaycastHit();
        Debug.Log("Shot beam");

        Vector3 orgin = EnemyRotatingObj.transform.position + EnemyRotatingObj.transform.forward;
        if (Physics.Raycast(orgin, (Player.transform.position- orgin).normalized, out hit))
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
    /*
    public IEnumerator PauseEnemy(float pauseTime)
    {
        CanTargetPlayer = false;
        Debug.Log("Paused Enemy");
        yield return new WaitForSeconds(pauseTime);
        CanTargetPlayer = true;
        Debug.Log("Paused Enemy");
        yield return null;
    }*/
}
