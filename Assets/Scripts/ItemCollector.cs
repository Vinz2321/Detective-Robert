using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Image CollectedItemImage;
    public AudioClip collectionSound; // General collection sound
    public AudioClip keyCollectionSound; // Specific sound for collecting the key
    private AudioSource audioSource; // Internal audio source
    private string collectedKeyID = ""; // Tracks the ID of the collected key

    private void Start()
    {
        CollectedItemImage.enabled = false;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);

        // Check if the object is tagged as Evidence
        if (other.CompareTag("Evidence"))
        {
            CollectItem(other.gameObject);

            Sprite itemSprite = other.GetComponent<SpriteRenderer>().sprite;
            CollectedItemImage.sprite = itemSprite;
            CollectedItemImage.enabled = true;

            // Play the general collection sound
            if (collectionSound != null)
            {
                audioSource.PlayOneShot(collectionSound);
            }
            else
            {
                Debug.LogWarning("No collection sound assigned for Evidence!");
            }
        }
        // Check if the object is tagged as KeyItem
        else if (other.CompareTag("Key"))
        {
            string keyID = other.GetComponent<Key>().keyID; // Get the key's unique ID
            CollectItem(other.gameObject);
            collectedKeyID = keyID;

            // Play the key collection sound
            if (keyCollectionSound != null)
            {
                audioSource.PlayOneShot(keyCollectionSound);
            }
            else
            {
                Debug.LogWarning("No key collection sound assigned!");
            }

            // Optionally update the UI for the key collection
            Sprite itemSprite = other.GetComponent<SpriteRenderer>().sprite;
            CollectedItemImage.sprite = itemSprite;
            CollectedItemImage.enabled = true;

            Debug.Log($"Key collected! Key ID: {keyID}");
        }
    }

    private void CollectItem(GameObject item)
    {
        Debug.Log("Item collected: " + item.name);
        Destroy(item);
    }

    public string GetCollectedKeyID()
    {
        return collectedKeyID; // Return the ID of the collected key
    }
}
