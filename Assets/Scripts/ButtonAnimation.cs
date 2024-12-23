using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{   
    Button btn;
    Vector3 upScale = new Vector3(1.2f, 1.2f, 1);
    public AudioClip buttonClickSound; // Reference to the sound clip
    private AudioSource audioSource;
    
    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Anim);
    }

     void Start()
    {
        // Ensure the AudioSource component is attached to the GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if not found
        }
    }

    void Anim()
    {
        LeanTween.scale(gameObject, upScale, 0.1f);
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.1f);

        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
