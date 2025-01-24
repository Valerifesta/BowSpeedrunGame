using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] Vector3[] PreviousTeleportPos = new Vector3[2];
    [SerializeField] Vector3 SpawnPos;
    [SerializeField] private GameObject _spawnPosObj;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject player;
    [SerializeField] private bool CanTeleportBack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Start()
    {
        SpawnPos = _spawnPosObj.transform.position;
        playerManager = FindFirstObjectByType<PlayerManager>();
    }
    public void UpdateTeleportArray(Vector3 newTeleportPos)
    {
        
        PreviousTeleportPos[0] = PreviousTeleportPos[1];
        PreviousTeleportPos[1] = newTeleportPos;

    }

    public void Teleport(GameObject objectToTeleport, Vector3 desiredPos)
    {
        objectToTeleport.transform.position = desiredPos;

        //add sound effect calls here
    }
    public void TeleportToLast(GameObject objectToTeleport)
    {
        if (CanTeleportBack)
        {
            Vector3 lastPoint = new Vector3();
            if (PreviousTeleportPos[0] != Vector3.zero)
            {
                lastPoint = PreviousTeleportPos[0]; //"Point is kind of an unintuitive name since the teleport position gets offsetted depending on what you hit and is not the actual point of contact.

            }
            else
            {
                lastPoint = SpawnPos;
            }
            objectToTeleport.transform.position = lastPoint;
            playerManager.ToggleRespawnShield();

        }


        //special effect here
        //delay for after

        //player

    }
}
