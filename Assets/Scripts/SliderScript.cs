using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SliderScript : MonoBehaviour
{
    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;
    public void setBGMVolume(float sliderValue)
    {
        bgmMixer.SetFloat("BGMVolume", Mathf.Log10(sliderValue)*20);
    }
}
