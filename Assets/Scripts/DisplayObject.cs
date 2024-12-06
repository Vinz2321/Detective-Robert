using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayObject : MonoBehaviour
{
    public GameObject canvasPanel;          // Reference to the UI Panel
    public PlayerMovement playerMovement;   // Reference to the PlayerMovement script
    public Button returnButton;             // Reference to the Return Button
    public Transform player;                // Reference to the player object
    public GameObject highlightObject;      // Reference to the highlight GameObject
    public float interactionDistance = 5f;  // Maximum distance for interaction
    public AudioClip uiAudioClip;           // The audio clip to play when the UI is displayed

    private AudioSource audioSource;        // AudioSource for playing UI audio

    void Start()
    {
        // Ensure the highlight object is inactive at the start
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }

        // Initialize the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Prevent the audio from playing automatically
        audioSource.loop = true;         // Set looping to true to keep audio playing while the UI is active
        audioSource.clip = uiAudioClip;

        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnToGame); // Link the button click to the method
        }
        else
        {
            Debug.LogWarning("Return Button is not assigned!");
        }
    }

    void Update()
    {
        // Check the distance between the player and this object
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Enable or disable the highlight based on the distance
        if (highlightObject != null)
        {
            highlightObject.SetActive(distanceToPlayer <= interactionDistance);
        }

        // If the canvas is active, play the audio, otherwise stop it
        if (canvasPanel.activeSelf)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();  // Start playing the audio
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();  // Stop playing the audio
            }
        }
    }

    void OnMouseDown()
    {
        // Check the distance to allow interaction
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionDistance)
        {
            // Show the canvas and disable player movement
            canvasPanel.SetActive(true);
            if (playerMovement != null)
            {
                playerMovement.SetMovementEnabled(false);
            }
        }
        else
        {
            Debug.Log("Player is too far to interact with this object.");
        }
    }

    public void ReturnToGame()
    {
        // Close the canvas and re-enable player movement
        canvasPanel.SetActive(false);
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(true);
        }
    }
}
