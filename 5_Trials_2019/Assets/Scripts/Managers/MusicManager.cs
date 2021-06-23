using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public bool hasIntro;

    public AudioClip musicIntro;
    public AudioClip musicLoop;
    public AudioMixerGroup mixerGroup;
    public bool playOnStart = false;

    private AudioSource source;

    void Awake(){
        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.clip = musicLoop;
        source.outputAudioMixerGroup = mixerGroup;
    }

    void Start(){
        if(playOnStart)
            PlayMusic();
    }

    public void PlayMusic(){
        if(hasIntro || musicIntro != null)
            PlayLoopWithIntro();
        else
            PlayOnlyLoop();
    }

    public void StopMusic(){
        source.Stop();
    }

    private void PlayLoopWithIntro(){
        source.PlayOneShot(musicIntro);
        source.PlayScheduled(AudioSettings.dspTime + musicIntro.length);
    }

    private void PlayOnlyLoop(){
        source.Play();
    }

}
