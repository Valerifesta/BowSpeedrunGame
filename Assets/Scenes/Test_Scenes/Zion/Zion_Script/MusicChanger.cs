using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MusicChanger : MonoBehaviour
{
    [Header("Music List")]
    [SerializeField] private AudioClip[] MusicList;
    [SerializeField] private AudioClip PlayingThisClip;
    [SerializeField] private AudioSource musicSource;
    private MasterMind mm;

    [Header("Scene States")]
    [SerializeField] private bool IntroSceneMusic;
    [SerializeField] private bool PlayModeSceneOnMusic;
    [SerializeField] private bool GameOverSceneOnMusic;
    [SerializeField] private bool MenySceneOnMusic;
    [SerializeField] private bool WinningSceneOnMusic;

    private int currentMusicIndex = -1;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        mm = FindAnyObjectByType<MasterMind>();

       
        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;

        if (MusicList == null || MusicList.Length == 0)
        {
            Debug.LogError("Inga musik har tilldelats!");
            return;
        }
    }

   /* public void musicChecking()
    {
        if (mm.PlayModeSceneOn == true)
        {
            IntroSceneMusic = false;
            PlayModeSceneOnMusic = true;// <-- 
            GameOverSceneOnMusic = false;
            MenySceneOnMusic = false;
            WinningSceneOnMusic = false;
        }
        else if (mm.IntroSceneOn == true)
        {
            IntroSceneMusic = true;// <-- 
            PlayModeSceneOnMusic = false;
            GameOverSceneOnMusic = false;
            MenySceneOnMusic = false;
            WinningSceneOnMusic = false;
        }
        else if (mm.GameOverSceneOn == true)
        {
            IntroSceneMusic = false;
            PlayModeSceneOnMusic = false;
            GameOverSceneOnMusic = true;// <-- 
            MenySceneOnMusic = false;
            WinningSceneOnMusic = false;
        }
        else if (mm.MenySceneOn == true)
        {
            IntroSceneMusic = false;
            PlayModeSceneOnMusic = false;
            GameOverSceneOnMusic = false;
            MenySceneOnMusic = true;// <-- 
            WinningSceneOnMusic = false;
        }
        else if (mm.WinningSceneOn == true)
        {
            IntroSceneMusic = false;
            PlayModeSceneOnMusic = false;
            GameOverSceneOnMusic = false;
            MenySceneOnMusic = false;
            WinningSceneOnMusic = true;// <--
        }

    }*/

    public void musicChecking()
    {
        if (mm.PlayModeSceneOn)
        {
            SetSceneState(false, true, false, false, false);
            ChangeMusicTrack(1); // PlayMode musik
        }
        else if (mm.IntroSceneOn)
        {
            SetSceneState(true, false, false, false, false);
            ChangeMusicTrack(0); // Intro musik
        }
        else if (mm.GameOverSceneOn)
        {
            SetSceneState(false, false, true, false, false);
            ChangeMusicTrack(2); // GameOver musik
        }
        else if (mm.MenySceneOn)
        {
            SetSceneState(false, false, false, true, false);
            ChangeMusicTrack(3); // Meny musik
        }
        else if (mm.WinningSceneOn)
        {
            SetSceneState(false, false, false, false, true);
            ChangeMusicTrack(4); // Winning musik
        }
    }

    private void SetSceneState(bool intro, bool playMode, bool gameOver, bool menu, bool winning)
    {
        IntroSceneMusic = intro;
        PlayModeSceneOnMusic = playMode;
        GameOverSceneOnMusic = gameOver;
        MenySceneOnMusic = menu;
        WinningSceneOnMusic = winning;
    }


    private void ChangeMusicTrack(int newIndex)
    {
        if (newIndex >= MusicList.Length) return;

        if (musicSource.clip != MusicList[newIndex])
        {
            musicSource.Stop();
            musicSource.clip = MusicList[newIndex];
            PlayingThisClip = MusicList[newIndex];
            musicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
        {
            musicChecking();

        }
    
}
