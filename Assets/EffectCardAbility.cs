using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCardAbility : CardAbility
{
    [SerializeField] int _healthEffect;

    [SerializeField] NodeFeature.NodeFeatureType _requiredNodeFeatureType;

    public override void ActivateOn(Node node)
    {
        var unitTarget = node.CurrentUnit._standardHealth;
        unitTarget.TakeDamage(-1 * _healthEffect);
    }

    public override bool CanActivateOn(Node node)
    {
        var unitInNode = node.CurrentUnit;
        var unitHealth = unitInNode?._standardHealth;
        var unitSide = unitInNode?.Side;

        bool isInRange = NodeInRangeOfFeatureType(node, _requiredNodeFeatureType); 

        return unitInNode && unitHealth && isInRange;
    }

    private bool NodeInRangeOfFeatureType(Node node, NodeFeature.NodeFeatureType featureType)
    {
        if (featureType == NodeFeature.NodeFeatureType.Null) return true;

        // TODO Optimise
        var Nodes = MapManager.Instance._nodeParent.GetComponentsInChildren<Node>();
        foreach (Node currentNode in Nodes) 
        {
            if (currentNode.NodeFeature && currentNode.NodeFeature.FeatureType == featureType)
            {
                if (currentNode.ControllingSide == TurnManager.Instance.CurrentSide)
                {
                    if (currentNode.NodeFeature.Range == -1) return true;

                    var distance = MapManager.Instance.GetDistanceBetweenNodes(currentNode, node, kalelovil.utility.pathfinding.Pathfinder.PathfindingType.Air);
                    if (distance.HasValue && distance <= currentNode.NodeFeature.Range) return true;
                }
            }
        }
        //
        return false;
    }
}
