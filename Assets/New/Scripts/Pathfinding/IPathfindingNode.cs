using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace kalelovil.utility.pathfinding
{
    public interface IPathfindingNode : ISerializable
    {
        int NumOfActions { get; }
        bool IsEmpty { get; }
        Vector3 PositionForHeuristic { get; }

        IEnumerable<IPathfindingNode> GetConnectedNodes();

        NodeConnection GetConnectionToNode(IPathfindingNode node);
        bool BlocksUnit(NodeUnit unit);
    }
}
