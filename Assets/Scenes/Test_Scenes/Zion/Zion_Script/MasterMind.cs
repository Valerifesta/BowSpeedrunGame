using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterMind : MonoBehaviour
{
    [SerializeField] public bool IntroSceneOn; // intro
    [SerializeField] public bool PlayModeSceneOn; // Playmode
    [SerializeField] public bool GameOverSceneOn; // Game Over
    [SerializeField] public bool MenySceneOn;  // Meny
    [SerializeField] public bool WinningSceneOn; // Winning
    [SerializeField] private MenuManager Menu;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



        /*
        IntroSceneOn = true;
        PlayModeSceneOn = false;

        Menu = FindAnyObjectByType<MenuManager>();
        IntroSceneOn = false;
        PlayModeSceneOn = true;

        Menu = FindAnyObjectByType<MenuManager>();
        IntroSceneOn = false;
        PlayModeSceneOn = true;

        Menu = FindAnyObjectByType<MenuManager>();
        IntroSceneOn = false;
        PlayModeSceneOn = true;

        Menu = FindAnyObjectByType<MenuManager>();
        IntroSceneOn = false;
        PlayModeSceneOn = true;

        Menu = FindAnyObjectByType<MenuManager>();
        IntroSceneOn = false;
        PlayModeSceneOn = true;

        Menu = FindAnyObjectByType<MenuManager>();
        IntroSceneOn = false;
        PlayModeSceneOn = true;

        Menu = FindAnyObjectByType<MenuManager>();
        IntroSceneOn = false;
        PlayModeSceneOn = true;

        GameOverSceneOn = false;
        WinningSceneOn = false;*/
        //OnCombat();
    }

    private void TestingIf()
    {
        IntroSceneOn = false;
        PlayModeSceneOn = true;
        GameOverSceneOn = false;
        WinningSceneOn = false;
    }

    // Update is called once per frame
    void Update()
    {

    


       /* if (Input.GetKeyDown(KeyCode.Q))
        {
            TestingIf();
        }*/

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameOverSceneOn = true;
            WinningSceneOn = false;
            IntroSceneOn = false;
            PlayModeSceneOn = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            
        }
    }

    public void OnWin()
    {
        WinningSceneOn = true;
        GameOverSceneOn = false;
        IntroSceneOn = false;
        PlayModeSceneOn = false;
    }

    public void OnCombat()
    {
        IntroSceneOn = false;
        PlayModeSceneOn = true;
        GameOverSceneOn = false;
        WinningSceneOn = false;
    }
}
