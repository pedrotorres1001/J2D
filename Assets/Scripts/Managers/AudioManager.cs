using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source ---------")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("--------- Audio Clip ---------")]
    public AudioClip background;
    public AudioClip enemyDamage;
    public AudioClip enemyDeath;
    public AudioClip hitRock;
    public AudioClip swing;
    public AudioClip BossHitRock;
    public AudioClip fire;

    private void Start()
    {
        musicSource.clip = background;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void loadSettings()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        SFXSource.volume = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void GetSoundVolume(string volume)
    {
        if (volume == "Music")
        {
            PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
        }
        else if (volume == "SFX")
        {
            PlayerPrefs.SetFloat("SFXVolume", SFXSource.volume);
        }
    }

    public void ChangeVolume(float volume, string type)
    {
        if (type == "Music")
        {
            musicSource.volume = volume;
        }
        else if (type == "SFX")
        {
            SFXSource.volume = volume;
        }
    }
}