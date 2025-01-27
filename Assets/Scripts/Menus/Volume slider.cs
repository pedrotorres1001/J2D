using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volumeslider : MonoBehaviour
{
    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume / 100);
        PlayerPrefs.SetFloat("SFXVolume", volume / 100);
        PlayerPrefs.SetFloat("MasterVolume", 1);
        PlayerPrefs.Save();
        AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //audioManager.loadSettings();
    }
}
