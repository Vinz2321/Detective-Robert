using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioPlayer : MonoBehaviour
{
    public Button button;      // Reference to the Button
    public AudioClip audioClip; // Reference to the AudioClip
    public float volume = 1.0f; // Volume of the sound (0.0 to 1.0)

    void Start()
    {
        // Ensure the button and audio clip are assigned
        if (button == null || audioClip == null)
        {
            Debug.LogError("Button or AudioClip is not assigned in the inspector!");
            return;
        }

        // Add a listener to the button's onClick event
        button.onClick.AddListener(PlaySound);
    }

    // Function to play the audio clip
    void PlaySound()
    {
        // Play the audio clip at the position of the button
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
    }

    // Optionally, remove the listener when the object is destroyed
    void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(PlaySound);
    }
}