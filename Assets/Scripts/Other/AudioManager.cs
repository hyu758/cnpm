using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static AudioManager instance;
    public static AudioManager Instance => instance;
    
    [Header("Audio Source")]
    [SerializeField] AudioSource BGMusic;
    [SerializeField] AudioSource SFX;
    
    [Header("Audio Properties")]
    public AudioClip Background;
    public AudioClip Death;
    public AudioClip PlaceBomb;
    public AudioClip Pickup;
    public AudioClip Health;
    public AudioClip SpeedUp;
    public AudioClip Explosion;
    public AudioClip Excalibur;
    public AudioClip DarkExcalibur;
    public AudioClip Win;
    
    public AudioClip ClickUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        BGMusic.clip = Background;
        BGMusic.loop = true;
        BGMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

    public void StopBGMusic()
    {
        BGMusic.Stop();
    }
}

