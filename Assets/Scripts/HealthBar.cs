using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the player's health script
    public Image fillImage;           // Reference to the Fill image

    void Update()
    {
        // Calculate the health percentage
        float healthPercentage = (float)playerHealth.currentHealth / playerHealth.maxHealth;

        // Update the fill amount of the image
        fillImage.fillAmount = healthPercentage; // Use fillAmount for smooth horizontal adjustment

        Debug.Log("Health Percentage: " + healthPercentage); // Debug log
    }
}
