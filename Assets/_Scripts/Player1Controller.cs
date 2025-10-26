using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : BaseController
{

    public FrogHandController frogHand;
    [SerializeField] public Transform thisTrans;

    [SerializeField] public int ScoreCounter;


    void OnEnable()
    {
        selectCharacter();
        playerInputActions.Player1Movement.Enable();
        playerInputActions.Player1Movement.SelectNextFinger.performed += SelectNextFingerDebug;
        playerInputActions.Player1Movement.SelectPreviousFinger.performed += SelectPreviousFingerDebug;


    }

    void Awake()
    {
        playerInputActions = new InputScheme();
    }


    void OnDisable()
    {
        playerInputActions.Player1Movement.Disable();
    }


    void FixedUpdate()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameplayActive)
        {
            // Optionally reset velocity if slippery mode is active
            // team1Velocity = Vector3.zero;
            // team2Velocity = Vector3.zero;
            return; // Stop processing input/movement
        }
        moveCurrentFinger(frogHand.getMoveSpeed());
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
    public void SelectNextFinger()
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

    /*public void moveCurrentFinger(float animalMoveSpeed)
    {
        Vector2 movementInput = playerInputActions.Player1Movement.teamMovement.ReadValue<Vector2>();
        Vector3 futurePosition = currentFingerTarget.position + new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed * Time.deltaTime;
        float calculatedDistance = Vector3.Distance(futurePosition, fingerknuckleList[currentFingerIndex].position);
        if (calculatedDistance > fingerMinDistanceList[currentFingerIndex] && calculatedDistance < fingerMaxDistanceList[currentFingerIndex])
        {

            //calculate % difference in distance to apply to scale
            float doubleMinDist = fingerMinDistanceList[currentFingerIndex] * 2;
            if (calculatedDistance > doubleMinDist)
            {
                float PercentageDifference = calculatedDistance / doubleMinDist;
                //Debug.Log("Difference percentage" + PercentageDifference);
                //fingerPointList[currentFingerIndex].localScale = new Vector3(PercentageDifference, fingerPointList[currentFingerIndex].localScale.y, fingerPointList[currentFingerIndex].localScale.z);
                //fingerPointList[currentFingerIndex].GetComponent<CircleCollider2D>().transform.localScale = Vector3.one;
                //fingerJointList[currentFingerIndex].localScale      = new Vector3(PercentageDifference, fingerJointList[currentFingerIndex].localScale.y, fingerJointList[currentFingerIndex].localScale.z);
                //fingerknuckleList[currentFingerIndex].localScale = new Vector3(PercentageDifference, PercentageDifference, fingerknuckleList[currentFingerIndex].localScale.z);
                //fingerJointList[currentFingerIndex].localScale     = new Vector3(1/PercentageDifference,1,1);;
                //fingerPointList[currentFingerIndex].localScale = new Vector3(1/PercentageDifference,1,1);
                //fingerPointList[currentFingerIndex].GetComponent<CircleCollider2D>().transform.localScale = new Vector3(1/PercentageDifference,1,1);
            }
            //currentFingerTarget.position += new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed * Time.deltaTime;
        }
    }*/

    public void moveCurrentFinger(float animalMoveSpeed)
    {
        Vector2 movementInput = playerInputActions.Player1Movement.teamMovement.ReadValue<Vector2>();

        // Calculate the desired velocity based purely on input and speed
        // This is the direction/speed the player WANTS to go
        Vector3 targetVelocity = new Vector3(movementInput.x, movementInput.y, 0) * animalMoveSpeed;

        // Calculate where the target WOULD go if moved directly by the desired amount THIS FRAME
        Vector3 desiredMovementDelta = targetVelocity * Time.deltaTime;
        Vector3 futurePosition = currentFingerTarget.position + desiredMovementDelta;

        // Check distance constraints using the potential future position
        float calculatedDistance = Vector3.Distance(futurePosition, fingerknuckleList[currentFingerIndex].position);

        bool isSlippery = frogHand.IsSlippery();
        // --- MOVEMENT LOGIC ---
        if (isSlippery)
        {
            Vector3 currentVelocity = frogHand.GetCurrentVelocity();
            // SLIPPERY: Smoothly adjust current velocity towards the target velocity
            frogHand.SetCurrentVelocity(Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * frogHand.slipperyAcceleration));

            // Check if applying this velocity KEEPS us within bounds
            Vector3 slipperyFuturePosition = currentFingerTarget.position + (currentVelocity * Time.deltaTime);
            float slipperyCalculatedDistance = Vector3.Distance(slipperyFuturePosition, fingerknuckleList[currentFingerIndex].position);

            if (slipperyCalculatedDistance > fingerMinDistanceList[currentFingerIndex] && slipperyCalculatedDistance < fingerMaxDistanceList[currentFingerIndex])
            {
                // Apply the calculated velocity
                currentFingerTarget.position += currentVelocity * Time.deltaTime;
            }
            else
            {
                // We would go out of bounds - dampen velocity instead of moving
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * frogHand.slipperyAcceleration * 2); // Dampen faster
                                                                                                                                   // Optionally apply the dampened velocity if still within bounds after damping:
                                                                                                                                   // currentFingerTarget.position += currentVelocity * Time.deltaTime;
            }
        }
        else // NOT Slippery
        {
            // NORMAL: Only move if the direct future position is within bounds
            if (calculatedDistance > fingerMinDistanceList[currentFingerIndex] && calculatedDistance < fingerMaxDistanceList[currentFingerIndex])
            {
                // Apply the desired movement directly
                currentFingerTarget.position += desiredMovementDelta;
            }
            // If out of bounds, do nothing (as per your original logic)
            frogHand.SetCurrentVelocity(Vector3.zero); // Ensure velocity is zeroed
        }

        // --- Your scaling logic can go here (using calculatedDistance) ---
        // (Your commented-out scaling code...)
        // if (calculatedDistance > fingerMinDistanceList[currentFingerIndex] && calculatedDistance < fingerMaxDistanceList[currentFingerIndex])
        // {
        //     //calculate % difference...
        // }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Strings")
        {
            /////TODO play sliding sstring sound here

        }

    }


    public void finalPointAggregator()
    {

    }

}
