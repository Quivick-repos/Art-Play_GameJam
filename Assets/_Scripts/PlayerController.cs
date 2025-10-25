using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform indexTarget;
    [SerializeField] Transform middleTarget;
    [SerializeField] Transform ringTarget;
    [SerializeField] Transform pinkyTarget;
    [SerializeField] Transform thumbTarget;
    [HideInInspector] public InputScheme playerInputActions;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        playerInputActions = new InputScheme();
        playerInputActions.PlayerMovement.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 temp =playerInputActions.PlayerMovement.team1Movement.ReadValue<Vector2>();
        Debug.Log("team1: "+temp);
        indexTarget.position += new Vector3(temp.x, temp.y, 0) * 5 * Time.deltaTime;


        Vector2 temp2 =playerInputActions.PlayerMovement.team2Movement.ReadValue<Vector2>();
        Debug.Log("team2: "+temp);
        middleTarget.position += new Vector3(temp2.x, temp2.y, 0) * 5 * Time.deltaTime;
    }
}
