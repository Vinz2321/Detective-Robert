using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cctv : MonoBehaviour
{
    public Color safeColor = Color.green;
    public Color alertColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public AudioClip alertSound; // Sound to play when alertColor is set

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the CCTV object!");
        }

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the initial color to safe
        spriteRenderer.color = safeColor;
    }

    // Trigger detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the object is the Player
        {
            spriteRenderer.color = alertColor;
            PlayAlertSound();
            Debug.Log("Player entered CCTV detection!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Reset color when Player leaves
        {
            spriteRenderer.color = safeColor;
            Debug.Log("Player left CCTV detection!");
        }
    }

    private void PlayAlertSound()
    {
        if (alertSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(alertSound);
        }
        else
        {
            Debug.LogWarning("Alert sound or AudioSource is missing!");
        }
    }
}
