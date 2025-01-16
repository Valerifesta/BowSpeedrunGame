using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool RespawnShieldActive;
    [SerializeField] float _respawnShieldDuration;

    public int HitPoints;
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
            //set ethereal material here on player
            //Disable "CanTargetPlayer" bool on all enemies that is inside bounding volume. 

        }
        else
        {
            RespawnShieldActive = false;
            //remove ethereal material on player

        }
    }
    IEnumerator ActivateShieldDuration()
    {
        float time = new float();
        while (time < _respawnShieldDuration)
        {
            time += 1.0f * Time.deltaTime;
        }
        ToggleRespawnShield();
        yield return null;
    }

}
