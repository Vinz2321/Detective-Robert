using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public int damagePercentage = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Player stepped on the trap!"); // Debug log
            int damage = Mathf.RoundToInt(playerHealth.maxHealth * (damagePercentage / 100f));
            playerHealth.TakeDamage(damage); // Reduce player's health
        }
        else
        {
            Debug.Log("No PlayerHealth script found on the object!"); // Debug log
        }
    }

}

