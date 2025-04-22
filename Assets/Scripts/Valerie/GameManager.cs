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
    public TextMeshProUGUI EnemyCountDisplay;
    public TextMeshProUGUI TimerDisplay;
    public DialoguePlayer _dialogue; //Assign in inspector
    [SerializeField] private Button[] _endUiButtons;
    [SerializeField] private GameObject[] _pauseMenuObjs;

    [SerializeField] private PlayerManager player;
    [SerializeField] private MenuManager menu;
    public bool TutorialActive;

    public bool CanToggleMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        EnemiesRemaining = enemies.Length;
        UpdateEnemyDisplay();

        if (!TutorialActive)
        {
            Camera.main.GetComponent<CameraBehaviour>().LoadLastSceneRotation();
        }
        player = FindFirstObjectByType<PlayerManager>();
        if (TutorialActive)
        {
            CanToggleMenu = false;
        }
        else
        {
            CanToggleMenu = true;
        }

        _dialogue.gameMan = this;
        if (!TutorialActive)
        {
            _dialogue.ReadNextDoc(1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (TimerEnabled)
        {
            TimerTimeElasped += 1.0f * Time.deltaTime;
            TimerDisplay.text = "Time elapsed " + Mathf.RoundToInt(TimerTimeElasped);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !FinishedLevel && CanToggleMenu)
        {
            ToggleMenu();
        }
    }
    public void ToggleMenu()
    {
        if (menu.SettingsActive)
        {
            menu.ToggleSetting();
        }
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
        UpdateEnemyDisplay();
        if (EnemiesRemaining <= 0)
        {
            Debug.Log("Last enemy destroyed.");
            ReadyTrainForExit();
            
        }
    }
    void UpdateEnemyDisplay()
    {
        EnemyCountDisplay.text = "Enemies left: " + EnemiesRemaining;
    }
    public void OnFinishLevel()
    {
        if (!FinishedLevel)
        {
            if (TutorialActive)
            {
                _dialogue.EndCurrentDocEarly();
                _dialogue.ReadNextDoc();
            }
            FinishedLevel = true;
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
