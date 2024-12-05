using UnityEngine;
using UnityEngine.UI;

public class Stage4_NPCTrigger : MonoBehaviour
{
    public string message = "Hello, sir!"; // The text to display
    public GameObject textUI; // Reference to the UI Text GameObject
    public Stage4_Guard2 guard; // Reference to the guard script

    private bool playerTriggered = false; // Tracks if the player triggered the NPC

    private void Start()
    {
        if (textUI != null)
        {
            textUI.SetActive(false); // Ensure the text UI is hidden at the start
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textUI != null)
        {
            textUI.SetActive(true); // Display the text UI
            textUI.GetComponent<Text>().text = message; // Set the message
            playerTriggered = true; // Mark the player as having triggered the NPC

            // Inform the guard that the player triggered this NPC
            if (guard != null)
            {
                guard.SetPlayerTriggered(true); // Start the chase
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textUI != null)
        {
            textUI.SetActive(false); // Hide the text UI when the player exits
            playerTriggered = false; // Reset the trigger flag

            // Inform the guard that the player is no longer in range
            if (guard != null)
            {
                guard.SetPlayerTriggered(false); // Stop chasing if needed
            }
        }
    }
}
