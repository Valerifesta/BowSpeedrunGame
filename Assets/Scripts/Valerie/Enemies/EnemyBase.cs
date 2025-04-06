using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Root Variables
    private protected bool playerWithinDetectionRange;
    private protected GameObject Player;

    //
    public bool IsStunned;
    private float _stunRemaining;

    [SerializeField] private float DetectionRange; //Should be Individual between variants.



    [Header("VFX")]
    [SerializeField] private ParticleSystem StunnedEffect;

    public virtual void Start()
    {
        Player = FindFirstObjectByType<PlayerManager>().gameObject;

    }


    // Update is called once per frame
    void Update()
    {
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

    public virtual void StartIdle()
    {
        StopAllCoroutines();

        Debug.Log("Made " + gameObject + " idle");
    }

    public virtual void EnemyOnHit() //When hit by player
    {
        Debug.Log(gameObject + " was hit by player");

    }
    public virtual void StunEnemy(float remainingStunTime)
    {
        StopAllCoroutines();
        if (StunnedEffect != null)
        {
            var main = StunnedEffect.main;
            main.duration = main.simulationSpeed * remainingStunTime;
            StunnedEffect.Play();

        }
        else
        {
            Debug.Log(gameObject + ": Stun effect NULL");
        }

        IsStunned = true;
        _stunRemaining = remainingStunTime;

        Debug.Log("Stunned enemy: " + gameObject);
    }

    public virtual void TargetPlayer(float linearDistance)
    {
        if (linearDistance < DetectionRange)
        {
            playerWithinDetectionRange = true;

            StopAllCoroutines();
        }

    }
}
