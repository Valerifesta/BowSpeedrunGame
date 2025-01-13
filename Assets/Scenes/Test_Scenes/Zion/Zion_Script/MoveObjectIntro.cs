using UnityEngine;
using System.Collections.Generic;

public class MoveObjectIntro : MonoBehaviour
{
    [System.Serializable]
    public class MovingObject
    {
        public GameObject objectToMove;
        public Vector3 direction = Vector3.forward;
        public float speed = 5f;
        public bool shouldRotate = false;
        public float rotationSpeed = 50f;

        [HideInInspector] public float timer = 0f; // Timer för varje objekt
    }


    [Header("Objects Settings")]
    [SerializeField] private List<MovingObject> movingObjects = new List<MovingObject>();

    [Header("Timer Settings")]
    [SerializeField] private float timeUntilDeactivation = 5f;
    [SerializeField] private bool useTimer = true; 


    private void Update()
    {
        for (int i = movingObjects.Count - 1; i >= 0; i--)
        {
            var obj = movingObjects[i];
            if (obj.objectToMove == null) continue;

            // Uppdatera timer om den är aktiverad
            if (useTimer)
            {
                obj.timer += Time.deltaTime;
                if (obj.timer >= timeUntilDeactivation)
                {
                    obj.objectToMove.SetActive(false);
                    continue; // Hoppa över resten av uppdateringen för detta objekt
                }
            }

            // Flytta objektet
            Vector3 movement = obj.direction.normalized * obj.speed * Time.deltaTime;
            obj.objectToMove.transform.position += movement;

            // Rotera objektet om det är aktiverat
            if (obj.shouldRotate)
            {
                obj.objectToMove.transform.Rotate(Vector3.up, obj.rotationSpeed * Time.deltaTime);
            }
        }
    }

   
}
