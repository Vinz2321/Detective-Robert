using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;      // Maximum health
    public int currentHealth;       // Current health
    public AudioClip deathSound;    // Reference to the AudioClip for the death sound

    public Vector3 soundPositionOffset = Vector3.zero; // Offset for where the sound plays (relative to player position)

    void Start()
    {
        // Test: Play the death sound at the start to ensure it works
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
        else
        {
            Debug.LogWarning("Death sound AudioClip is not assigned!");
        }

        currentHealth = maxHealth; // Set initial health to max
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent health from going out of bounds

        Debug.Log("Current Health: " + currentHealth); // Debug log to track health

        // Check if health reaches 0
        if (currentHealth == 0)
        {
            PlayDeathSound();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    private void PlayDeathSound()
    {
        if (deathSound != null) // Check if the AudioClip is assigned
        {
            // Play the sound at the player's position + offset
            Vector3 playPosition = transform.position + soundPositionOffset;
            AudioSource.PlayClipAtPoint(deathSound, playPosition);

            Debug.Log("Player health reached 0. Playing death sound.");
        }
        else
        {
            Debug.LogWarning("Death sound AudioClip is not assigned!");
        }
    }
}
