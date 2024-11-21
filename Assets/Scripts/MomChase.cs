using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomChase : MonoBehaviour
{
    public GameObject player;
    public GameObject dog;
    public float moveSpeed = 5f;
    public Animator animator;
    public LineRenderer lineRenderer;
    public float lineLength = 4f;
    public Rigidbody2D rb;

    private float distance;
    Vector2 movement;
    private DogScript dogScript;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; //POINTS
        dogScript = dog.GetComponent<DogScript>();
        Material lineMaterial = lineRenderer.material;

        Color color = lineMaterial.color;
        color.a = 0.1f; // OPACITY
        lineMaterial.color = color;
        ChangeLineColor(Color.white, 0.1f);
    }

    void Update()
    {
        
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

          Vector2 directionNormalized = direction.normalized;

          float dotProduct = Vector2.Dot(transform.up, directionNormalized);

        //LINE OF SIGHT RANGE
        if(distance < 4 && dotProduct > 0.90)
        {
           transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime);
           transform.up = direction;
           ChangeLineColor(Color.red, 1.0f);
        }
        else if (dogScript.seesPlayer)
        {
           transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime * 2);
           transform.up = direction;
           ChangeLineColor(Color.red, 1.0f);
        }
        else
        {
            ChangeLineColor(Color.white, 1.0f);
        }

        //MOVES MOM
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position;

        //MOVES LINE OF SIGHT
        lineRenderer.SetPosition(0, startPosition);
        Vector3 endPoint = transform.position + transform.up * lineLength;
        lineRenderer.SetPosition(1, endPoint);

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

     void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void ChangeLineColor(Color color, float alpha)
    {
        // Adjust the color's alpha value
        color.a = alpha;

        // Update the LineRenderer's colors
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
