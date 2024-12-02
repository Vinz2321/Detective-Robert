using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_Guard1 : MonoBehaviour
{
    public GameObject player;                // Reference to the player GameObject
    public PlayerHealth playerHealth;       // Reference to the PlayerHealth script
    public float moveSpeed = 4f;            // Guard's movement speed
    public Animator animator;               // Animator for guard animations
    public LineRenderer lineRenderer;       // LineRenderer for visualizing detection
    public float lineLength = 4f;           // Length of the detection line
    public Rigidbody2D rb;
    public AudioSource deadSound;           // Reference to the AudioSource for the dead sound

    private bool isChasingPlayer = false;   // Tracks if the guard is chasing the player
    private bool playerCaught = false;      // Tracks if the player has been caught
    private Vector2 startingPosition;       // Guard's initial position
    private bool isReturningToStart = false;

    public Vector2[] patrolPoints = new Vector2[]
    {
        new Vector2(0, 5),  // Patrol point up
        new Vector2(0, -5)  // Patrol point down
    };

    private int currentPatrolIndex = 0;
    private float patrolWaitTime = 2f;
    private float patrolTimer = 0f;

    void Start()
    {
        // Initialize line renderer
        lineRenderer.positionCount = 2;
        ChangeLineColor(Color.white, 0.1f);

        // Save starting position
        startingPosition = transform.position;

        // Adjust patrol points relative to starting position
        patrolPoints = new Vector2[]
        {
            new Vector2(startingPosition.x, startingPosition.y + 5),
            new Vector2(startingPosition.x, startingPosition.y - 5)
        };
    }

    void Update()
    {
        if (player == null || playerHealth == null)
            return;

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Check if player is within the detection line
        if (IsPlayerInDetectionLine() && !playerCaught)
        {
            isChasingPlayer = true;
            playerCaught = true; // Mark player as caught
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            transform.up = directionToPlayer;

            ChangeLineColor(Color.red, 1.0f);
            animator.SetFloat("Speed", moveSpeed);

            // Reduce player's health to 0
            playerHealth.TakeDamage(playerHealth.currentHealth);
            Debug.Log("Player detected by guard. Health reduced to 0.");

            // Play dead sound
            if (deadSound != null)
            {
                deadSound.Play();
            }
        }
        else
        {
            ChangeLineColor(Color.white, 1.0f);

            if (isChasingPlayer)
            {
                isChasingPlayer = false;
                isReturningToStart = true;
            }

            if (isReturningToStart)
            {
                // Return to starting position
                transform.position = Vector2.MoveTowards(transform.position, startingPosition, moveSpeed * Time.deltaTime);

                if ((Vector2)transform.position == startingPosition)
                {
                    isReturningToStart = false;
                    currentPatrolIndex = 0; // Reset patrol
                }

                animator.SetFloat("Speed", moveSpeed);
            }
            else
            {
                animator.SetFloat("Speed", 0); // Idle animation
            }
        }

        // Patrol logic
        if (!isChasingPlayer && !isReturningToStart)
        {
            Patrol();
        }

        // Update detection line
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + (Vector3)(transform.up * lineLength));
    }

    void Patrol()
    {
        Vector2 targetPatrolPoint = patrolPoints[currentPatrolIndex];
        Vector2 currentPosition = transform.position;
        Vector2 direction = (targetPatrolPoint - currentPosition).normalized;

        if (direction != Vector2.zero)
        {
            transform.up = direction;
        }

        transform.position = Vector2.MoveTowards(currentPosition, targetPatrolPoint, moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == targetPatrolPoint)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                patrolTimer = 0f;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }

        animator.SetFloat("Speed", moveSpeed); // Walking animation
    }

    bool IsPlayerInDetectionLine()
    {
        // Calculate the start and end points of the detection line
        Vector2 lineStart = transform.position;
        Vector2 lineEnd = lineStart + (Vector2)(transform.up * lineLength);

        // Check if the player is within a small distance from the line
        float distanceToLine = Mathf.Abs((lineEnd.x - lineStart.x) * (lineStart.y - player.transform.position.y) -
                                         (lineStart.x - player.transform.position.x) * (lineEnd.y - lineStart.y)) /
                               Vector2.Distance(lineStart, lineEnd);

        return distanceToLine < 0.5f; // Adjust threshold as needed
    }

    void ChangeLineColor(Color color, float alpha)
    {
        color.a = alpha;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
