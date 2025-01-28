using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public int Score;
    public int ArrowsShot;
    public int TimesHit;
    //public int EnemiesHit;
    public int TimesTeleported;

    public bool TimerEnabled;
    [SerializeField] private float TimerTimeElasped;
    public int EnemiesRemaining;
    
    public MoveTrainIntro TrainMover;
    [SerializeField] private Volume purpleFilter;
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
    public void OnEnemyHit()
    {
        EnemiesRemaining -= 1;
        if (EnemiesRemaining <= 0)
        {
            Debug.Log("Last enemy destroyed.");
            ReadyTrainForExit();
        }
    }
    public void ToggleTimer()
    {
        TimerEnabled = !TimerEnabled;
    }
    public void ReadyTrainForExit()
    {
        TrainMover.ResetAllObjects();
    }
    
    public IEnumerator ShowEndScreen(float delay)
    {
        Debug.Log("Started end screen delay");
        yield return new WaitForSeconds(delay);
        Debug.Log("Finished delay. Starting effect");

        FilmGrain letterEffect;
        WhiteBalance balance;
        purpleFilter.profile.TryGet(out letterEffect);
        purpleFilter.profile.TryGet(out balance);

        balance.temperature.value = 24;
        letterEffect.active = true;


        yield return null;
    }


}
