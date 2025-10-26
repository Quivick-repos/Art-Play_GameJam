using UnityEngine;
using System;
using System.Collections.Generic;

public class BaseController : MonoBehaviour
{


    [HideInInspector] public InputScheme playerInputActions;
    public float moveSpeed = 5f;

    protected List<Transform> fingerTargetList;
    protected List<Transform> fingerPointList;
    protected List<Transform> fingerJointList;
    protected List<Transform> fingerknuckleList;
    protected List<float> fingerMinDistanceList;
    protected List<float> fingerMaxDistanceList;
    protected Transform currentFingerTarget;
    protected int currentFingerIndex = 0;
    

    void OnEnable()
    {
        playerInputActions.Player1Movement.Enable();

    }

    void Awake(){
        playerInputActions = new InputScheme();
    }


    void OnDisable()
    {
        playerInputActions.Player1Movement.Disable();

    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }



    // public void moveCurrentFinger(float animalMoveSpeed){
    //     Vector2 movementInput = playerInputActions.Player1Movement.teamMovement.ReadValue<Vector2>();
    //     Debug.Log(currentFinger);
    //     currentFinger.position += new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed * Time.deltaTime;
    // }


}

