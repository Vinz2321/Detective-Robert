using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CopsPatrol : MonoBehaviour
{
    public Transform pointA;  // The starting point of patrol
    public Transform pointB;  // The ending point of patrol
    public float speed = 2f;  // Movement speed
    public float waitTime = 2f;  // Time to wait at each patrol point

    private bool movingToB = true;  // Flag to track current direction
    private bool isWaiting = false; // Flag to check if enemy is waiting

    void Update()
    {
        if (!isWaiting)
        {
            // Move towards the current target point
            if (movingToB)
            {
                transform.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
                if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
                {
                    // Reached point B, switch direction after a wait
                    StartCoroutine(WaitAndSwitchDirection());
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
                if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
                {
                    // Reached point A, switch direction after a wait
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
}