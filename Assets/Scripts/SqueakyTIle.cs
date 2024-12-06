using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SqueakyTile : MonoBehaviour
{
    public AudioClip soundClip;     // The 2D audio clip to play when the player steps on it
    private AudioSource audioSource; // The AudioSource component to play the sound

    void Start()
    {
        // Ensure the AudioSource is added to the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;   // Assign the audio clip to the AudioSource
        audioSource.playOnAwake = false; // Don't play the sound immediately on start
        audioSource.spatialBlend = 0f;   // Set spatial blend to 0 for 2D audio (no 3D effect)
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player (tagged as "Player") enters the trigger
        if (other.CompareTag("Player"))
        {
            // Play the sound
            if (audioSource != null && soundClip != null)
            {
                audioSource.Play();
            }
        }
    }
}
