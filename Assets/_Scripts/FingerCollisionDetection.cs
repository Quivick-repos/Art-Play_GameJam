using UnityEngine;

public class FingerCollisionDetection : MonoBehaviour
{
    private CircleCollider2D fingerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        fingerCollider = GetComponent<CircleCollider2D>();
    }


    void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Strings"))
        {
            AudioManager.Instance.PlaySlidingSound();
            Debug.Log("sliding string");
        }
        else if (collider.CompareTag("MetalBars"))
        {
            Debug.Log("Barras mano");
        }
        else
        {
            Debug.Log("how");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
