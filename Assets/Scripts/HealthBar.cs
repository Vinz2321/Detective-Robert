using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the player's health script
    public Image fillImage;           // Reference to the Fill image
    public float smoothSpeed = 5f;    // Speed of the health bar transition

    private float targetFillAmount;

    void Update()
    {
        if (playerHealth != null)
        {
            // Calculate the health percentage
            float healthPercentage = (float)playerHealth.currentHealth / playerHealth.maxHealth;

            // Smoothly update the fill amount
            targetFillAmount = Mathf.Lerp(fillImage.fillAmount, healthPercentage, Time.deltaTime * smoothSpeed);
            fillImage.fillAmount = targetFillAmount;
        }
        else
        {
            Debug.LogWarning("PlayerHealth reference is missing in the HealthBar script!");
        }
    }

    // Public method to manually update the health bar (called from other scripts like Traps)
    public void UpdateHealthBar()
    {
        if (playerHealth != null)
        {
            // Calculate the health percentage and update the fill image directly
            float healthPercentage = (float)playerHealth.currentHealth / playerHealth.maxHealth;
            fillImage.fillAmount = healthPercentage;
        }
        else
        {
            Debug.LogWarning("PlayerHealth reference is missing in the UpdateHealthBar method!");
        }
    }
}
