using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInitialiser : MonoBehaviour
{
    public Slider sliderMusic;
    public Slider sliderSFX;
    // Start is called before the first frame update
    void Start()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }
}
