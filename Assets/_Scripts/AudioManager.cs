using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Clips")]
    [Tooltip("Sounds of the finger sliding over strings")]
    [SerializeField] private AudioClip[] slidingOverBarsSound;
    

    [Tooltip("Sound for hitting the metal bar")]
    [SerializeField] private AudioClip failBarSound;
    
    [Tooltip("Sound for missing completely")]
    [SerializeField] private AudioClip failMissSound;

    [Tooltip("Array of sounds, one for each string (index 0 = string 0)")]
    [SerializeField] private AudioClip[] correctStringSounds; // Make sure size matches your string count

    void Awake()
    {
        // Setup Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Find the AudioSource if we forgot to link it
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // --- Public functions for the GameManager to call ---

    public void PlayCorrectNote(int stringIndex)
    {
        // Basic safety check for the array index
        if (correctStringSounds != null && stringIndex >= 0 && stringIndex < correctStringSounds.Length && correctStringSounds[stringIndex] != null)
        {
            audioSource.PlayOneShot(correctStringSounds[stringIndex]);
        }
        else
        {
            Debug.LogWarning($"Tried to play correct note sound for string index {stringIndex}, but clip is missing or index is out of range!");
        }
    }

    public void PlayFailBarSound()
    {
        if (failBarSound != null)
        {
            audioSource.PlayOneShot(failBarSound);
        }
    }

    public void PlayFailMissSound()
    {
        if (failMissSound != null)
        {
            audioSource.PlayOneShot(failMissSound);
        }
    }

    public void PlaySlidingSound()
    {
        int selection = Random.Range(0, slidingOverBarsSound.Length - 1);
        if (slidingOverBarsSound!=null)
        {
            audioSource.PlayOneShot(slidingOverBarsSound[selection]);
        }
    }
}