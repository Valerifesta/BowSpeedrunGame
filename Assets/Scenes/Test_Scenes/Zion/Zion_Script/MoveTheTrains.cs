using UnityEngine;
using System.Collections.Generic;

public class MoveTrainIntro : MonoBehaviour
{
    [System.Serializable]
    public class MovingTrain
    {
        public GameObject objectToMove;
        public Vector3 direction = Vector3.forward;
        public float speed = 5f;
        [HideInInspector] public float timer = 0f;
        [HideInInspector] public bool hasStopped = false; 
    }

    [Header("Objects Settings")]
    [SerializeField] private List<MovingTrain> movingObjects = new List<MovingTrain>();

    [Header("Timer Settings")]
    [SerializeField] private float timeUntilStop = 5f;
    [SerializeField] private bool useTimer = true;

    [Header("Stop Settings")]
    [SerializeField] private bool deactivateOnStop = false;

    private void Update()
    {
        for (int i = movingObjects.Count - 1; i >= 0; i--)
        {
            var obj = movingObjects[i];
            if (obj.objectToMove == null) continue;

            
            if (useTimer && !obj.hasStopped)
            {
                obj.timer += Time.deltaTime;

                if (obj.timer >= timeUntilStop)
                {
                    obj.hasStopped = true;

                    if (deactivateOnStop)
                    {
                        obj.objectToMove.SetActive(false);
                    }

                    continue;
                }
            }

            // Move only if the object not moving...  endast om det inte har stannat
            if (!obj.hasStopped)
            {
                Vector3 movement = obj.direction.normalized * obj.speed * Time.deltaTime;
                obj.objectToMove.transform.position += movement;
            }
        }
    }

    // Restart for a specific train for movement
    public void ResetObject(int index)
    {
        if (index >= 0 && index < movingObjects.Count)
        {
            movingObjects[index].timer = 0f;
            movingObjects[index].hasStopped = false;
        }
    }

    // Restart for all the train for movement
    public void ResetAllObjects()
    {
        foreach (var obj in movingObjects)
        {
            obj.timer = 0f;
            obj.hasStopped = false;
        }
    }
}
