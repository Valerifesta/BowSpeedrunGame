using UnityEngine;
using System.Collections;

public class RailCreater : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectToSpawn; // Prefab att spawna
    [SerializeField] private float spawnDelay = 2f; // Sekunder innan ny spawn
    [SerializeField] private float spawnDistance = 10f; // Hur l�ngt fram objektet spawnas
    [SerializeField] private Collider col;
    //[SerializeField] private string triggerTag = "Player"; // Tagg f�r objekt som triggar respawn

    //[Header("Optional Settings")]
    // [SerializeField] private bool randomizePosition = false; // Om position ska vara slumpm�ssig
    // [SerializeField] private float randomRange = 5f; // Omr�de f�r slumpm�ssig position

    [SerializeField] private bool isWaitingToSpawn = false;


    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        // Kolla om objektet som kolliderar har r�tt tagg
        if (other.CompareTag("Player") && !isWaitingToSpawn)
        {
            print("Whoops");
            // Starta respawn-processen
            StartCoroutine(RespawnObject());
        }
       
    }

    private IEnumerator RespawnObject()
    {
        print("Create time");
        isWaitingToSpawn = true;
        gameObject.SetActive(false);
        print("Creating almost done");
        yield return new WaitForSeconds(spawnDelay);
        print("Create done");
        // Ber�kna ny position
        Vector3 newPosition = CalculateSpawnPosition();

        // Skapa nytt objekt
        GameObject newObject = Instantiate(objectToSpawn, newPosition, Quaternion.identity);

       
        RailCreater newRespawner = newObject.AddComponent<RailCreater>();
        CopySettings(newRespawner);
        Destroy(gameObject);
        print("its over now");
    }

    private Vector3 CalculateSpawnPosition()
    {
        // Utg� fr�n nuvarande position
        Vector3 basePosition = transform.position + transform.forward * spawnDistance;

       /* if (randomizePosition)
        {
            // L�gg till slumpm�ssig offset i X och Z led
            float randomX = Random.Range(-randomRange, randomRange);
            float randomZ = Random.Range(-randomRange, randomRange);
            basePosition += new Vector3(randomX, 0, randomZ);
        }*/

        return basePosition;
    }

    private void CopySettings(RailCreater newRespawner)
    {
        newRespawner.objectToSpawn = this.objectToSpawn;
        newRespawner.spawnDelay = this.spawnDelay;
        newRespawner.spawnDistance = this.spawnDistance;
        //newRespawner.triggerTag = this.triggerTag;
        //newRespawner.randomizePosition = this.randomizePosition;
        //newRespawner.randomRange = this.randomRange;
    }
}
