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
    public TestBowBehaviour _bow;
    private Collider playerColl;

    [HideInInspector] public VFXTrainMoveAnimation previousOnTrain;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerColl = GetComponent<Collider>();
    }

    // Update is called once per frame
    public void TogglePlayerInputs()
    {
        Camera.main.GetComponent<CameraBehaviour>().ToggleFreeMouse();
        _bow.ToggleBowInputs();
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
            _bow.UpdateAggros(false);
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

    public GameObject StandingOnTrain() //only works for non-player trains. Collider should be a child of the train cart.
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(gameObject.transform.position + -gameObject.transform.up, 1.5f, Vector3.down);
        foreach (RaycastHit rayHit in hits)
        {
            Collider coll = rayHit.collider;
            if (coll != playerColl)
            {
                if (coll.tag == "WalkArea" && coll.transform.parent != null)
                {
                    Debug.Log("Returned parented train of collider underneath");
                    return coll.transform.parent.gameObject;
                }
                else if (coll.transform.parent == null)
                {
                    Debug.Log("There is no parent underneath you!!!!");
                }
            }
            
        }
        
         
        return null;
    }

    public void TryChangeWholeTrainAnimation()
    {
        Debug.Log("Trying to change whole train animation");
        GameObject train = StandingOnTrain();
        if (train != null && train.transform.parent != null)
        {
            GameObject wholeTrain = train.transform.parent.gameObject;
            if (wholeTrain != null && wholeTrain.GetComponent<VFXTrainMoveAnimation>())
            {
                VFXTrainMoveAnimation trainAnimation = wholeTrain.GetComponent<VFXTrainMoveAnimation>();
                if (previousOnTrain != null && previousOnTrain != trainAnimation)
                {
                    previousOnTrain.resumeAnimation();
                    //Restart animation of previous train here
                    Debug.Log("Ended previous train animation");

                }
                else
                {
                    Debug.Log("either previousOnTrain is null or the same train as you teleported to.");
                }
                trainAnimation.endAnimation();

                previousOnTrain = trainAnimation;
                Debug.Log("Started new train animation");
                //start animation of new train here
            }
            else
            {
                Debug.Log("No whole train or vfx train animation");
            }
        }
        else
        {
            Debug.Log("Train or full train is null");
        }
    }

}
