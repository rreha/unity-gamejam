using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider volumeSlider; // Slider bileşeni
    public AudioSource audioSource; // Audio Source bileşeni

    void Start()
    {
        // Slider'ı varsayılan ses seviyesine ayarla
        volumeSlider.value = audioSource.volume;

        // Slider'ın dinleyicisini tanımla
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Slider hareket ettikçe çağrılan fonksiyon
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
