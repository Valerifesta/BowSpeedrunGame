using UnityEngine;
using System.Collections;

public class RailCreater : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectToSpawn; // Prefab att spawna
    [SerializeField] private float spawnDelay = 2f; // Sekunder innan ny spawn
    [SerializeField] private float spawnDistance = 10f; // Hur långt fram objektet spawnas
    [SerializeField] private Collider col;
    //[SerializeField] private string triggerTag = "Player"; // Tagg för objekt som triggar respawn

    //[Header("Optional Settings")]
    // [SerializeField] private bool randomizePosition = false; // Om position ska vara slumpmässig
    // [SerializeField] private float randomRange = 5f; // Område för slumpmässig position

    [SerializeField] private bool isWaitingToSpawn = false;


    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        // Kolla om objektet som kolliderar har rätt tagg
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
        // Beräkna ny position
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
        // Utgå från nuvarande position
        Vector3 basePosition = transform.position + transform.forward * spawnDistance;

       /* if (randomizePosition)
        {
            // Lägg till slumpmässig offset i X och Z led
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
