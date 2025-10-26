using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : BaseController
{    
    
    public FrogHandController frogHand;
    [SerializeField] public Transform thisTrans;
    

    void OnEnable()
    {
        selectCharacter();
        playerInputActions.Player1Movement.Enable();
        playerInputActions.Player1Movement.SelectNextFinger.performed += SelectNextFinger;
        playerInputActions.Player1Movement.SelectPreviousFinger.performed += SelectPreviousFinger;


    }

    void Awake(){
        playerInputActions = new InputScheme();
    }


    void OnDisable(){
        playerInputActions.Player1Movement.Disable();
    }


    void FixedUpdate()
    {
        moveCurrentFinger(5);
    }

    public void selectCharacter()
    {
        frogHand = new FrogHandController(thisTrans);

        fingerTargetList = frogHand.getFingerTargetList();
        //Debug.Log(fingerTargetList.Count);
        fingerPointList = frogHand.getFingerPointList();
        //Debug.Log(fingerTargetList.Count);
        fingerJointList = frogHand.getFingerMiddleJointList();
        //Debug.Log(fingerTargetList.Count);
        fingerknuckleList = frogHand.getFingerKnuckleList();
        //Debug.Log(fingerTargetList.Count);
        fingerMinDistanceList = frogHand.getMinDistancesList();
        fingerMaxDistanceList = frogHand.getMaxDistancesList();
        
        currentFingerTarget = fingerTargetList[0];
        //Debug.Log(fingerTargetList.Count);
    }
    

    public void SelectNextFinger(InputAction.CallbackContext context){
        //Debug.Log("currentFingerIndex = " + currentFingerIndex);
        try
        {
            if (currentFingerIndex == fingerTargetList.Count-1){
            currentFingerIndex = 0;
            currentFingerTarget = fingerTargetList[currentFingerIndex]; 
            
        }
        else{
            currentFingerIndex++;
            currentFingerTarget = fingerTargetList[currentFingerIndex];
        }
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }
    public void SelectPreviousFinger(InputAction.CallbackContext context)
    {
        //Debug.Log("currentFingerIndex = " + currentFingerIndex);
        try
        {
            if (currentFingerIndex == 0)
            {
                currentFingerIndex = fingerTargetList.Count - 1;
                currentFingerTarget = fingerTargetList[currentFingerIndex];
            }
            else
            {
                currentFingerIndex--;
                currentFingerTarget = fingerTargetList[currentFingerIndex];
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }



    }
    
    public void moveCurrentFinger(float animalMoveSpeed){
        Vector2 movementInput = playerInputActions.Player1Movement.teamMovement.ReadValue<Vector2>();
        Vector3 futurePosition = currentFingerTarget.position + new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed * Time.deltaTime;
        float calculatedDistance = Vector3.Distance(futurePosition, fingerknuckleList[currentFingerIndex].position);
        if (calculatedDistance > fingerMinDistanceList[currentFingerIndex] && calculatedDistance < fingerMaxDistanceList[currentFingerIndex]){
          
            //calculate % difference in distance to apply to scale
            float doubleMinDist = fingerMinDistanceList[currentFingerIndex] * 2;
            if (calculatedDistance > doubleMinDist ){
                float PercentageDifference = calculatedDistance / doubleMinDist;
                Debug.Log("Difference percentage" + PercentageDifference);
                fingerPointList[currentFingerIndex].localScale = new Vector3(PercentageDifference, fingerPointList[currentFingerIndex].localScale.y, fingerPointList[currentFingerIndex].localScale.z);
                //fingerPointList[currentFingerIndex].GetComponent<CircleCollider2D>().transform.localScale = Vector3.one;
                fingerJointList[currentFingerIndex].localScale      = new Vector3(PercentageDifference, fingerJointList[currentFingerIndex].localScale.y, fingerJointList[currentFingerIndex].localScale.z);
                fingerknuckleList[currentFingerIndex].localScale    = new Vector3(PercentageDifference, fingerknuckleList[currentFingerIndex].localScale.y,fingerknuckleList[currentFingerIndex].localScale.z);
            
            }
            currentFingerTarget.position += new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed * Time.deltaTime;
        }
    }
    
}
