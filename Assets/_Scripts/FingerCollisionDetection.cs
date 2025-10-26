using UnityEngine;

public class FingerCollisionDetection : MonoBehaviour
{
    private CircleCollider2D fingerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        fingerCollider = GetComponent<CircleCollider2D>();
    }


    void OnCollisionEnter2D(Collision2D collision){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
