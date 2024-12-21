using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Add this to use SceneManager

using UnityEngine;
using UnityEngine.UI; // For the UI Text
using System;

public class CountDown : MonoBehaviour
{
    [SerializeField] private float duration;  // Total countdown duration in minutes
    [SerializeField] private AudioSource startSound;  // Start sound
    [SerializeField] private AudioSource endSound;  // End sound
    [SerializeField] private Text countdownText;  // Reference to the UI Text for showing the countdown

    private static DateTime endTime;
    private static bool isCountingDown = false;

    private void Start()
    {
        if (!isCountingDown)
        {
            StartCountdown(duration);
        }
    }

    public void StartCountdown(float newDuration)
    {
        if (isCountingDown) return;

        endTime = DateTime.Now.AddMinutes(newDuration);
        PlayStartSound();
        isCountingDown = true;
    }

    private void Update()
    {
        if (!isCountingDown) return;

        TimeSpan remainingTime = endTime - DateTime.Now;

        if (remainingTime.TotalSeconds > 0)
        {
            // Update the countdown display
            UpdateCountdownText(remainingTime);
        }
        else
        {
            EndCountdown();
        }
    }

    private void UpdateCountdownText(TimeSpan remainingTime)
    {
        // Display the remaining time in minutes and seconds format
        countdownText.text = string.Format("{0:D2}:{1:D2}", remainingTime.Minutes, remainingTime.Seconds);
    }

    private void EndCountdown()
    {
        PlayEndSound();
        isCountingDown = false;

        TriggerGameOver(); // Trigger Game Over when time runs out
    }

    private void PlayStartSound()
    {
        if (startSound != null)
        {
            startSound.Play();
        }
    }

    private void PlayEndSound()
    {
        if (endSound != null)
        {
            endSound.Play();
        }
    }

    // Function to handle Game Over scene loading
    public void TriggerGameOver()
    {
        Time.timeScale = 0; // Pause the game
        LoadGameOverScene(); // Load the Game Over scene
    }

    // Method to load the Game Over scene
    private void LoadGameOverScene()
    {
        // Make sure "GameOver" is the correct scene name
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }
}
