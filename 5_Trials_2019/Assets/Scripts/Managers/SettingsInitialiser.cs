using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInitialiser : MonoBehaviour
{
    public Slider sliderMusic;
    public Slider sliderSFX;

    public Options options;
    // Start is called before the first frame update
    void Start()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.35f);
        sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 1.05f);

        options.SetMusicVolume(sliderMusic.value);
        options.SetSFXVolume(sliderSFX.value);
    }
}
