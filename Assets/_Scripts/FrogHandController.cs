using UnityEngine;
using System.Collections.Generic;
public class FrogHandController
{
    [SerializeField] Transform LeftFingerTarget;
    [SerializeField] Transform MiddleFingerTarget;
    [SerializeField] Transform RightFingerTarget;

    [SerializeField] Transform LeftFingerPoint;
    [SerializeField] Transform MiddleFingerPoint;
    [SerializeField] Transform RightFingerPoint;

    [SerializeField] Transform LeftFingerKnuckle;
    [SerializeField] Transform MiddleFingerKnuckle;
    [SerializeField] Transform RightFingerKnuckle;

    [SerializeField] Transform LeftFingerMiddleJoint;
    [SerializeField] Transform MiddleFingerMiddleJoint;
    [SerializeField] Transform RightFingerMiddleJoint;

    [SerializeField] float LeftFingerTargetToKnuckleDistance;
    [SerializeField] float MiddleFingerTargetToKnuckleDistance;
    [SerializeField] float RightFingerTargetToKnuckleDistance;
    private List<Transform> fingerList = new();
    private GameObject frogHand;
    private float defaultMoveSpeed = 5f;
    private float moveSpeed = 5f;

    public FrogHandController(Transform thisTrans)
    {
        frogHand = Resources.Load<GameObject>("Prefabs/FrogHand green");
        GameObject FrogInstance = GameObject.Instantiate(frogHand, thisTrans);

        LeftFingerTarget = FrogInstance.GetComponentInChildren<Transform>().Find("LeftFingerSolver");
        MiddleFingerTarget = FrogInstance.GetComponentInChildren<Transform>().Find("MiddleFingerSolver");
        RightFingerTarget = FrogInstance.GetComponentInChildren<Transform>().Find("RightFingerSolver");

        LeftFingerKnuckle = FrogInstance.GetComponentInChildren<Transform>().Find("bone_1").Find("bone_2").Find("LeftFinger");
        MiddleFingerKnuckle = FrogInstance.GetComponentInChildren<Transform>().Find("bone_1").Find("bone_2").Find("MiddleFinger");
        RightFingerKnuckle = FrogInstance.GetComponentInChildren<Transform>().Find("bone_1").Find("bone_2").Find("RightFinger");

        LeftFingerMiddleJoint = LeftFingerKnuckle.GetComponentInChildren<Transform>().Find("LeftFingerJoint");
        MiddleFingerMiddleJoint = MiddleFingerKnuckle.GetComponentInChildren<Transform>().Find("MiddleFingerJoint");
        RightFingerMiddleJoint = RightFingerKnuckle.GetComponentInChildren<Transform>().Find("RightFingerJoint");

        LeftFingerPoint = LeftFingerMiddleJoint.GetComponentInChildren<Transform>().Find("LeftFingerPoint");
        MiddleFingerPoint = MiddleFingerMiddleJoint.GetComponentInChildren<Transform>().Find("MiddleFingerPoint");
        RightFingerPoint = RightFingerMiddleJoint.GetComponentInChildren<Transform>().Find("RightFingerPoint");


        LeftFingerTargetToKnuckleDistance = Vector3.Distance(LeftFingerKnuckle.position, LeftFingerTarget.position);
        MiddleFingerTargetToKnuckleDistance = Vector3.Distance(MiddleFingerKnuckle.position, MiddleFingerTarget.position);
        RightFingerTargetToKnuckleDistance = Vector3.Distance(RightFingerTarget.position, RightFingerKnuckle.position);



    }
    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public void setMoveSpeed(float moveSpeedMultiplier)
    {
        moveSpeed = defaultMoveSpeed * moveSpeedMultiplier;
    }

    public void resetMoveSpeed()
    {
        moveSpeed = defaultMoveSpeed;
    }

    public List<Transform> getFingerTargetList()
    {
        return new List<Transform> { LeftFingerTarget, MiddleFingerTarget, RightFingerTarget };
    }

    public List<Transform> getFingerPointList()
    {
        return new List<Transform> { LeftFingerPoint, MiddleFingerPoint, RightFingerPoint };
    }

    public List<Transform> getFingerKnuckleList()
    {
        return new List<Transform> { LeftFingerKnuckle, MiddleFingerKnuckle, RightFingerKnuckle };
    }

    public List<Transform> getFingerMiddleJointList()
    {
        return new List<Transform> { LeftFingerMiddleJoint, MiddleFingerMiddleJoint, RightFingerMiddleJoint };
    }
    public List<float> getMinDistancesList()
    {
        return new List<float> { LeftFingerTargetToKnuckleDistance / 2, MiddleFingerTargetToKnuckleDistance / 2, RightFingerTargetToKnuckleDistance / 2 };

    }

    public List<float> getMaxDistancesList()
    {
        return new List<float> { LeftFingerTargetToKnuckleDistance * 2f, MiddleFingerTargetToKnuckleDistance * 2f, RightFingerTargetToKnuckleDistance * 2f };

    }




}
