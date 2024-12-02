using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public JoystickScript joystickScript;
    public float moveSpeed = 5f;
    public float boostMultiplier = 2f; // Speed boost factor
    public Rigidbody2D rb;
    public Animator animator;
    public Slider staminaSlider;

    // Stamina variables
    public float maxStamina = 150f;
    public float staminaDrainRate = 50f;    // Stamina drain per second while boosting
    public float staminaRegenRate = 25f;    // Stamina regen per second
    private float currentStamina;

    private bool isBoosting = false;

    private Vector2 movement;

    // Audio variables
    public AudioClip sprintSound; // Sprint sound clip
    public AudioClip walkSound;  // Walking sound clip
    private AudioSource audioSource;

    void Start()
    {
        // Initialize current stamina to maximum
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina; // Set slider value to match current stamina
        }

        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Update movement based on joystick input
        if (joystickScript != null)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        // Drain stamina if boosting
        if (isBoosting && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                StopBoost(); // Stop boost if stamina is depleted
            }
        }
        else
        {
            // Regenerate stamina when not boosting
            currentStamina = Mathf.Min(currentStamina + staminaRegenRate * Time.deltaTime, maxStamina);
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }

        // Listen for input to start or stop boosting
        if (Input.GetButtonDown("Sprint")) // Replace with your actual input button for sprint
        {
            StartBoost();
        }

        if (Input.GetButtonUp("Sprint")) // Replace with your actual input button for sprint
        {
            StopBoost();
        }

        // Play walking sound if moving
        if (movement.sqrMagnitude > 0.1f && !isBoosting) // Only play walking sound if moving and not boosting
        {
            PlayWalkingSound();
        }
        else if (movement.sqrMagnitude == 0 && !isBoosting) // Stop walking sound if idle
        {
            StopWalkingSound();
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Apply speed boost if boosting and stamina is available
            float currentSpeed = isBoosting ? moveSpeed * boostMultiplier : moveSpeed;
            rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
        }
    }

    // Method to start boost (call this from the UI button)
    public void StartBoost()
    {
        if (currentStamina > 0) // Start boost only if there is stamina left
        {
            isBoosting = true;

            // Play the sprint sound if it's set
            if (sprintSound != null && !audioSource.isPlaying) // Prevent sound from playing again if it's already playing
            {
                audioSource.PlayOneShot(sprintSound);
            }
            else
            {
                Debug.LogWarning("Sprint sound not assigned!");
            }
        }
    }

    // Method to stop boost (call this from the UI button)
    public void StopBoost()
    {
        isBoosting = false;

        // Stop the sprint sound when boost ends
        audioSource.Stop();
    }

    // Method to play walking sound
    private void PlayWalkingSound()
    {
        if (!audioSource.isPlaying && walkSound != null) // Play walking sound if not already playing
        {
            audioSource.clip = walkSound;
            audioSource.loop = true;  // Loop the walking sound as long as the player is moving
            audioSource.Play();
        }
    }

    // Method to stop walking sound
    private void StopWalkingSound()
    {
        if (audioSource.isPlaying && audioSource.clip == walkSound) // Stop if walking sound is playing
        {
            audioSource.Stop();
        }
    }
    public bool IsBoosting()
    {
        return isBoosting;
    }

}
