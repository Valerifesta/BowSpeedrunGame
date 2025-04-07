using System.Collections;
using System.Drawing;
using Unity.Hierarchy;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TeslaTower : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator teslaAnimator;
    private bool hasDeployed;
    private bool canBeginAttackCycle;
    private bool IsAttacking;
    private bool EngineDestroyed;
    [SerializeField] private GameObject TeslaAOE_prefab;
    [SerializeField] private GameObject TeslaAOE_pivot;
    [SerializeField] private GameObject EngineObject;
    private GameObject _aoeObject;
    private Vector3 _aoeOriginalScale;
    private float _timeSinceLastAttack;

    
    [Header("Stats")]
    [SerializeField] private float Range; //Radius of AOE
    [SerializeField] private float TimeToFullyCharge;
    [SerializeField] private float AttackIntervals;
    
    public override void Start()
    {
        base.Start();
        teslaAnimator = GetComponent<Animator>();

        TrySpawnReusableAOE();
        
    }

    public override void Update()
    {
        base.Update();
        if (!IsAttacking && hasDeployed && !IsStunned && canBeginAttackCycle)
        {
            _timeSinceLastAttack += 1.0f * Time.deltaTime;
            if (_timeSinceLastAttack > AttackIntervals)
            {
                StartCoroutine(InitiateAttack(0.0f));
            }
        }
    }
    // Update is called once per frame

    public override void EnemyOnHit()
    {
        base.EnemyOnHit();
        if (IsAttacking)
        {
            cancelAttack();
        }
        EngineObject.gameObject.SetActive(false);
        EngineDestroyed = true;
        StartIdle();

    }
    public override void TargetPlayer(float linearDistance)
    {
        base.TargetPlayer(linearDistance);
        if (playerWithinDetectionRange && !hasDeployed && !EngineDestroyed) //Takes ish 3 seconds for it to visually finish deploying. Call "OnFinishDeploy" in 3 seconds?
        {
            hasDeployed = true;
            canBeginAttackCycle = false;
            teslaAnimator.SetTrigger("PlayerHere");
            StartCoroutine(OnFinishDeploy(3.0f));
        }
    }
    public override void StartIdle()
    {
        base.StartIdle();
        if (hasDeployed && !IsAttacking)
        {
            teslaAnimator.SetTrigger("NoPlayerHere");
            hasDeployed = false;
        }
    }
    private void TrySpawnReusableAOE()
    {
        if (_aoeObject == null)
        {
            _aoeObject = Instantiate<GameObject>(TeslaAOE_prefab).gameObject;
            _aoeObject.transform.parent = gameObject.transform;
            _aoeObject.transform.position = TeslaAOE_pivot.transform.position;
            _aoeObject.SetActive(false);
            _aoeOriginalScale = _aoeObject.transform.localScale;
        }
    }
    public override void StunEnemy(float remainingStunTime)
    {
        base.StunEnemy(remainingStunTime);
        if (IsAttacking)
        {
            cancelAttack();
        }
           
        
    }
    IEnumerator OnFinishDeploy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Tesla finished deploying");


        //OnDeploy VFX
        //OnDeploy SFX

        StartCoroutine(InitiateAttack(3.0f));
        canBeginAttackCycle = true;
        yield return null;
    }
    IEnumerator InitiateAttack(float delay)
    {
        yield return null;

        IsAttacking = true;
        _aoeObject.SetActive(true);
        Debug.Log("Tesla delaying attack with " +delay + " seconds");
        yield return new WaitForSeconds(delay);
        Debug.Log("Tesla started to initiate attack");


        float t = new float();
        float currentChargeRange = new float();
        float fixedT = new float();
        while (t < TimeToFullyCharge)
        {
            t += 1.0f * Time.deltaTime;
            fixedT = Mathf.InverseLerp(0.0f, TimeToFullyCharge, t);
            currentChargeRange = Mathf.Lerp(0.0f, Range, fixedT);
            _aoeObject.transform.localScale = (Vector3.one * currentChargeRange) / 2.0f;
            
            yield return null;
        }

        Debug.Log("Tesla executing attack");

        Collider[] colls = Physics.OverlapSphere(_aoeObject.transform.position, Range);
        foreach (Collider collider in colls)
        {
            Debug.Log("Tesla hit " + collider);
            if (collider.GetComponent<PlayerManager>())
            {
                collider.GetComponent<PlayerManager>().OnPlayerHit();
            }
            yield return null;
        }
        _aoeObject.SetActive(false);
        _aoeObject.transform.localScale = _aoeOriginalScale;
        Debug.Log("Tesla finished attack");
        IsAttacking = false;
        yield return null;
    }
  
    void cancelAttack()
    {
        IsAttacking = false;
        _aoeObject.transform.localScale = _aoeOriginalScale;
        _aoeObject.SetActive(false);
    }
}
