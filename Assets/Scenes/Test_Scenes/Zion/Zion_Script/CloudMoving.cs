using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CloudMoving : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    public GameObject[] cloudPrefabs;  

    [Header("Spawn Settings")]
    public int cloudsPerType = 25;     
    public float spawnWidth = 50f;    
    public float heightVariation = 10f; 

    [Header("Movement Settings")]
    public float minSpeed = 3f;
    public float maxSpeed = 8f;
    public float distanceToReset = 100f;

    private List<CloudInfo> clouds = new List<CloudInfo>();
    [System.Serializable]
    private class CloudInfo
    {
        public GameObject cloudObject;
        public Vector3 originalPos;
        public float speed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cloudPrefabs == null || cloudPrefabs.Length == 0)
        {
            Debug.LogError("Inga moln-prefabs har tilldelats!");
            return;
        }

        SpawnClouds();
    }

    private void SpawnClouds()
    {
        foreach (GameObject cloudPrefab in cloudPrefabs)
        {
            for (int i = 0; i < cloudsPerType; i++)
            {
                // Slumpmässig startposition
                float randomX = Random.Range(-spawnWidth / 2, spawnWidth / 2);
                float randomY = Random.Range(-heightVariation / 2, heightVariation / 2);
                Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0);

                // Skapa molnet
                GameObject newCloud = Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
                newCloud.transform.parent = transform;

                // Slumpmässig storlek
                float randomScale = Random.Range(0.8f, 1.5f);
                newCloud.transform.localScale *= randomScale;

                // Lägg till information om molnet
                CloudInfo cloudInfo = new CloudInfo
                {
                    cloudObject = newCloud,
                    originalPos = spawnPosition,
                    speed = Random.Range(minSpeed, maxSpeed)
                };

                clouds.Add(cloudInfo);
            }
        }
    }
    private void Update()
    {
        foreach (CloudInfo cloud in clouds)
        {
            // Flytta molnet
            Vector3 movement = Vector3.right * cloud.speed * Time.deltaTime;
            cloud.cloudObject.transform.position += movement;

            // Kontrollera om molnet behöver återställas
            float distanceTraveled = Vector3.Distance(cloud.cloudObject.transform.position,
                                                    new Vector3(cloud.originalPos.x,
                                                    cloud.cloudObject.transform.position.y,
                                                    cloud.cloudObject.transform.position.z));

            if (distanceTraveled >= distanceToReset)
            {
                // Återställ positionen men behåll Y-position för variation
                Vector3 resetPos = cloud.originalPos;
                resetPos.y = cloud.cloudObject.transform.position.y;
                cloud.cloudObject.transform.position = resetPos;
            }
        }
    }
}
