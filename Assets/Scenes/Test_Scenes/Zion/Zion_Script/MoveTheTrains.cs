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
        public bool isPlayerTrain;
        [HideInInspector] public float timer = 0f;
        [HideInInspector] public bool hasStopped = false; 

        
    }
    
    [Header("Managers")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerManager Player;

    [Header("Objects Settings")]
    [SerializeField] private List<MovingTrain> movingObjects = new List<MovingTrain>();

    [Header("Timer Settings")]
    [SerializeField] private float timeUntilStop = 5f;
    [SerializeField] private bool useTimer = true;

    [Header("Stop Settings")]
    [SerializeField] private bool deactivateOnStop = false;

    private void Start()
    {
        for (int i = 0; i < movingObjects.Count; i++)
        {
            if (movingObjects[i].isPlayerTrain)
            {
                ParentPlayerToTrain(movingObjects[i].objectToMove);
                break;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAllObjects();
        }
        for (int i = movingObjects.Count - 1; i >= 0 && !movingObjects[i].hasStopped; i--)
        {
            var obj = movingObjects[i];
            if (obj.objectToMove == null) continue;

            
            if (useTimer && !obj.hasStopped)
            {
                obj.timer += Time.deltaTime;

                if (obj.timer >= timeUntilStop)
                {
                    obj.hasStopped = true;
                    UnparentPlayer();

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

           Debug.Log("moving");
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
    public void PlayerTrainExitLevel()
    {
        gameManager.StartCoroutine(gameManager.ShowEndScreen(5.0f));

        for (int i = 0; i < movingObjects.Count; i++)
        {
            if (movingObjects[i].isPlayerTrain)
            {
                ParentPlayerToTrain(movingObjects[i].objectToMove);
                timeUntilStop = 30.0f;
                ResetAllObjects();
                break;
            }
        }

    }
    public void ParentPlayerToTrain(GameObject train)
    {
        Player.gameObject.transform.parent = train.transform;
    }
    public void UnparentPlayer()
    {
        Player.gameObject.transform.parent = null;
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
