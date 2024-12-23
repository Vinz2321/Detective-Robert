using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // Reference to SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    // Color value to set in the Inspector
    [SerializeField]
    private Color colorToTurnTo = Color.white;

    // Use this for initialization
    private void Start()
    {
        // Assign SpriteRenderer component to spriteRenderer variable
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the SpriteRenderer component exists
        if (spriteRenderer != null)
        {
            // Change the sprite color to the selected color
            spriteRenderer.color = colorToTurnTo;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject!");
        }
    }
}