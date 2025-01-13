using UnityEngine;
using System.Collections.Generic;

public class TrainGenerator : MonoBehaviour
{

    [Header("Train Configuration")]
    [SerializeField] private GameObject locomotivePrefab; // Loket
    [SerializeField] private GameObject[] carPrefabs; // Array med alla olika vagnprefabs
    [SerializeField] private int minCars = 3; // Minimum antal 
    [SerializeField] private int maxCars = 8; // Maximum antal
    [SerializeField] private float cartDistance = 2f; // Delay between carts

    [Header("Generation Settings")]
    [SerializeField] private Vector3 startPosition = Vector3.zero;
    [SerializeField] private Vector3 trainDirection = Vector3.forward;

    private List<GameObject> currentTrain = new List<GameObject>();

    void Start()
    {
      //Checkar om alla delar finns 
        if (locomotivePrefab == null)
        {
            Debug.LogError("Inget lok är tilldelat! Lägg till ett lok i Inspector.");
            return;
        }

        if (carPrefabs == null || carPrefabs.Length == 0)
        {
            Debug.LogError("Inga vagnar är tilldelade! Lägg till vagnar i Inspector.");
            return;
        }

        // Generera tåget
        GenerateNewTrain();
    }
    public void GenerateNewTrain()
    {
        // Clear up the existed train
        //CleanupCurrentTrain();

        // Bestäm antal vagnar för detta tåg
        int numberOfCars = Random.Range(minCars, maxCars + 1);

        // Placera loket först
        Vector3 currentPosition = startPosition;
        GameObject locomotive = Instantiate(locomotivePrefab, currentPosition, Quaternion.LookRotation(trainDirection));
        currentTrain.Add(locomotive);

        // Uppdatera position för första vagnen
        currentPosition += trainDirection * cartDistance;

        // Generera och placera vagnar
        for (int i = 0; i < numberOfCars; i++)
        {
            // Välj en slumpmässig vagn från prefabs
            int randomCarIndex = Random.Range(0, carPrefabs.Length);
            GameObject selectedCarPrefab = carPrefabs[randomCarIndex];

            // Skapa vagnen och sätt position/rotation
            GameObject car = Instantiate(selectedCarPrefab, currentPosition, Quaternion.LookRotation(trainDirection));
            currentTrain.Add(car);

            // Uppdatera position för nästa vagn
            currentPosition += trainDirection * cartDistance;
        }

        ConnectTrainCars();
    }

    private void ConnectTrainCars()
    {
        for (int i = 0; i < currentTrain.Count - 1; i++)
        {
            
        }
    }

    private void CleanupCurrentTrain()
    {
        foreach (GameObject car in currentTrain)
        {
            Destroy(car);
        }
        currentTrain.Clear();
    }

}
