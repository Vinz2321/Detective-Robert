using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cctv : MonoBehaviour
{
    public Color safeColor = Color.green;
    public Color alertColor = Color.red;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the CCTV object!");
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
}