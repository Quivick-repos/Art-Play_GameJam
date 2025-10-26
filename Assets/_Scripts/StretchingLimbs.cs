using UnityEngine;
using UnityEngine.U2D.IK;

[DefaultExecutionOrder(1000)] // ensures this runs AFTER IKManager2D
public class StretchingLimbs : MonoBehaviour
{
    [Header("Stretch Settings")]
    [Tooltip("Maximum stretch relative to original length (1 = no stretch)")]
    public float maxStretchMultiplier = 1.5f;

    [Tooltip("How quickly to interpolate scaling changes")]
    public float smoothSpeed = 10f;

    [Tooltip("Axis along which bones are oriented (X for horizontal, Y for vertical)")]
    public Vector3 scaleAxis = new Vector3(1, 0, 0);

    private LimbSolver2D solverLeft;
    private LimbSolver2D solverMiddle;
    private LimbSolver2D solverRight;

    private IKChain2D leftChain;
    private IKChain2D middleChain;
    private IKChain2D rightChain;

    private float originalLengthLeft;
    private float originalLengthMiddle;
    private float originalLengthRight;

    private Vector3 originalLeftUpperScale;
    private Vector3 originalLeftLowerScale;

    private Vector3 originalMiddleUpperScale;
    private Vector3 originalMiddleLowerScale;

    private Vector3 originalRightUpperScale;
    private Vector3 originalRightLowerScale;

    void Awake()
    {
        solverLeft = GameObject.Find("LeftFingerSolver").GetComponent<LimbSolver2D>();
        solverMiddle = GameObject.Find("MiddleFingerSolver").GetComponent<LimbSolver2D>();
        solverRight = GameObject.Find("RightFingerSolver").GetComponent<LimbSolver2D>();

        leftChain = solverLeft.GetChain(0);
        middleChain = solverMiddle.GetChain(0);
        rightChain = solverRight.GetChain(0);

        originalLengthLeft = GetOriginalLength(leftChain);
        originalLengthMiddle = GetOriginalLength(middleChain);
        originalLengthRight = GetOriginalLength(rightChain);

        // Cache original scales
        originalLeftUpperScale = leftChain.transforms[0].localScale;
        originalLeftLowerScale = leftChain.transforms[1].localScale;
        originalMiddleUpperScale = middleChain.transforms[0].localScale;
        originalMiddleLowerScale = middleChain.transforms[1].localScale;
        originalRightUpperScale = rightChain.transforms[0].localScale;
        originalRightLowerScale = rightChain.transforms[1].localScale;
    }

    float GetOriginalLength(IKChain2D chain)
    {
        return Vector2.Distance(chain.transforms[0].position, chain.transforms[1].position)
             + Vector2.Distance(chain.transforms[1].position, chain.transforms[2].position);
    }

    void LateUpdate()
    {
        ApplyStretch(leftChain, originalLengthLeft, originalLeftUpperScale, originalLeftLowerScale);
        ApplyStretch(middleChain, originalLengthMiddle, originalMiddleUpperScale, originalMiddleLowerScale);
        ApplyStretch(rightChain, originalLengthRight, originalRightUpperScale, originalRightLowerScale);
    }

    void ApplyStretch(IKChain2D chain, float originalLength, Vector3 upperOrigScale, Vector3 lowerOrigScale)
    {
        Transform root = chain.transforms[0];
        Transform mid = chain.transforms[1];
        Transform target = chain.target;

        float currentDistance = Vector2.Distance(root.position, target.position);
        float stretchRatio = Mathf.Clamp(currentDistance / originalLength, 1f, maxStretchMultiplier);

        // Smooth stretch transition
        float smoothed = Mathf.Lerp(1f, stretchRatio, Time.deltaTime * smoothSpeed);

        // Apply proportional scaling
        root.localScale = upperOrigScale + scaleAxis * (smoothed - 1f);
        mid.localScale = lowerOrigScale + scaleAxis * (smoothed - 1f);
    }
}
