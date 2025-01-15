using UnityEngine;

public class MasterMind : MonoBehaviour
{
    [SerializeField] public bool IntroSceneOn; // intro
    [SerializeField] public bool PlayModeSceneOn; // Playmode
    [SerializeField] public bool GameOverSceneOn; // Game Over
    [SerializeField] public bool MenySceneOn;  // Meny
    [SerializeField] public bool WinningSceneOn; // Winning


    private class IntroInfo
    {
       
    }
    private class PlayModeInfo 
    {


    }
    private class MenyInfo
    {


    }
    private class GameOverInfo
    {

    }
    private class WinningInfo
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IntroSceneOn = true;
        PlayModeSceneOn = false;
    }

    private void TestingIf()
    {
        IntroSceneOn = false;
        PlayModeSceneOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TestingIf();
        }
    }
}
