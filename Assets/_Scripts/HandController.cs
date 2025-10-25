using UnityEngine;
using UnityEngine.U2D.IK;

public class HandController : MonoBehaviour
{
    public Transform targetToFollow;
    public CCDSolver2D[] fingerSolvers;

    private int _activeFingerIndex = 0;

    void Start()
    {
        // On start, set the first finger (index 0)
        SwitchToFinger(0);
    }

    // This will be called by our Game Manager.
    public void SwitchToFinger(int fingerIndex)
    {
        // 1. Tell the "old" finger to stop following
        if (fingerSolvers[_activeFingerIndex] != null)
        {
            fingerSolvers[_activeFingerIndex].GetChain(0).target = null;
        }

        // 2. Set the new finger index
        _activeFingerIndex = fingerIndex;

        // 3. "Wrap around" just in case
        if (_activeFingerIndex >= fingerSolvers.Length)
        {
            _activeFingerIndex = 0;
        }

        // 4. Tell the "new" finger to start following
        if (fingerSolvers[_activeFingerIndex] != null)
        {
            fingerSolvers[_activeFingerIndex].GetChain(0).target = targetToFollow;
        }
    }
}