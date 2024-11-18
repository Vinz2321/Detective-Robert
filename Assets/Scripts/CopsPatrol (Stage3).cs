using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CopsPatrol : MonoBehaviour
{
    public Transform pointA;  // The starting point of patrol
    public Transform pointB;  // The ending point of patrol
    public float patrolSpeed = 2f;  // Speed when patrolling
    public float chaseSpeed = 4f;   // Speed when chasing
    public float waitTime = 2f;     // Time to wait at each patrol point
    public float detectionRange = 5f;  // How far the enemy can detect the player
    public float fieldOfViewAngle = 45f;  // Angle of the FOV cone
    public float lostSightTime = 3f;  // Time to wait before stopping the chase

    private bool movingToB = true;  // Flag to track current direction
    private bool isWaiting = false; // Flag to check if enemy is waiting
    private Transform player;       // Reference to the player's transform
    private bool isChasing = false; // Flag to check if enemy is chasing
    private float lostSightTimer;   // Timer to track when the enemy last saw the player

    void Start()
    {
        // Find the player in the scene (assumes the player has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
        else if (!isWaiting)
        {
            // Patrol behavior when not chasing
            if (movingToB)
            {
                transform.position = Vector2.MoveTowards(transform.position, pointB.position, patrolSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
                {
                    StartCoroutine(WaitAndSwitchDirection());
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, pointA.position, patrolSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
                {
                    StartCoroutine(WaitAndSwitchDirection());
                }
            }
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
            // Calculate the angle between the enemy's forward direction and the direction to the player
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

        return false; // Player is not in the FOV or range
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a circle to visualize the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw the FOV cone
        Vector3 leftBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.right * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.right * detectionRange;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}