using UnityEngine;

public class TestEnemy2 : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void EnemyOnHit()
    {
        base.EnemyOnHit();
        Destroy(gameObject);
    }
}
