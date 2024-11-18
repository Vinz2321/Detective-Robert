using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public Animator animator;
    public LineRenderer lineRenderer;
    public float lineLength = 6f;

    private float distance;
    private Vector2 movement;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; //POINTS

        Material lineMaterial = lineRenderer.material;

        Color color = lineMaterial.color;
        color.a = 0.1f; // OPACITY
        lineMaterial.color = color;
    }

    void Update()
    {
        
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

          Vector2 directionNormalized = direction.normalized;

          float dotProduct = Vector2.Dot(transform.up, directionNormalized);

        //LINE OF SIGHT RANGE
        if(distance < 6 && dotProduct > 0.90)
        {
           transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
           transform.up = direction;
        }

        //MOVES MOM
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position;

        //MOVES LINE OF SIGHT
        lineRenderer.SetPosition(0, startPosition);
        Vector3 endPoint = transform.position + transform.up * lineLength;
        lineRenderer.SetPosition(1, endPoint);
    }
}
