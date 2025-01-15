using UnityEngine;
using System.Collections;

public class RailCreator : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float spawnDistance = 10f;

    [Header("Direction Settings")]
    [SerializeField] private float rotationAngle = 45f; // Vinkel att rotera varje ny kopia
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Axel att rotera runt (standard är Y-axeln)

    private bool isWaitingToSpawn = false;

    private void Start()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Ingen prefab tilldelad i Object To Spawn!", this);
            enabled = false;
            return;
        }

        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("Ingen Collider hittad!", this);
            enabled = false;
            return;
        }

        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.CompareTag("Player") && !isWaitingToSpawn)
        {
            StartCoroutine(RespawnObject());
        }
    }

    private IEnumerator RespawnObject()
    {
        if (isWaitingToSpawn) yield break;

        isWaitingToSpawn = true;
        yield return new WaitForSeconds(0.1f);

        // Beräkna ny position och rotation
        Vector3 newPosition = CalculateSpawnPosition();
        Quaternion newRotation = CalculateNewRotation();

        // Skapa nytt objekt med ny rotation
        GameObject newObject = Instantiate(objectToSpawn, newPosition, newRotation);

        if (newObject != null)
        {
            RailCreator newRespawner = newObject.GetComponent<RailCreator>();
            if (newRespawner == null)
            {
                newRespawner = newObject.AddComponent<RailCreator>();
            }

            CopySettings(newRespawner);
        }

        yield return new WaitForSeconds(spawnDelay);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private Vector3 CalculateSpawnPosition()
    {
        // Använd den aktuella rotationen för att beräkna framåtriktningen
        return transform.position + transform.forward * spawnDistance;
    }

    private Quaternion CalculateNewRotation()
    {
        // Rotera runt den specificerade axeln med den angivna vinkeln
        return transform.rotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);
    }

    private void CopySettings(RailCreator newRespawner)
    {
        if (newRespawner != null)
        {
            newRespawner.objectToSpawn = this.objectToSpawn;
            newRespawner.spawnDelay = this.spawnDelay;
            newRespawner.spawnDistance = this.spawnDistance;
            newRespawner.rotationAngle = this.rotationAngle;
            newRespawner.rotationAxis = this.rotationAxis;
            newRespawner.isWaitingToSpawn = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Rita en linje som visar nästa spawnposition
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * spawnDistance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * spawnDistance, 0.5f);

        // Rita en linje som visar rotationsaxeln
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + rotationAxis);
    }
}