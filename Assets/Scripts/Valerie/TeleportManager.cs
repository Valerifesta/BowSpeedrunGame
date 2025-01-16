using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] Vector3[] PreviousTeleportPos = new Vector3[2];
    [SerializeField] Vector3 SpawnPos;
    [SerializeField] private PlayerManager playerManager;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Start()
    {
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

        Vector3 lastPoint = PreviousTeleportPos[0]; //"Point is kind of an unintuitive name since the teleport position gets offsetted depending on what you hit and is not the actual point of contact.
        objectToTeleport.transform.position = lastPoint;
        
        //special effect here
        //delay for after
        playerManager.ToggleRespawnShield();

    }
}
