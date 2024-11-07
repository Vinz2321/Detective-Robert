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

    void Start()
    {
        // Initialize current stamina to maximum
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina; // Set slider value to match current stamina
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
        }
    }

    // Method to stop boost (call this from the UI button)
    public void StopBoost()
    {
        isBoosting = false;
    }
}