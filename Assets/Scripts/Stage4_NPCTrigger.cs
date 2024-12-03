using UnityEngine;

public class Stage4_NPCTrigger : MonoBehaviour
{
    public GameObject guard;  // Reference to the guard GameObject
    private Stage4_Guard2 guardScript;

    void Start()
    {
        // Get the guard's script component
        if (guard != null)
        {
            guardScript = guard.GetComponent<Stage4_Guard2>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // When player enters the NPC's trigger zone
        {
            Debug.Log("Player entered the trigger zone");
            if (guardScript != null)
            {
                guardScript.ActivateChasing(true); // Activate chasing
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // When player leaves the NPC's trigger zone
        {
            Debug.Log("Player exited the trigger zone");
            if (guardScript != null)
            {
                guardScript.ActivateChasing(false); // Deactivate chasing
            }
        }
    }

}
