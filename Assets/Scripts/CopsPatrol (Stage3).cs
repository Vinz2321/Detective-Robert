using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FovAxis
{
    XAxis,  // Left-Right (Horizontal)
    YAxis   // Up-Down (Vertical)
}

public class CopsPatrol : MonoBehaviour
{
    public FovAxis fovAxis = FovAxis.XAxis;  // Public axis selection for the FOV cone
    public Transform pointA;  // The starting point of patrol
    public Transform pointB;  // The ending point of patrol
    public float patrolSpeed = 2f;  // Speed when patrolling
    public float chaseSpeed = 4f;   // Speed when chasing
    public float waitTime = 2f;     // Time to wait at each patrol point
    public float detectionRange = 5f;  // How far the enemy can detect the player
    public float fieldOfViewAngle = 45f;  // Angle of the FOV cone
    public float lostSightTime = 3f;  // Time to wait before stopping the chase
    public Vector3 fovOffset = new Vector3(0, 0, 0);  // Offset for FOV cone placement (change this in the inspector)

    private bool movingToB = true;  // Flag to track current direction
    private bool isWaiting = false; // Flag to check if enemy is waiting
    private Transform player;       // Reference to the player's transform
    private bool isChasing = false; // Flag to check if enemy is chasing
    private float lostSightTimer;   // Timer to track when the enemy last saw the player

    private LineRenderer lineRenderer;  // Reference to LineRenderer for FOV visualization

    void Start()
    {
        // Find the player in the scene (assumes the player has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 3;  // FOV is a cone, so we need three points (start, left, and right)
            lineRenderer.loop = true;  // To close the cone visually
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
        }
    }

    void Update()
    {
        if (player != null)
        {
            if (IsPlayerInFieldOfView())
            {
                // Start or continue chasing the player if detected
                isChasing = true;
                lostSightTimer = lostSightTime; // Reset the lost sight timer
            }
            else
            {
                // Start countdown if player is not in view
                if (isChasing)
                {
                    lostSightTimer -= Time.deltaTime;
                    if (lostSightTimer <= 0)
                    {
                        isChasing = false; // Stop chasing when timer runs out
                    }
                }
            }
        }

        if (isChasing)
        {
            // Rotate sprite to face the player
            Vector2 directionToPlayer = player.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
        else if (!isWaiting)
        {
            // Patrol behavior when not chasing
            if (movingToB)
            {
                // Rotate sprite to face pointB
                Vector2 directionToB = pointB.position - transform.position;
                float angleToB = Mathf.Atan2(directionToB.y, directionToB.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToB));

                transform.position = Vector2.MoveTowards(transform.position, pointB.position, patrolSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
                {
                    StartCoroutine(WaitAndSwitchDirection());
                }
            }
            else
            {
                // Rotate sprite to face pointA
                Vector2 directionToA = pointA.position - transform.position;
                float angleToA = Mathf.Atan2(directionToA.y, directionToA.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToA));

                transform.position = Vector2.MoveTowards(transform.position, pointA.position, patrolSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
                {
                    StartCoroutine(WaitAndSwitchDirection());
                }
            }
        }

        // Visualize the FOV cone
        if (lineRenderer != null)
        {
            DrawFieldOfView();
        }
    }

    private void DrawFieldOfView()
    {
        // Get the position from which to draw the FOV (including the offset)
        Vector3 fovOrigin = transform.position + fovOffset;

        // Check the selected axis to draw the FOV cone
        if (fovAxis == FovAxis.XAxis)
        {
            // Calculate the left and right boundary of the FOV cone (Horizontal)
            Vector3 leftBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.right * detectionRange;
            Vector3 rightBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.right * detectionRange;

            // Set the positions of the LineRenderer to create the FOV cone (Horizontal)
            lineRenderer.SetPosition(0, fovOrigin);  // Starting point (with offset)
            lineRenderer.SetPosition(1, fovOrigin + leftBoundary);  // Left boundary
            lineRenderer.SetPosition(2, fovOrigin + rightBoundary);  // Right boundary
        }
        else if (fovAxis == FovAxis.YAxis)
        {
            // Calculate the up and down boundary of the FOV cone (Vertical)
            Vector3 upBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.up * detectionRange;
            Vector3 downBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.up * detectionRange;

            // Set the positions of the LineRenderer to create the FOV cone (Vertical)
            lineRenderer.SetPosition(0, fovOrigin);  // Starting point (with offset)
            lineRenderer.SetPosition(1, fovOrigin + upBoundary);  // Upward boundary
            lineRenderer.SetPosition(2, fovOrigin + downBoundary);  // Downward boundary
        }
    }

    private IEnumerator WaitAndSwitchDirection()
    {
        isWaiting = true;  // Set waiting flag to true
        yield return new WaitForSeconds(waitTime);  // Wait for the specified time
        movingToB = !movingToB;  // Switch direction
        isWaiting = false;  // Reset waiting flag
    }

    private bool IsPlayerInFieldOfView()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Check if the player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Check based on the selected axis
            if (fovAxis == FovAxis.XAxis)
            {
                // Calculate the angle between the enemy's "right" direction and the direction to the player
                float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer.normalized);

                // Check if the angle is within the FOV
                if (angleToPlayer <= fieldOfViewAngle / 2)
                {
                    // Optionally, perform a raycast to ensure there are no obstacles blocking view
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        return true; // Player is detected in the FOV cone
                    }
                }
            }
            else if (fovAxis == FovAxis.YAxis)
            {
                // Calculate the angle between the enemy's "up" direction and the direction to the player
                float angleToPlayer = Vector2.Angle(transform.up, directionToPlayer.normalized);

                // Check if the angle is within the FOV
                if (angleToPlayer <= fieldOfViewAngle / 2)
                {
                    // Optionally, perform a raycast to ensure there are no obstacles blocking view
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        return true; // Player is detected in the FOV cone
                    }
                }
            }
        }

        return false; // Player is not in the FOV or range
    }
}