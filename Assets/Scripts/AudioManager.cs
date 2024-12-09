using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioSource bgmSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void playMusic()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void stopMusic()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop(); // Stop the audio
        }
    }
}

