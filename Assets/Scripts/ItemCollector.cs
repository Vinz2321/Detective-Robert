using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Image CollectedItemImage;
    public AudioClip collectionSound; // Add a reference to the sound clip
    private AudioSource audioSource; // Internal audio source

    private void Start()
    {
        CollectedItemImage.enabled = false;
        // Optionally, initialize the audio source if you want to play the sound from this GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);
        if (other.CompareTag("Evidence"))
        {
            collectItem(other.gameObject);

            Sprite itemSprite = other.GetComponent<SpriteRenderer>().sprite;
            CollectedItemImage.sprite = itemSprite;
            CollectedItemImage.enabled = true;

            // Play the collection sound
            if (collectionSound != null)
            {
                audioSource.PlayOneShot(collectionSound);
            }
            else
            {
                Debug.LogWarning("No collection sound assigned!");
            }
        }
    }

    private void collectItem(GameObject Item)
    {
        Debug.Log("Item collected");
        Destroy(Item);
    }
}
