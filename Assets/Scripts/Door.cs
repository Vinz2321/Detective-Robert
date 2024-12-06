using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Item playerItem; // Reference to the player's Item script
    private bool isOpen = false; // Track if the door is already open
    public float openSpeed = 2f; // Speed of door opening animation
    public Vector3 openPositionOffset = new Vector3(0, 3f, 0); // Position offset for the open state
    private Vector3 closedPosition; // Initial position of the door
    private Vector3 openPosition; // Target position for the open state

    public string requiredKeyID; // The ID of the key required to open this door
    public AudioClip errorSound; // Sound when the wrong key or no key is used
    public AudioClip doorOpenSound; // Sound when the door opens
    private AudioSource audioSource;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openPositionOffset;

        playerItem = FindObjectOfType<Item>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            string collectedKeyID = playerItem.GetCollectedKeyID();

            if (collectedKeyID == requiredKeyID)
            {
                StartCoroutine(OpenDoor());
            }
            else
            {
                PlayErrorSound();
                Debug.Log(collectedKeyID == ""
                    ? "You need a key to open this door!"
                    : $"The collected key ({collectedKeyID}) doesn't match this door!");
            }
        }
    }

    private System.Collections.IEnumerator OpenDoor()
    {
        Debug.Log("Door opening...");
        isOpen = true;

        if (doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(closedPosition, openPosition, elapsedTime);
            elapsedTime += Time.deltaTime * openSpeed;
            yield return null;
        }

        transform.position = openPosition;
        Debug.Log("Door opened!");
    }

    private void PlayErrorSound()
    {
        if (errorSound != null)
        {
            audioSource.PlayOneShot(errorSound);
        }
    }
}
