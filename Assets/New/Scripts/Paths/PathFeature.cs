using System;
using UnityEngine;

[Serializable]
public abstract class PathFeature : MonoBehaviour
{
    public abstract int GetAttackPenalty();

    public abstract int GetMovePenalty();

    public abstract bool GetIsMoveBlocked(NodeUnit unit);

    internal void SetPosition(NodeConnection nodePath)
    {
        transform.position = (nodePath._node1.transform.position + nodePath._node2.transform.position) / 2f;
        transform.right = (nodePath._node2.transform.position - nodePath._node1.transform.position);
    }
}