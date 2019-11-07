using kalelovil.utility.pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [SerializeField] internal Transform _unitParent;
    [SerializeField] internal Transform _nodeParent;
    [SerializeField] internal Transform _pathParent;

    [SerializeField] NodeUnit _unitPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        /*
        if (_unitParent.childCount > 0) return;

        var unit = Instantiate(_unitPrefab, _unitParent);
        var startingNode = _nodeParent.GetComponentInChildren<Node>();
        if (!startingNode.CurrentUnit)
        {
            unit.CurrentNode = startingNode;
        }
        */
    }

    private void OnValidate()
    {
        Instance = this;
    }

    internal int? GetDistanceBetweenNodes(Node startNode, Node endNode, Pathfinder.PathfindingType pathfindingType)
    {
        // TODO Temp
        Path path = Pathfinder.GetPathOfType(startNode, endNode, pathfindingType);
        //
        return path?._pathNodeStack.Count;
    }

#if UNITY_EDITOR
    public void RegenerateConnectionReferences()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        NodeConnection[] connections = FindObjectsOfType<NodeConnection>();

        foreach (NodeConnection connection in connections)
        {
            var node1 = connection._node1;
            var node2 = connection._node2;

            node1._nodePaths[node2] = connection;
            node2._nodePaths[node1] = connection;
        }
    }
#endif
}
