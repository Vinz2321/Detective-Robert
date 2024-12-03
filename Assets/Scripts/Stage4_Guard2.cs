using UnityEngine;

public class Stage4_Guard2 : MonoBehaviour
{
    public GameObject player;                // Reference to the player GameObject
    public float detectionAngle = 60f;       // Field of view angle for detecting the player
    public float detectionRange = 6f;        // Front detection range
    public float speed = 3f;                 // Speed at which the guard chases the player
    private Vector3 originalPosition;        // Original position of the guard
    private bool isChasing = false;          // Tracks if the guard is chasing the player

    private LineRenderer lineRenderer;       // LineRenderer to draw the front detection area
    public int segments = 50;                // Number of segments for the detection arc

    void Start()
    {
        // Save the guard's original position
        originalPosition = transform.position;

        // Initialize LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;        // Thickness of the line
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true;     // Use world space for the detection arc
        lineRenderer.loop = false;             // Do not close the arc
        lineRenderer.positionCount = segments + 1; // Number of points for the arc
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;

        DrawDetectionArc();
    }

    void Update()
    {
        if (player == null)
            return;

        // Only start chasing if the player is in detection range and if the guard is activated to chase
        if (isChasing)
        {
            // Check if the player is within the front detection area
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            Debug.Log($"Distance: {distanceToPlayer}, Angle: {angleToPlayer}");

            if (distanceToPlayer <= detectionRange && angleToPlayer <= detectionAngle / 2)
            {
                // Chase the player
                Debug.Log("Chasing player");
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else
            {
                // Return to the original position if not chasing the player
                Debug.Log("Returning to original position");
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            }
        }
    }


    // Method to activate or deactivate chasing
    public void ActivateChasing(bool chase)
    {
        isChasing = chase;
    }

    // Draw the front detection arc using LineRenderer
    void DrawDetectionArc()
    {
        float angle = -detectionAngle / 2; // Start angle for the arc
        float angleStep = detectionAngle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * detectionRange;
            float y = Mathf.Cos(angle * Mathf.Deg2Rad) * detectionRange;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0) + transform.position);
            angle += angleStep;
        }
    }
}
