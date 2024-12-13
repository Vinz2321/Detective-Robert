using System;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this to use SceneManager

public class CountDown : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private AudioSource startSound;
    [SerializeField] private AudioSource endSound;

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
            // Timer updates here if needed, or you can show remaining time in console
        }
        else
        {
            EndCountdown();
        }
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
        SceneManager.LoadScene("GameOver"); // Ensure "GameOver" is the correct scene name
    }
}
