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
            Debug.LogError("Inget lok �r tilldelat! L�gg till ett lok i Inspector.");
            return;
        }

        if (carPrefabs == null || carPrefabs.Length == 0)
        {
            Debug.LogError("Inga vagnar �r tilldelade! L�gg till vagnar i Inspector.");
            return;
        }

        // Generera t�get
        GenerateNewTrain();
    }
    public void GenerateNewTrain()
    {
        // Clear up the existed train
        //CleanupCurrentTrain();

        // Best�m antal vagnar f�r detta t�g
        int numberOfCars = Random.Range(minCars, maxCars + 1);

        // Placera loket f�rst
        Vector3 currentPosition = startPosition;
        GameObject locomotive = Instantiate(locomotivePrefab, currentPosition, Quaternion.LookRotation(trainDirection));
        currentTrain.Add(locomotive);

        // Uppdatera position f�r f�rsta vagnen
        currentPosition += trainDirection * cartDistance;

        // Generera och placera vagnar
        for (int i = 0; i < numberOfCars; i++)
        {
            // V�lj en slumpm�ssig vagn fr�n prefabs
            int randomCarIndex = Random.Range(0, carPrefabs.Length);
            GameObject selectedCarPrefab = carPrefabs[randomCarIndex];

            // Skapa vagnen och s�tt position/rotation
            GameObject car = Instantiate(selectedCarPrefab, currentPosition, Quaternion.LookRotation(trainDirection));
            currentTrain.Add(car);

            // Uppdatera position f�r n�sta vagn
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
