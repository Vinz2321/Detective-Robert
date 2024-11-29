using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string SceneName;
    public AudioClip buttonClickSound; // Reference to the sound clip
    private AudioSource audioSource;

    void Start()
    {
        // Ensure the AudioSource component is attached to the GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if not found
        }
    }

    public void ChangeScene()
    {
        // Play the button click sound if it's assigned
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }

        // Load the scene
        //SceneManager.LoadScene(SceneName);
    }
}
