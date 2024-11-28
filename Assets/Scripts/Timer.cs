using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] float duration; // duration in minutes
    [SerializeField] AudioSource startSound; // Sound to play when countdown starts
    [SerializeField] AudioSource endSound;   // Sound to play when countdown ends

    private float currentTime;

    void Start()
    {
        currentTime = duration * 60; // convert duration to seconds
        PlayStartSound(); // Play start sound
        StartCoroutine(TimeIEn());
    }

    void PlayStartSound()
    {
        if (startSound != null)
        {
            startSound.Play();
        }
    }

    void PlayEndSound()
    {
        if (endSound != null)
        {
            endSound.Play();
        }
    }

    IEnumerator TimeIEn()
    {
        while (currentTime >= 0)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60); // calculate minutes
            int seconds = Mathf.FloorToInt(currentTime % 60); // calculate seconds

            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // format as MM:SS
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        timeText.text = "00:00"; // display 00:00 when timer ends
        PlayEndSound(); // Play end sound
    }
}
