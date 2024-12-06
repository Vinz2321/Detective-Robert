using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public GameObject player;
    public float speed;
    public LineRenderer lineRenderer;
    public float lineLength = 4f;

    private float distance;
    private Vector2 movement;
    private bool hasBarked = false;

    [HideInInspector] public bool seesPlayer = false;
    void Start()
    {
        source = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; //POINTS

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
            if (!seesPlayer)
            {
                // First time spotting the player
                PlayBarkSound();
            }
           seesPlayer = true;
           transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
           transform.up = direction;

           ChangeLineColor(Color.red, 1.0f);
        }
        else{
            seesPlayer = false;
            hasBarked = false;
            ChangeLineColor(Color.white, 1.0f);
        }

        //MOVES DOG
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position;

        //MOVES LINE OF SIGHT
        lineRenderer.SetPosition(0, startPosition);
        Vector3 endPoint = transform.position + transform.up * lineLength;
        lineRenderer.SetPosition(1, endPoint);
    }
    void PlayBarkSound()
    {
        if (source != null && clip != null && !hasBarked)
        {
            source.PlayOneShot(clip);
            hasBarked = true; // Ensure barking happens only once
        }
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
