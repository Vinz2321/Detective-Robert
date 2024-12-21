using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public Slider volumeSlider;    // Reference to the UI Slider

    void Start()
    {
        // Ensure the slider value is initialized to the current volume of the audio source
        volumeSlider.value = audioSource.volume;

        // Add a listener to the slider's OnValueChanged event
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // Called when the slider value changes
    void OnVolumeChanged(float value)
    {
        audioSource.volume = value; // Change the volume of the AudioSource
    }
}
