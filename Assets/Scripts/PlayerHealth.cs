using UnityEngine;
using UnityEngine.SceneManagement;  // Add this to use SceneManager

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public AudioClip deathSound;
    public Vector3 soundPositionOffset = Vector3.zero;

    private CountDown countDown;

    void Start()
    {
        currentHealth = maxHealth;
        countDown = FindObjectOfType<CountDown>(); // Find CountDown in the scene
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth == 0)
        {
            PlayDeathSound();
            TriggerGameOver();  // Trigger game over when health reaches 0
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            Vector3 playPosition = transform.position + soundPositionOffset;
            AudioSource.PlayClipAtPoint(deathSound, playPosition);
        }
    }

    private void TriggerGameOver()
    {
        if (countDown != null)
        {
            countDown.TriggerGameOver(); // Call GameOver from CountDown script
        }
    }
}
