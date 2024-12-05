using System.Collections;
using UnityEngine;

public class Stage4_Guard2 : MonoBehaviour
{
    public GameObject player;                // Reference to the player GameObject
    public GameObject guardSprite;           // 2D sprite to display next to the guard
    public float detectionRadius = 5f;       // Radius for detecting the player
    public float chaseSpeed = 4f;            // Speed of the guard when chasing the player
    public float sprintSpeed = 6f;           // Sprint speed of the guard when chasing while player boosts
    public float returnSpeed = 3f;           // Speed of the guard when returning to its position

    private bool isChasing = false;          // Whether the guard is currently chasing the player
    private bool playerTriggered = false;    // Tracks if the player triggered the guard
    private Vector3 originalPosition;        // The guard's initial position
    private PlayerMovement playerMovement;   // Reference to the PlayerMovement script

    private LineRenderer lineRenderer;       // LineRenderer to draw the detection radius
    public int segments = 50;                // Number of segments for the circle

    void Start()
    {
        // Save the guard's original position
        originalPosition = transform.position;

        // Get the PlayerMovement script from the player GameObject
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        // Initialize LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;       // Thickness of the circle
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true;     // Use world space to draw around guard
        lineRenderer.loop = true;              // Close the circle
        lineRenderer.positionCount = segments + 1; // Number of points for the circle
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        DrawCircle();

        // Always make the guard sprite visible
        if (guardSprite != null)
        {
            guardSprite.SetActive(true);
        }
    }

    void Update()
    {
        if (player == null || playerMovement == null)
            return;

        // Only chase if the player has triggered the NPC
        if (playerTriggered)
        {
            // Check the distance between the guard and the player
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // Check if player is within the detection range and boosting
            if (distanceToPlayer <= detectionRadius && playerMovement.IsBoosting())
            {
                // Only start chasing if not already chasing
                if (!isChasing)
                {
                    isChasing = true;
                    Debug.Log("Guard started chasing because the player is boosting.");
                }
                ChasePlayer(true); // Chase at sprint speed
            }
            else if (isChasing && distanceToPlayer > detectionRadius)
            {
                // Stop chasing if the player is out of range
                Debug.Log("Guard stopped chasing because the player left the detection radius.");
                StartCoroutine(ReturnToPosition());
            }
            else if (!playerMovement.IsBoosting())
            {
                // Continue chasing at normal speed when not boosting
                if (isChasing)
                {
                    Debug.Log("Guard continues chasing at normal speed.");
                    ChasePlayer(false); // Chase at normal speed
                }
            }
        }
    }

    // This method is called from the Stage4_NPCTrigger to set the trigger state
    public void SetPlayerTriggered(bool triggered)
    {
        playerTriggered = triggered;
    }

    void ChasePlayer(bool sprinting)
    {
        // Move towards the player's position
        if (player != null)
        {
            float currentSpeed = sprinting ? sprintSpeed : chaseSpeed;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);
        }
    }

    IEnumerator ReturnToPosition()
    {
        isChasing = false;

        // Gradually move back to the original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = originalPosition; // Snap to the original position
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
