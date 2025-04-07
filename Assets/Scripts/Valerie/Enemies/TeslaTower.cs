using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class TeslaTower : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator teslaAnimator;
    private bool hasDeployed;

    private bool IsAttacking;
    [SerializeField] private GameObject TeslaAOE_prefab;
    [SerializeField] private GameObject TeslaAOE_pivot;
    private GameObject _aoeObject;

    [Header("Stats")]
    [SerializeField] private float Range; //Radius of AOE
    [SerializeField] private float TimeToFullyCharge;
    
    public override void Start()
    {
        base.Start();
        teslaAnimator = GetComponent<Animator>();

        TrySpawnReusableAOE();
        
    }

    // Update is called once per frame

    public override void TargetPlayer(float linearDistance)
    {
        base.TargetPlayer(linearDistance);
        if (playerWithinDetectionRange && !hasDeployed) //Takes ish 3 seconds for it to visually finish deploying. Call "OnFinishDeploy" in 3 seconds?
        {
            teslaAnimator.SetTrigger("PlayerHere");
            hasDeployed = true;
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
        }
    }
    IEnumerator OnFinishDeploy(float delay)
    {
        yield return new WaitForSeconds(delay);

        //OnDeploy VFX
        //OnDeploy SFX

        StartCoroutine(InitiateAttack(5.0f));
        yield return null;
    }
    IEnumerator InitiateAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        _aoeObject.SetActive(true);

        float t = new float();
        float currentChargeRange = new float();
        float fixedT = new float();
        while (t < TimeToFullyCharge)
        {
            t += 1.0f * Time.deltaTime;
            fixedT = Mathf.InverseLerp(0.0f, TimeToFullyCharge, t);
            currentChargeRange = Mathf.Lerp(0.0f, Range, fixedT);
            
            yield return null;
        }

        yield return null;
    }
}
