using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public bool hasIntro;

    public AudioClip musicIntro;
    public AudioClip musicLoop;

    private AudioSource source;

    void Awake(){
        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.clip = musicLoop;
    }

    void Start(){
        PlayMusic();
    }

    public void PlayMusic(){
        if(hasIntro || musicIntro == null)
            PlayLoopWithIntro();
        else
            PlayOnlyLoop();
    }

    private void PlayLoopWithIntro(){
        source.PlayOneShot(musicIntro);
        source.PlayScheduled(AudioSettings.dspTime + musicIntro.length);
    }

    private void PlayOnlyLoop(){
        source.Play();
    }

}
