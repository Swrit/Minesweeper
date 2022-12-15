using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class SoundEngine : MonoBehaviour
{
    public static SoundEngine instance;

    Dictionary<string, AudioClip> sounds;

    AudioSource audioSource;



    void Awake()
    {
        instance = this;
        audioSource = gameObject.GetComponent<AudioSource>();
        sounds = new Dictionary<string, AudioClip>()
        {
            ["explosion"] = Resources.Load<AudioClip>("Sounds/explosion") as AudioClip,
            ["victory_jingle"] = Resources.Load<AudioClip>("Sounds/victory_jingle") as AudioClip,
            ["record_jingle"] = Resources.Load<AudioClip>("Sounds/record_jingle") as AudioClip,
            ["loss_jingle"] = Resources.Load<AudioClip>("Sounds/loss_jingle") as AudioClip,
            ["flag"] = Resources.Load<AudioClip>("Sounds/flag") as AudioClip,
        };
    }

    //void Start()
    //{

    //}



    public void Play(string soundname)
    {
        if (soundname == "victory_jingle" | soundname == "record_jingle" | soundname == "loss_jingle")
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(sounds[soundname], 1f);
    }

    public void RestartBGM()
    {
        audioSource.Play();
    }


}
