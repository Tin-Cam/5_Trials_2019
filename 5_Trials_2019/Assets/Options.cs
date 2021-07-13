using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void SetMusicVolume(float value){
        audioMixer.SetFloat("volumeMusic", value);
    }

    public void SetSFXVolume(float value){
        audioMixer.SetFloat("volumeSFX", value);
    }

    public void SetFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }
}
