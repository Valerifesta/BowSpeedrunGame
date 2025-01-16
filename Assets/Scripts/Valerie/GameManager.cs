using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Score;
    public bool TimerEnabled;
    [SerializeField] private float TimerTimeElasped;
    public int EnemiesRemaining;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewEnemyBehaviour[] enemies = FindObjectsByType<NewEnemyBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        EnemiesRemaining = enemies.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerEnabled)
        {
            TimerTimeElasped += 1.0f * Time.deltaTime;
        }
    }
    public void AddScore(int pointsToAdd)
    {
        Score += pointsToAdd;
    }
    public void ToggleTimer()
    {
        TimerEnabled = !TimerEnabled;
    }
}
