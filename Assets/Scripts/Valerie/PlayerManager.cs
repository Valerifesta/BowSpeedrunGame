using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool RespawnShieldActive;
    [SerializeField] float _respawnShieldDuration;

    public int HitPoints;
    [SerializeField] private GameManager _GameMan;
    [SerializeField] private TeleportManager _TeleportMan;
    public float ElapsedShieldDuration;
    [SerializeField] private TestBowBehaviour _bow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleRespawnShield()
    {
        RespawnShieldActive = !RespawnShieldActive;

        if (RespawnShieldActive)
        {
            StartCoroutine(ActivateShieldDuration());
            Debug.Log("Activated shield");
            //set ethereal material here on player
            //Disable "CanTargetPlayer" bool on all enemies that is inside bounding volume. 

        }
        else
        {
            Debug.Log("RemovedShield");
            ElapsedShieldDuration = 0;
            //RespawnShieldActive = false;
            //remove ethereal material on player

        }
    }
    public void OnPlayerHit()
    {
        HitPoints -= 1;
        //_bow.TryDeaggroEnemies(); 
        _GameMan.AddScore(-2);
        _TeleportMan.TeleportToLast(gameObject);

    }
    IEnumerator ActivateShieldDuration()
    {
        
        while (ElapsedShieldDuration < _respawnShieldDuration)
        {
            ElapsedShieldDuration += 1.0f * Time.deltaTime;
            yield return null;
        }
        ToggleRespawnShield();
        yield return null;
    }

}
