﻿using kalelovil.utility.pathfinding;
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
        RegenerateConnectionReferences();
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
        RegenerateConnectionReferences();
        Instance = this;
    }

    internal int? GetDistanceBetweenNodes(Node startNode, Node endNode, Pathfinder.PathfindingType pathfindingType)
    {
        // TODO Temp
        Path path = Pathfinder.GetPathOfTypeForUnit(startNode, endNode, pathfindingType, null);
        //
        return path?._pathNodeStack.Count;
    }

    public void RegenerateConnectionReferences()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        NodeConnection[] connections = FindObjectsOfType<NodeConnection>();

        foreach (NodeConnection connection in connections)
        {
            connection._node1 = GetClosestNodeTo(connection, 0, nodes);
            connection._node2 = GetClosestNodeTo(connection, 1, nodes);

            var node1 = connection._node1;
            var node2 = connection._node2;

            node1._nodeConnections[node2] = connection;
            node2._nodeConnections[node1] = connection;
        }
    }

    public void ShortenConnections()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        NodeConnection[] connections = FindObjectsOfType<NodeConnection>();

        float newStartFraction = 0.1f, newEndFraction = 1f - newStartFraction;
        foreach (NodeConnection connection in connections)
        {
            Vector3 startPosition = connection._lineRenderer.GetPosition(0);
            Vector3 endPosition = connection._lineRenderer.GetPosition(1);

            Vector3 newStartPosition = ((startPosition * newEndFraction) + (endPosition * newStartFraction)) / 2f;
            Vector3 newEndPosition = ((startPosition * newStartFraction) + (endPosition * newEndFraction)) / 2f;

            connection._lineRenderer.SetPosition(0, newStartPosition);
            connection._lineRenderer.SetPosition(1, newEndPosition);
        }
    }

    private Node GetClosestNodeTo(NodeConnection connection, int index, Node[] nodes)
    {
        Vector2 pos = connection._lineRenderer.GetPosition(index);
        //Debug.Log($"Connection {connection.name} End Position: {pos}");
        Vector2 nodePos;
        foreach (var node in nodes)
        {
            nodePos = (Vector2)node.transform.localPosition;
            //Debug.Log($"Node {node.name} Position: {nodePos}");
            if (Vector3.Distance(nodePos, pos) < 1f)
            {
                return node;
            }
        }
        throw new InvalidOperationException($"No Node Found At Position: {pos}");
    }
}
