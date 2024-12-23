using UnityEngine;

public class ToggleAssetOnCollision2D : MonoBehaviour
{
    public Sprite newSprite; // New sprite to display
    public GameObject newPrefab; // New prefab to instantiate (optional)

    private Sprite originalSprite; // Store the original sprite
    private GameObject originalPrefab; // Store the original prefab reference

    private void Start()
    {
        // Save the original sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite;
        }

        // Save the original prefab reference (if needed)
        originalPrefab = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the triggering object (optional)
        if (other.gameObject.CompareTag("TriggerObject")) // Replace "TriggerObject" with your tag
        {
            // Replace with a new prefab
            if (newPrefab != null)
            {
                Instantiate(newPrefab, transform.position, transform.rotation);
                gameObject.SetActive(false); // Disable the original object
                return; // Exit early since the prefab is being replaced
            }

            // Change the sprite
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider is the same triggering object (optional)
        if (other.gameObject.CompareTag("TriggerObject"))
        {
            // Revert the prefab
            if (newPrefab != null)
            {
                gameObject.SetActive(true); // Re-enable the original object
                Destroy(GameObject.Find(newPrefab.name)); // Destroy the new prefab instance
                return;
            }

            // Revert the sprite
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && originalSprite != null)
            {
                spriteRenderer.sprite = originalSprite;
            }
        }
    }
}
