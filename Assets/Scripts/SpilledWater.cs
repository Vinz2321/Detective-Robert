using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpilledWater : MonoBehaviour
{
    public AudioClip spillSound;  // Reference to the AudioClip to play
    private AudioSource audioSource;  // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
        }
    }

    // Called when another collider enters the trigger collider attached to this GameObject
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the AudioClip is assigned and the AudioSource is available
        if (spillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spillSound);  // Play the audio clip
            Debug.Log("Spilled water sound triggered!");
        }
        else
        {
            Debug.LogWarning("Spill sound or AudioSource is missing.");
        }
    }
}
