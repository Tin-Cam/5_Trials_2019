using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationSFX : MonoBehaviour
{
    public AudioSource source;
    public List<AudioClip> clips = new List<AudioClip>();

    public void PlaySound(){
        PlaySound(0);
    }

    public void PlaySound(int id){
        try{
            source.clip = clips[id];
        }
        catch(System.IndexOutOfRangeException){
            Debug.LogError("Sound not found. Index out of range.");
            return;
        }
        source.Play();
    }
}
