using UnityEngine;
using UnityEngine.UI;
using System;

public class CountDown : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private float duration; // Duration in minutes
    [SerializeField] private AudioSource startSound; // Sound to play when countdown starts
    [SerializeField] private AudioSource endSound;   // Sound to play when countdown ends

    private static DateTime endTime; // End time for the countdown (shared across instances)
    private static bool isCountingDown = false; // To track if the countdown is active
    private static float initialDuration; // To store the initial duration for new sessions

    private void Awake()
    {
        // Ensure this GameObject persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Only initialize the countdown if it hasn’t started before
        if (!isCountingDown)
        {
            StartCountdown(duration);
        }
    }

    /// <summary>
    /// Starts the countdown timer.
    /// </summary>
    /// <param name="newDuration">The duration of the timer in minutes.</param>
    public void StartCountdown(float newDuration)
    {
        if (isCountingDown) return;

        initialDuration = newDuration; // Store the initial duration
        endTime = DateTime.Now.AddMinutes(newDuration); // Calculate the target end time
        PlayStartSound();
        isCountingDown = true;
    }

    private void Update()
    {
        if (!isCountingDown) return;

        // Calculate the remaining time
        TimeSpan remainingTime = endTime - DateTime.Now;

        if (remainingTime.TotalSeconds > 0)
        {
            UpdateTimerUI(remainingTime);
        }
        else
        {
            EndCountdown();
        }
    }

    /// <summary>
    /// Updates the UI with the remaining time.
    /// </summary>
    /// <param name="remainingTime">TimeSpan object representing the time left.</param>
    private void UpdateTimerUI(TimeSpan remainingTime)
    {
        int minutes = Mathf.Max(remainingTime.Minutes, 0); // Ensure non-negative values
        int seconds = Mathf.Max(remainingTime.Seconds, 0); // Ensure non-negative values

        if (timeText != null) // Check if the text is available in this scene
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    /// <summary>
    /// Called when the countdown ends.
    /// </summary>
    private void EndCountdown()
    {
        if (timeText != null) // Check if the text is available in this scene
        {
            timeText.text = "00:00";
        }

        PlayEndSound();
        isCountingDown = false;
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
}
