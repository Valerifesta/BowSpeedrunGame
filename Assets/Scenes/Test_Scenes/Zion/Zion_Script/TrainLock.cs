using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainLock : MonoBehaviour
{
    private GameObject TrainLocker;
    private GameObject TheCart;

    [SerializeField] private float spawnDistance = 2f; // Avståndet bakom originalet

    public bool trainIsInclude;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrainLocker = gameObject.GetComponent<GameObject>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cart"))
        {
            trainIsInclude = true;
            TheCart = gameObject.GetComponent<GameObject>();
            
        }
    }
 

   
    private void Update()
    {
        
    }


}
