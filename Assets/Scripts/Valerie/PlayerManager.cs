using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool RespawnShieldActive;
    [SerializeField] float _respawnShieldDuration;

    //public int TimesHit;
    [SerializeField] private GameManager _GameMan;
    [SerializeField] private TeleportManager _TeleportMan;
    public float ShieldTimeRemaining;
    [SerializeField] private TestBowBehaviour _bow;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void TogglePlayerInputs()
    {
        Camera.main.GetComponent<CameraBehaviour>().ToggleFreeMouse();
        _bow.CanUpdateBowInputs = !_bow.CanUpdateBowInputs;
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
            ShieldTimeRemaining = 0;
            _bow.UpdateAggros();
            //RespawnShieldActive = false;
            //remove ethereal material on player

        }
    }
    public void OnPlayerHit()
    {
        if (ShieldTimeRemaining <= 0)
        {
            _GameMan.TimesHit += 1;

            _TeleportMan.TeleportToLast(gameObject);

            Debug.Log("Player got hit");
            ShieldTimeRemaining = 0.1f;
        }
        else
        {
            Debug.Log("Player shield is active, could not get hit");
        }
        

    }
    IEnumerator ActivateShieldDuration()
    {
        ShieldTimeRemaining = _respawnShieldDuration;
        _bow.UpdateAggros(); //Stuns enemies nearby since ShieldTimeRemaining is above 0.
        while (ShieldTimeRemaining > 0)
        {
            ShieldTimeRemaining -= 1.0f * Time.deltaTime;
            yield return null;
        }
        ToggleRespawnShield();
        yield return null;
    }

}
