using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [Tooltip("Source for one-shot sounds (notes, fails)")]
    [SerializeField] private AudioSource oneShotAudioSource; // Your original source
    [Tooltip("Source for the looping sliding sound")]
    [SerializeField] private AudioSource slidingAudioSource; // The NEW source

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

        // Find the OneShot source if we forgot to link it
        if (oneShotAudioSource == null)
        {
            // Try to get the first AudioSource if not assigned
            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length > 0) oneShotAudioSource = sources[0];
            Debug.LogWarning("OneShot AudioSource not assigned in Inspector, trying to find one.");
        }
        // Find the Sliding source if we forgot to link it
        if (slidingAudioSource == null)
        {
            AudioSource[] sources = GetComponents<AudioSource>();
            // If there are multiple, assume the second one is for sliding
            if (sources.Length > 1) slidingAudioSource = sources[1];
            else slidingAudioSource = oneShotAudioSource; // Fallback, might not be ideal
            Debug.LogWarning("Sliding AudioSource not assigned in Inspector, trying to find one.");
        }

        if (slidingAudioSource != null)
        {
            slidingAudioSource.loop = true; // Make it loop
            slidingAudioSource.playOnAwake = false; // Don't play automatically
        }
        else
        {
            Debug.LogError("Sliding AudioSource is missing in AudioManager!");
        }
    }

    // --- Public functions for the GameManager to call ---

    public void PlayCorrectNote(int stringIndex)
    {
        // Basic safety check for the array index
        if (correctStringSounds != null && stringIndex >= 0 && stringIndex < correctStringSounds.Length && correctStringSounds[stringIndex] != null)
        {
            oneShotAudioSource.PlayOneShot(correctStringSounds[stringIndex]);
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
            oneShotAudioSource.PlayOneShot(failBarSound);
        }
    }

    public void PlayFailMissSound()
    {
        if (failMissSound != null)
        {
            oneShotAudioSource.PlayOneShot(failMissSound);
        }
    }

    /*public void PlaySlidingSound()
    {
        int selection = Random.Range(0, slidingOverBarsSound.Length - 1);
        if (slidingOverBarsSound != null)
        {
            oneShotAudioSource.PlayOneShot(slidingOverBarsSound[selection]);
        }
    }*/

    public void StartSlidingSound()
    {
        // Only start if it's not already playing and we have sounds
        if (slidingAudioSource != null && !slidingAudioSource.isPlaying && slidingOverBarsSound != null && slidingOverBarsSound.Length > 0)
        {
            // 1. Pick a random clip from the array
            int selection = Random.Range(0, slidingOverBarsSound.Length);
            AudioClip clipToPlay = slidingOverBarsSound[selection];

            // 2. Assign it to the sliding source
            slidingAudioSource.clip = clipToPlay;

            // 3. Play the source (it's already set to loop)
            slidingAudioSource.Play();
        }
    }

    public void StopSlidingSound()
    {
        if (slidingAudioSource != null && slidingAudioSource.isPlaying)
        {
            slidingAudioSource.Stop();
        }
    }
}