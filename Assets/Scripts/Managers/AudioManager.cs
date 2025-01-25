using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixerGroup mixerGroup;
    public Sound[] sounds;

    public Sound[] music;
    [SerializeField] private GameObject musicSource;

    public float masterVolume = 1;
    public float musicVolume = 1;
    public float SFXVolume = 1;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }

        foreach (Sound m in music)
        {
            m.source = musicSource.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.loop = m.loop;
            m.source.volume = 0;

            m.source.outputAudioMixerGroup = mixerGroup;

            if (m.name == "track1")
            {
                StartCoroutine(TurnUpTrack(m));
            }
        }

        
    }

    public void loadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * SFXVolume * masterVolume; ;
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void Play(AudioSource audioSource, string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        audioSource.clip = s.clip;
        audioSource.volume = s.volume * SFXVolume * masterVolume; 
        audioSource.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        audioSource.loop = s.loop;

        audioSource.Play();
    }

    public void PlayMusic(string trackName)
    {
        Sound m = Array.Find(music, item => item.name == trackName);
        if (m == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        Sound playing = null;
        foreach (Sound t in music)
        {
            if (t.source.volume > 0)
            {
                playing = t;
            }
        }

        m.source.pitch = 1;
        m.source.loop = true;

        m.source.Play();
        m.source.pitch = 1;
        m.source.loop = true;

        if (playing != null)
            StartCoroutine(TurnDownTrack(playing));
        StartCoroutine(TurnUpTrack(m));
        
    }

    IEnumerator TurnDownTrack(Sound nowPlaying)
    {
        float percentage = 0;
        while (nowPlaying.source.volume > 0)
        {
            nowPlaying.source.volume = Mathf.Lerp(nowPlaying.volume * musicVolume * masterVolume, 0, percentage);
            percentage += Time.deltaTime / 2;
            yield return null;
        }

        nowPlaying.source.Pause();
    }

    IEnumerator TurnUpTrack(Sound nowPlaying)
    {
        float percentage = 0;
        nowPlaying.source.Play();
        while (nowPlaying.source.volume < nowPlaying.volume * musicVolume * masterVolume)
        {
            nowPlaying.source.volume = Mathf.Lerp(0, nowPlaying.volume, percentage);
            percentage += Time.deltaTime / 2;
            yield return null;
        }
    }
}