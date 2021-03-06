﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using kalelovil.utility.pathfinding;
using System.Linq;
using System.Runtime.Serialization;

[RequireComponent(typeof(NodeVisuals))]
[ExecuteAlways]
public class Node : MonoBehaviour, IPointerClickHandler, IPathfindingNode
{
    #region PathfindingNode Interface
    public int NumOfActions { get { return 1; } }
    public bool IsEmpty { get { return !_currentUnit; } }
    public bool BlocksUnit(NodeUnit unit)
    {
        return unit && CurrentUnit && CurrentUnit.Side == unit.Side;
    }
    public Vector3 PositionForHeuristic { get { return transform.position; } }

    public IEnumerable<IPathfindingNode> GetConnectedNodes()
    {
        return _nodePaths.Keys;
    }
    public NodeConnection GetConnectionToNode(IPathfindingNode destinationNode)
    {
        _nodePaths.TryGetValue((Node)destinationNode, out var returnConnection);
        return returnConnection;
    } 
    #endregion

    [SerializeField] internal RectTransform _rectTransform;

    [SerializeField] NodeConnection _nodePathPrefab;

    [SerializeField] public NodeToConnectionMap _nodePaths = new NodeToConnectionMap();


    // Current Unit
    [SerializeField] NodeUnit _currentUnit;
    public NodeUnit CurrentUnit { get { return _currentUnit; } set { SetCurrentUnit(value); } }
    void SetCurrentUnit(NodeUnit value)
    {
        _currentUnit = value;
        if (CurrentUnit) ControllingSide = CurrentUnit.Side;
    }
    //

    // Controller
    [SerializeField] AbstractPlayerManager _controllingSide;
    public AbstractPlayerManager ControllingSide { get { return _controllingSide; } set { SetControllingSide(value); } }
    private void SetControllingSide(AbstractPlayerManager value)
    {
        _controllingSide = value;
        _nodeVisuals.SetSideVisualState(ControllingSide);
    }
    //

    // Node Feature
    public NodeFeature NodeFeature { get; internal set; }
    //
    
    [SerializeField] internal NodeVisuals _nodeVisuals;

    public void OnEnable()
    {
        CardBase.CardSelectedAction += CardSelected;
        AbstractPlayerManager.UnitSelectedAction += UnitSelected;
        StandardMovement.UnitNodeSetAction += MovableUnitNodeSet;
    }

    public void OnDisable()
    {
        CardBase.CardSelectedAction -= CardSelected;
        AbstractPlayerManager.UnitSelectedAction -= UnitSelected;
        StandardMovement.UnitNodeSetAction -= MovableUnitNodeSet;
    }


    private void CardSelected(CardBase selectedCard)
    {
        // TODO Refactor
        if (selectedCard)
        {
            bool canBePlayed = selectedCard.CanBePlayed();
            bool canBePlayedOn = selectedCard.CanBePlayedOn(this);
            _nodeVisuals.SetPlayableOnVisualState(canBePlayed && canBePlayedOn);
        }
        else
        {
            _nodeVisuals.SetPlayableOnVisualState(true);
        }
    }
    private void UnitSelected(NodeUnit unit)
    {
        var unitMovement = unit?._standardMovement;
        MovableUnitNodeSet(unitMovement, unitMovement?.CurrentNode);
    }
    private void MovableUnitNodeSet(StandardMovement unitMovement, Node node)
    {
        // TODO Refactor
        if (unitMovement)
        {
            // TODO Refactor expensive GetComponent call
            NodeUnit unit = unitMovement.GetComponent<NodeUnit>();
            //
            var pathToNode = Pathfinder.GetPathOfTypeForUnit
            (
                unitMovement.CurrentNode, 
                this, 
                Pathfinder.PathfindingType.Ground, 
                unit
            ); 
            _nodeVisuals.SetMovableToVisualState(pathToNode != null);
        }
        else
        {
            _nodeVisuals.SetMovableToVisualState(true);
        }
    }


#if UNITY_EDITOR
    public void AddNodePath()
    {
        if (NodeConnection._currentlyPlacedNodePath)
        {
            Destroy(NodeConnection._currentlyPlacedNodePath.gameObject);
        }
        NodeConnection nodePath = PrefabUtility.InstantiatePrefab(_nodePathPrefab, MapManager.Instance._pathParent) as NodeConnection;
        nodePath._node1 = this;
        NodeConnection._currentlyPlacedNodePath = nodePath;
    }

    public void Update()
    {
        if (NodeConnection._currentlyPlacedNodePath)
        {
            if (Selection.activeTransform == transform)
            {
                if (NodeConnection._currentlyPlacedNodePath._node1 != this)
                {
                    NodeConnection._currentlyPlacedNodePath._node1._nodePaths.Add(this, NodeConnection._currentlyPlacedNodePath);
                    NodeConnection._currentlyPlacedNodePath._node2 = this;
                    _nodePaths.Add(NodeConnection._currentlyPlacedNodePath._node1, NodeConnection._currentlyPlacedNodePath);
                    NodeConnection._currentlyPlacedNodePath = null;
                }
            }
        }
    }
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerInputManager.Instance.NodeClicked(this);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}