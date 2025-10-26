using UnityEngine;

[ExecuteAlways]
public class StretchyFingerSolver : MonoBehaviour
{
    [Header("Bones")]
    public Transform root;
    public Transform mid;
    public Transform end;

    [Header("Target")]
    public Transform target;

    [Header("Settings")]
    [Tooltip("Maximum stretch multiplier")]
    public float maxStretch = 4.5f;
    [Tooltip("If true, bones are oriented vertically")]
    public bool vertical = false;

    private float upperLength;
    private float lowerLength;
    private float totalLength;

    private Vector3 rootInitialRight;
    private Vector3 midInitialRight;

    void Awake()
    {
        if (root == null || mid == null || end == null || target == null)
        {
            Debug.LogWarning("Assign all bones and target!");
            enabled = false;
            return;
        }

        upperLength = Vector3.Distance(root.position, mid.position);
        lowerLength = Vector3.Distance(mid.position, end.position);
        totalLength = upperLength + lowerLength;

        rootInitialRight = root.right;
        midInitialRight = mid.right;
    }

    void LateUpdate()
    {
        Vector3 rootPos = root.position;
        Vector3 targetPos = target.position;

        // Direction from root to target
        Vector3 dir = (targetPos - rootPos).normalized;
        float dist = Vector3.Distance(rootPos, targetPos);

        // Clamp total stretch
        dist = Mathf.Min(dist, totalLength * maxStretch);

        // Compute new positions along the line
        mid.position = rootPos + dir * (upperLength / totalLength * dist);
        end.position = rootPos + dir * dist;

        // Rotate bones along the direction
        root.right = dir;
        mid.right = dir;

        if (vertical)
        {
            root.up = dir;
            mid.up = dir;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (root == null || mid == null || end == null || target == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(root.position, mid.position);
        Gizmos.DrawLine(mid.position, end.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(end.position, target.position);
    }
}
