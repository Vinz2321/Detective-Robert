using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FovAxisActors
{
    XAxis,  // Left-Right (Horizontal)
    YAxis   // Up-Down (Vertical)
}

public class Actors: MonoBehaviour
{
    public FovAxis fovAxis = FovAxis.XAxis;  // Public axis selection for the FOV cone
    public Transform pointA;  // The starting point of patrol
    public Transform pointB;  // The ending point of patrol
    public float patrolSpeed = 2f;  // Speed when patrolling
    public float waitTime = 2f;     // Time to wait at each patrol point
    public float detectionRange = 5f;  // How far the enemy can detect the player
    public float fieldOfViewAngle = 45f;  // Angle of the FOV cone
    public Vector3 fovOffset = new Vector3(0, 0, 0);  // Offset for FOV cone placement (change this in the inspector)

    private bool movingToB = true;  // Flag to track current direction
    private bool isWaiting = false; // Flag to check if enemy is waiting
    private Transform player;       // Reference to the player's transform
    private bool isGreeting = false; // Flag to indicate greeting state

    private LineRenderer lineRenderer;  // Reference to LineRenderer for FOV visualization

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 3;
            lineRenderer.loop = true;
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
                if (!isGreeting)
                {
                    
                    StartCoroutine(GreetPlayer());
                }
            }
        }

        if (isGreeting) return; // Skip movement logic while greeting

        if (!isWaiting)
        {
            if (movingToB)
            {
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

        if (lineRenderer != null)
        {
            DrawFieldOfView();
        }
    }

    private IEnumerator GreetPlayer()
{
    isGreeting = true;
    Debug.Log("Hello, Player!");  // Greet the player

    // Update FOV color to green
    if (lineRenderer != null)
    {
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        DrawFieldOfView(); // Ensure the visual is updated immediately
    }

    yield return new WaitForSeconds(2f); // Pause to greet

    isGreeting = false;

    // Restore FOV color to yellow after greeting
    if (lineRenderer != null)
    {
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        DrawFieldOfView(); // Ensure the visual is updated back
    }
}


    private void DrawFieldOfView()
{
    Vector3 fovOrigin = transform.position + fovOffset;

    // Determine boundaries based on the axis
    if (fovAxis == FovAxis.XAxis)
    {
        Vector3 leftBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.right * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.right * detectionRange;

        lineRenderer.SetPosition(0, fovOrigin);
        lineRenderer.SetPosition(1, fovOrigin + leftBoundary);
        lineRenderer.SetPosition(2, fovOrigin + rightBoundary);
    }
    else if (fovAxis == FovAxis.YAxis)
    {
        Vector3 upBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.up * detectionRange;
        Vector3 downBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.up * detectionRange;

        lineRenderer.SetPosition(0, fovOrigin);
        lineRenderer.SetPosition(1, fovOrigin + upBoundary);
        lineRenderer.SetPosition(2, fovOrigin + downBoundary);
    }
}

    private IEnumerator WaitAndSwitchDirection()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        movingToB = !movingToB;
        isWaiting = false;
    }

    private bool IsPlayerInFieldOfView()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= detectionRange)
        {
            if (fovAxis == FovAxis.XAxis)
            {
                float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer.normalized);
                if (angleToPlayer <= fieldOfViewAngle / 2)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
            else if (fovAxis == FovAxis.YAxis)
            {
                float angleToPlayer = Vector2.Angle(transform.up, directionToPlayer.normalized);
                if (angleToPlayer <= fieldOfViewAngle / 2)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}