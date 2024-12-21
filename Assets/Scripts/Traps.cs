using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public int damageAmount = 15;   // Fixed amount of damage to deal
    public float soundSpeed = 1.0f; // Default sound speed (normal)
    public HealthBar healthBar;     // Reference to the HealthBar script

    private HashSet<GameObject> damagedObjects = new HashSet<GameObject>(); // Track objects already damaged

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null && !damagedObjects.Contains(collision.gameObject))
        {
            Debug.Log("Player stepped on the trap!"); // Debug log
            playerHealth.TakeDamage(damageAmount); // Reduce player's health by a fixed amount

            // Update the health bar when damage is taken
            if (healthBar != null)
            {
                healthBar.UpdateHealthBar();  // Update the health bar after damage
            }

            // Mark the object as damaged
            damagedObjects.Add(collision.gameObject);

            // Play the trap sound
            AudioSource trapSound = GetComponent<AudioSource>();
            if (trapSound != null)
            {
                trapSound.pitch = soundSpeed; // Adjust the sound speed
                trapSound.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource component is missing!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Remove the object from the damaged list when it leaves the trap
        if (damagedObjects.Contains(collision.gameObject))
        {
            damagedObjects.Remove(collision.gameObject);
        }
    }
}