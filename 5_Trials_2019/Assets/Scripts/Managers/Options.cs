using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Options : MonoBehaviour
{

    public AudioMixer audioMixer;


    public void SetMusicVolume(float value){
        PlayerPrefs.SetFloat("MusicVolume", value);
        
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("volumeMusic", volume);
    }

    public void SetSFXVolume(float value){
        PlayerPrefs.SetFloat("SFXVolume", value);

        float volume = Mathf.Log10(value) * 20;     
        audioMixer.SetFloat("volumeSFX", volume);
        
    }

    public void SetFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }
}
