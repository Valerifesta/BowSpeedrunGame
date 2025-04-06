using Unity.Hierarchy;
using UnityEngine;

public class TeslaTower : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator teslaAnimator;
    private bool hasDeployed;
    public override void Start()
    {
        base.Start();
        teslaAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public override void TargetPlayer(float linearDistance)
    {
        base.TargetPlayer(linearDistance);
        if (playerWithinDetectionRange && !hasDeployed)
        {
            teslaAnimator.SetTrigger("PlayerHere");
            hasDeployed = true;
        }
    }
    public override void StartIdle()
    {
        base.StartIdle();
        if (hasDeployed)
        {
            teslaAnimator.SetTrigger("NoPlayerHere");
            hasDeployed = false;
        }
    }
}
