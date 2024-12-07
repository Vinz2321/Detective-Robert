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

    // AudioSource variables
    public AudioSource sprintAudioSource; // AudioSource for sprint sound
    public AudioClip walkingSound; // Walking sound clip
    public AudioClip breathingSound; // Breathing sound clip
    private AudioSource audioSource;
    private AudioSource breathingAudioSource;

    void Start()
    {
        // Initialize current stamina to maximum
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina; // Set slider value to match current stamina
        }

        // Initialize the audio sources
        breathingAudioSource = gameObject.AddComponent<AudioSource>();
        breathingAudioSource.volume = 0.1f;
        audioSource = gameObject.AddComponent<AudioSource>();

        if (sprintAudioSource != null)
        {
            sprintAudioSource.loop = false; // Sprint sound should not loop
        }
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

            // Play walking sound if the player is walking but not boosting
            if (!isBoosting && movement.sqrMagnitude > 0.1f && !audioSource.isPlaying && walkingSound != null)
            {
                audioSource.PlayOneShot(walkingSound); // Play walking sound
            }
            // Stop walking sound if player stops moving or starts boosting
            else if (movement.sqrMagnitude == 0 || isBoosting)
            {
                audioSource.Stop(); // Stop walking sound
            }
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

        if (currentStamina <= 0 && !breathingAudioSource.isPlaying)
        {
            PlayBreathingSound();
        }
        else if (currentStamina > 75f && breathingAudioSource.isPlaying)
        {
            StopBreathingSound();
        }

        // Listen for input to start or stop boosting
        if (Input.GetButtonDown("Sprint"))
        {
            StartBoost();
        }

        if (Input.GetButtonUp("Sprint"))
        {
            StopBoost();
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

    // Method to start boost (play sprint sound)
    public void StartBoost()
    {
        if (currentStamina > 0)
        {
            isBoosting = true;

            // Play the sprint sound if it's set and not already playing
            if (sprintAudioSource != null && !sprintAudioSource.isPlaying)
            {
                sprintAudioSource.Play(); // Play sprint sound
            }
            else
            {
                Debug.LogWarning("Sprint AudioSource not assigned or already playing!");
            }
        }
    }

    // Method to stop boost (stop sprint sound)
    public void StopBoost()
    {
        isBoosting = false;

        // Stop the sprint sound when boost ends
        if (sprintAudioSource != null && sprintAudioSource.isPlaying)
        {
            sprintAudioSource.Stop(); // Stop sprint sound
        }
    }

    private void PlayBreathingSound()
    {
        if (breathingSound != null && !breathingAudioSource.isPlaying)
        {
            breathingAudioSource.clip = breathingSound;
            breathingAudioSource.loop = true;  // Loop breathing sound
            breathingAudioSource.Play();
        }
    }

    private void StopBreathingSound()
    {
        if (breathingAudioSource.isPlaying)
        {
            breathingAudioSource.Stop(); // Stop breathing sound
        }
    }

    public bool IsBoosting()
    {
        return isBoosting;
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        this.enabled = isEnabled; // Toggles the script on or off
        if (!isEnabled)
        {
            // Stop the walking sound when movement is disabled
            audioSource.Stop();
            movement = Vector2.zero; // Ensure the player stops moving
        }
    }
}
