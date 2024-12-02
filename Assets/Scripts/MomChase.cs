using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomChase : MonoBehaviour
{
    public GameObject player;
    public GameObject dog;
    public float moveSpeed = 4f;
    public Animator animator;
    public LineRenderer lineRenderer;
    public float lineLength = 4f;
    public Rigidbody2D rb;

    private float distance;
    private Vector2 movement;
    private DogScript dogScript;

    public Vector2[] patrolPoints = new Vector2[]
    {
        new Vector2(0, 5),  // (up)
        new Vector2(0, -5),  // (down)
    };

    private bool isReturningToStart = false;
    private Vector2 startingPosition;
    private int currentPatrolIndex = 0; 
    private float patrolWaitTime = 2f; 
    private float patrolTimer = 0f;    
    private bool isChasingPlayer = false; 

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // POINTS
        dogScript = dog.GetComponent<DogScript>();

        Material lineMaterial = lineRenderer.material;
        Color color = lineMaterial.color;
        color.a = 0.1f; // OPACITY
        lineMaterial.color = color;
        ChangeLineColor(Color.white, 0.1f);

    // Save Pos
    startingPosition = transform.position;

    // Set patrol points
    patrolPoints = new Vector2[]
    {
        new Vector2(startingPosition.x, startingPosition.y + 5),  // Up from start
        new Vector2(startingPosition.x, startingPosition.y - 5)  // Down from start
    };

    }

    void Update()
    {
        // Chasing logic
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector2 directionNormalized = direction.normalized;
        float dotProduct = Vector2.Dot(transform.up, directionNormalized);

        if (distance < 4 && dotProduct > 0.9)
        {
            isChasingPlayer = true;
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            transform.up = direction;
            ChangeLineColor(Color.red, 1.0f);
            animator.SetFloat("Speed", moveSpeed);
        }
        else if (dogScript.seesPlayer)
        {
            isChasingPlayer = true;
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime * 2);
            transform.up = direction;
            ChangeLineColor(Color.red, 1.0f);
            animator.SetFloat("Speed", moveSpeed);
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
            transform.position = Vector2.MoveTowards(transform.position, startingPosition, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == startingPosition)
            {
                isReturningToStart = false; // Stop 
                currentPatrolIndex = 0;     // Restart 
            }

            animator.SetFloat("Speed", moveSpeed); // walking  
        }
        else
        {
            animator.SetFloat("Speed", 0); // Idle 
        }
    }

    // Patrolling
    if (!isChasingPlayer && !isReturningToStart)
    {
        Patrol();
    }

        // Line of sight
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position;
        lineRenderer.SetPosition(0, startPosition);
        Vector3 endPoint = transform.position + transform.up * lineLength;
        lineRenderer.SetPosition(1, endPoint);

        animator.SetFloat("Horizontal", directionNormalized.x);
        animator.SetFloat("Vertical", directionNormalized.y);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

void Patrol()
{
    animator.SetFloat("Speed", moveSpeed);
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
        animator.SetFloat("Speed", 0);
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolWaitTime)
        {
            patrolTimer = 0f;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
}

    void ChangeLineColor(Color color, float alpha)
    {
        color.a = alpha;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
