using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TeslaSFX : MonoBehaviour
{
    [Header("Tesla's start sfx")]
    [SerializeField] private AudioClip teslaStarts;
    [Header("Tesla's EngineExplosion sfx")]
    [SerializeField] private AudioClip teslaEngineExp;
    [Header("Tesla's shoots sfx")]
    [SerializeField] private AudioClip teslaBoom;
    [SerializeField] private TeslaTower tt;

    // Flags to track if sounds have already been played
    private bool hasPlayedStartSound = false;
    private bool hasPlayedExplosionSound = false;
    private bool isPlayingShootingSound = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        tt = GetComponent<TeslaTower>();
    }

    public void teslaIsOnSFX()
    {
        SoundSFXManager.instance.PlaySoundFXClip(teslaStarts, transform, 1f);
        print("This sounds work--- Tesla start");
    }

    public void teslaEngineExplosionSFX()
    {
        SoundSFXManager.instance.PlaySoundFXClip(teslaEngineExp, transform, 1f);
        print("This sounds work--- EngineExpl");
    }

    public void teslaShootingSFX()
    {
        SoundSFXManager.instance.PlaySoundFXClip(teslaBoom, transform, 1f);
        print("This sounds work--- Tesla shoots");
    }

    private void Update()
    {
        // Only play the start sound once when hasDeployed becomes true
        if (tt.hasDeployed && !hasPlayedStartSound)
        {
            teslaIsOnSFX();
            hasPlayedStartSound = true;
        }

        // Only play the explosion sound once when EngineDestroyed becomes true
        if (tt.EngineDestroyed && !hasPlayedExplosionSound)
        {
            teslaEngineExplosionSFX();
            hasPlayedExplosionSound = true;
        }

        // For shooting sound, we can either use a cooldown timer or just track state changes
        if (tt.IsAttacking && !isPlayingShootingSound)
        {
            teslaShootingSFX();
            isPlayingShootingSound = true;
        }
        else if (!tt.IsAttacking && isPlayingShootingSound)
        {
            // Reset the flag when attack stops
            isPlayingShootingSound = false;
        }
    }
}
