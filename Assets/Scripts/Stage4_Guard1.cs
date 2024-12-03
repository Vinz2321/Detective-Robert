using System.Collections;
using UnityEngine;

public class Stage4_Guard1 : MonoBehaviour
{
    public GameObject player;                // Reference to the player GameObject
    public PlayerHealth playerHealth;       // Reference to the PlayerHealth script
    public float detectionRadius = 4f;      // Radius for detecting the player
    public AudioSource deadSound;           // Reference to the AudioSource for the dead sound
    private bool playerCaught = false;      // Tracks if the player has been caught
    private PlayerMovement playerMovement;  // Reference to the PlayerMovement script

    private LineRenderer lineRenderer;      // LineRenderer to draw the detection radius
    public int segments = 50;               // Number of segments for the circle

    void Start()
    {
        // Get the PlayerMovement script from the player GameObject
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        // Initialize LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;       // Thickness of the circle
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true;    // Use world space to draw around guard
        lineRenderer.loop = true;             // Close the circle
        lineRenderer.positionCount = segments + 1; // Number of points for the circle
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        DrawCircle();
    }

    void Update()
    {
        if (player == null || playerHealth == null || playerCaught || playerMovement == null)
            return;

        // Check if the player is within the detection radius
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRadius && playerMovement.IsBoosting()) // Check if player is boosting
        {
            playerCaught = true; // Mark the player as caught
            Debug.Log("Player detected by guard while boosting. Health reduced to 0.");

            // Reduce the player's health to 0
            playerHealth.TakeDamage(playerHealth.currentHealth);

            // Play dead sound
            if (deadSound != null)
            {
                deadSound.Play();
            }
        }
    }

    // Draw a circular detection radius using LineRenderer
    void DrawCircle()
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(angle) * detectionRadius + transform.position.x;
            float y = Mathf.Sin(angle) * detectionRadius + transform.position.y;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 2 * Mathf.PI / segments;
        }
    }
}
