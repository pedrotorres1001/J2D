using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

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
}