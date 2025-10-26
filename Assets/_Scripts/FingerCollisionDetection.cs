using UnityEngine;

public class FingerCollisionDetection : MonoBehaviour
{
    // --- NEW ---
    private int stringsTouchingCount = 0; // Count how many strings we're touching

    // We don't need OnEnable or the fingerCollider variable anymore

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameplayActive)
        {
            return; // Don't process collisions if game isn't active
        }

        if (collider.CompareTag("Strings"))
        {
            // Increment the count
            stringsTouchingCount++;

            // --- If this is the FIRST string we started touching ---
            if (stringsTouchingCount == 1)
            {
                AudioManager.Instance.StartSlidingSound(); // Play the sound (needs new function)
                Debug.Log("Start sliding sound");
            }
            Debug.Log($"Entered string, count: {stringsTouchingCount}"); // Optional debug
        }
        else if (collider.CompareTag("MetalBars"))
        {
            Debug.Log("Entered Metal Bar");
            // Optionally, you could stop the sliding sound here too
            // AudioManager.Instance.StopSlidingSound();
        }
        else
        {
            Debug.Log($"Entered unknown collider: {collider.name}");
        }
    }

    // --- NEW: Add OnTriggerExit2D ---
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Strings"))
        {
            // Decrement the count
            stringsTouchingCount--;

            // --- Safety check in case count goes below zero ---
            if (stringsTouchingCount < 0) stringsTouchingCount = 0;

            // --- If this is the LAST string we stopped touching ---
            if (stringsTouchingCount == 0)
            {
                AudioManager.Instance.StopSlidingSound(); // Stop the sound (needs new function)
                Debug.Log("Stop sliding sound");
            }
            Debug.Log($"Exited string, count: {stringsTouchingCount}"); // Optional debug
        }
        else if (collider.CompareTag("MetalBars"))
        {
            Debug.Log("Exited Metal Bar");
        }
    }

    // Update is not needed
    // void Update() { }
}