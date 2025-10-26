using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : BaseController
{    
    
    public FrogHandController frogHand;
    [SerializeField] public Transform thisTrans;
    

    void OnEnable()
    {
        selectCharacter();
        playerInputActions.Player2Movement.Enable();
        playerInputActions.Player2Movement.SelectNextFinger.performed += SelectNextFingerDebug;
        playerInputActions.Player2Movement.SelectPreviousFinger.performed += SelectPreviousFingerDebug;


    }

    void Awake(){
        playerInputActions = new InputScheme();
    }


    void OnDisable(){
        playerInputActions.Player2Movement.Disable();
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
        //Debug.Log(fingerPointList.Count);
        fingerJointList = frogHand.getFingerMiddleJointList();
        //Debug.Log(fingerJointList.Count);
        fingerknuckleList = frogHand.getFingerKnuckleList();
        //Debug.Log(fingerknuckleList.Count);
        fingerMinDistanceList = frogHand.getMinDistancesList();
        fingerMaxDistanceList = frogHand.getMaxDistancesList();

        currentFingerTarget = fingerTargetList[0];



        //Debug.Log(fingerTargetList.Count);
    }
    

public void SelectNextFingerDebug(InputAction.CallbackContext context)
    {
        //Debug.Log("currentFingerIndex = " + currentFingerIndex);
        try
        {
            if (currentFingerIndex == fingerTargetList.Count - 1)
            {
                currentFingerIndex = 0;
                currentFingerTarget = fingerTargetList[currentFingerIndex];

            }
            else
            {
                currentFingerIndex++;
                currentFingerTarget = fingerTargetList[currentFingerIndex];
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }
        public void SelectNextFinger(){
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
    public void SelectPreviousFingerDebug(InputAction.CallbackContext context)
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

        public void SelectPreviousFinger()
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
        Vector2 movementInput = playerInputActions.Player2Movement.teamMovement.ReadValue<Vector2>();
        //Debug.Log(currentFingerTarget);
        Vector3 futurePosition = currentFingerTarget.position + new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed * Time.deltaTime;
        float calculatedDistance = Vector3.Distance(futurePosition, fingerknuckleList[currentFingerIndex].position);

        if (calculatedDistance > fingerMinDistanceList[currentFingerIndex] && calculatedDistance < fingerMaxDistanceList[currentFingerIndex])
        {
          
            //calculate % difference in distance to apply to scale
            float doubleMinDist = fingerMinDistanceList[currentFingerIndex] * 2;
            if (calculatedDistance > doubleMinDist ){
                // float PercentageDifference = calculatedDistance / (fingerMinDistanceList[currentFingerIndex] * 2);
                // fingerPointList[currentFingerIndex].localScale      = new Vector3(1f + (1f - PercentageDifference), fingerPointList[currentFingerIndex].localScale.y, fingerPointList[currentFingerIndex].localScale.z);
                // fingerJointList[currentFingerIndex].localScale      = new Vector3(1f + (1f -PercentageDifference), fingerJointList[currentFingerIndex].localScale.y, fingerJointList[currentFingerIndex].localScale.z);
                // fingerknuckleList[currentFingerIndex].localScale    = new Vector3(1f + (1f -PercentageDifference), fingerknuckleList[currentFingerIndex].localScale.y,fingerknuckleList[currentFingerIndex].localScale.z);
            
            }
            currentFingerTarget.position += new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed * Time.deltaTime;
        }
        

        
    }
    
}
