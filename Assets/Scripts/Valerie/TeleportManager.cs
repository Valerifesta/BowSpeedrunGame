using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] Vector3[] PreviousTeleportPos = new Vector3[2];
    //[SerializeField] Vector3 SpawnPos;
    [SerializeField] private GameObject _spawnPosObj;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject player;
    //[SerializeField] private MasterMind mm;
    [SerializeField] private PostProcessManager PPM;
    [SerializeField] public bool TeleportOn;
    //[SerializeField] private bool CanTeleportBack;
    public float CallbackTime;

    private TutorialScript tutorial;
    //public float T;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Start()
    {
        //SpawnPos = _spawnPosObj.transform.position;
        playerManager = FindFirstObjectByType<PlayerManager>();
        tutorial = FindFirstObjectByType<TutorialScript>();
        //mm = FindFirstObjectByType<MasterMind>();
        PPM = FindFirstObjectByType<PostProcessManager>();

        if (CallbackTime <= 0)
        {
            CallbackTime = 1.0f;
        }
        if (tutorial == null)
        {
            if (GameSettings.PlayerPosLastScene != Vector3.zero)
            {
                _spawnPosObj.transform.localPosition = GameSettings.PlayerPosLastScene;
            }
        }
    }
    public void UpdateTeleportArray(Vector3 newTeleportPos)
    {
        
        PreviousTeleportPos[0] = PreviousTeleportPos[1];
        PreviousTeleportPos[1] = newTeleportPos;

    }

    public void Teleport(GameObject objectToTeleport, Vector3 desiredPos)
    {
        objectToTeleport.transform.position = desiredPos;
        if (tutorial != null)
        {
            tutorial.OnTeleport(desiredPos);

        }
        TeleportOn = true;


        //add sound effect calls here
    }
    public void TeleportToLast(GameObject objectToTeleport)
    {
        Vector3 lastPoint = new Vector3();
        if (PreviousTeleportPos[0] != Vector3.zero)
        {
            lastPoint = PreviousTeleportPos[0]; //"Point is kind of an unintuitive name since the teleport position gets offsetted depending on what you hit and is not the actual point of contact.

        }
        else
        {
            lastPoint = _spawnPosObj.transform.position;
        }
        UpdateTeleportArray(lastPoint);
        StartCoroutine(Callback(objectToTeleport, lastPoint));
        // mm.TeleportOn = true;
        TeleportOn = true;
        //special effect here
        //delay for after

        //player

    }
    private IEnumerator Callback(GameObject objectToTravel, Vector3 endPos)
    {
        Debug.Log("Started Callback");
        float t = new float();
        float fixedT = new float();
        if (tutorial == null)
        {
            playerManager._bow.ToggleBowInputs();
        }
        Vector3 startPos = objectToTravel.transform.position;
        while (t < CallbackTime)
        {
            t += 1.0f * Time.deltaTime;
            fixedT = t / CallbackTime;
            objectToTravel.transform.position = Vector3.Lerp(startPos, endPos, fixedT);
            yield return null;
        }
        if (tutorial == null)
        {
            playerManager._bow.ToggleBowInputs();
        }
        objectToTravel.transform.position = endPos;
        playerManager.ToggleRespawnShield();
        Debug.Log("Ended Callback");

        yield return null;
    }
    
}
