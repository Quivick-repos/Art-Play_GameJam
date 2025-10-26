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


    [Tooltip("Sounds for hitting the metal bar (picks one randomly)")]
    [SerializeField] private AudioClip[] failBarSounds;

    [Tooltip("Sound for missing completely")]
    [SerializeField] private AudioClip failMissSound;

    [Tooltip("Array of 3 sounds for correct notes (A for 0/1, B for 2/3, C for 4/5)")]
    [SerializeField] private AudioClip[] correctNoteSounds;

    [Header("End Game Chord Clips")]
    [Tooltip("Sounds for a perfect game (3/3 correct)")]
    [SerializeField] private AudioClip[] correctChordSounds;
    [Tooltip("Sounds for an imperfect game (<3 correct)")]
    [SerializeField] private AudioClip[] incorrectChordSounds;

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

    public void PlayCorrectNote(int actualStringIndex)
    {
        // Check if the array exists and has at least 3 sounds
        if (correctNoteSounds != null && correctNoteSounds.Length >= 3)
        {
            int soundIndex = 0; // Default to the first sound

            // --- Determine which sound to play based on string index ---
            if (actualStringIndex >= 0 && actualStringIndex <= 1)
            {
                soundIndex = 0; // Sound A for strings 0, 1
            }
            else if (actualStringIndex >= 2 && actualStringIndex <= 3)
            {
                soundIndex = 1; // Sound B for strings 2, 3
            }
            else if (actualStringIndex >= 4 && actualStringIndex <= 5)
            {
                soundIndex = 2; // Sound C for strings 4, 5
            }
            else
            {
                // Handle unexpected string index (optional)
                Debug.LogWarning($"PlayCorrectNote called with invalid string index: {actualStringIndex}");
                return;
            }
            // -----------------------------------------------------------

            // Play the chosen clip if it's not null
            AudioClip clipToPlay = correctNoteSounds[soundIndex];
            if (clipToPlay != null)
            {
                oneShotAudioSource.PlayOneShot(clipToPlay);
            }
            else
            {
                Debug.LogWarning($"Correct note sound at index {soundIndex} (for string {actualStringIndex}) is missing!");
            }
        }
        else
        {
            Debug.LogWarning("Correct note sounds array is missing or doesn't have 3 sounds!");
        }
    }

    public void PlayFailBarSound()
    {
        if (failBarSounds != null && failBarSounds.Length > 0)
        {
            // Pick a random index
            int randomIndex = Random.Range(0, failBarSounds.Length);
            AudioClip clipToPlay = failBarSounds[randomIndex];

            // Play the chosen clip if it's not null
            if (clipToPlay != null)
            {
                oneShotAudioSource.PlayOneShot(clipToPlay);
            }
            else
            {
                Debug.LogWarning($"Fail bar sound at index {randomIndex} is missing!");
            }
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

    public void PlayEndGameChord(bool perfectScore)
    {
        AudioClip[] soundArray = perfectScore ? correctChordSounds : incorrectChordSounds;
        string soundType = perfectScore ? "correct" : "incorrect";

        if (soundArray != null && soundArray.Length > 0)
        {
            int randomIndex = Random.Range(0, soundArray.Length);
            AudioClip clipToPlay = soundArray[randomIndex];
            if (clipToPlay != null)
            {
                oneShotAudioSource.PlayOneShot(clipToPlay);
            }
            else
            {
                Debug.LogWarning($"End game {soundType} chord sound at index {randomIndex} is missing!");
            }
        }
        else
        {
            Debug.LogWarning($"End game {soundType} chord sounds array is empty or missing!");
        }
    }
}