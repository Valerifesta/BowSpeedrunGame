using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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
    public bool FinishedLevel;
    
    public MoveTrainIntro TrainMover;
    [SerializeField] private Volume purpleFilter;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private DialoguePlayer _dialogue;
    [SerializeField] private Button[] _endUiButtons;
    [SerializeField] private GameObject[] _pauseMenuObjs;

    [SerializeField] private PlayerManager player;
    
    public bool TutorialActive;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewEnemyBehaviour[] enemies = FindObjectsByType<NewEnemyBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        EnemiesRemaining = enemies.Length;
        if (!TutorialActive)
        {
            Camera.main.GetComponent<CameraBehaviour>().LoadLastSceneRotation();
        }
        player = FindFirstObjectByType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (TimerEnabled)
        {
            TimerTimeElasped += 1.0f * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !FinishedLevel)
        {
            ToggleMenu();
        }
    }
    public void ToggleMenu()
    {
        player.TogglePlayerInputs();
        foreach (GameObject obj in _pauseMenuObjs)
        {
            obj.SetActive(!obj.activeSelf);
        }

    }
    //public void Toggle
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
        if (TutorialActive)
        {
            TrainMover.TutorialShowPlayerTrain();
            _dialogue.ReadNextDoc();
        }
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
        StartCoroutine(ShowEndUI());

        yield return null;
    }
    public IEnumerator ShowEndUI()
    {
        Debug.Log("Started showing end score");
        string[] Headers = new string[] { "Time: ", "Times Teleported: ", "Times Hit: ", "Arrows Shot: "};
        float[] stats = new float[] { TimerTimeElasped, TimesTeleported, TimesHit, ArrowsShot };
        for (int i = 0; i < Headers.Length; i++)
        {
            winText.text += (Headers[i] + stats[i] + "\n");
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }
        Debug.Log("stopped showing end score");

        foreach (Button button in _endUiButtons)
        {
            button.gameObject.SetActive(true);
        }
        player.TogglePlayerInputs();

        yield return null;
    }


}
