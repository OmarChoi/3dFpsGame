using UnityEngine;

public readonly struct JumpData
{
    public readonly Vector3 StartPosition;
    public readonly Vector3 EndPosition;

    public JumpData(Vector3 startPosition, Vector3 endPosition)
    {
        StartPosition = startPosition;
        EndPosition = endPosition;
    }
}
