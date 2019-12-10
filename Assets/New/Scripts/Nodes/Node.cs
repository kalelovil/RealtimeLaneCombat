using System;
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

    public bool BlocksUnit(NodeUnit unit)
    {
        return unit && CurrentUnit && CurrentUnit.Side == unit.Side && !CurrentUnit.Movement.CurrentConnection;
    }
    public Vector3 PositionForHeuristic { get { return transform.position; } }

    public IEnumerable<IPathfindingNode> GetConnectedNodes()
    {
        return _nodeConnections.Keys;
    }
    public NodeConnection GetConnectionToNode(IPathfindingNode destinationNode)
    {
        _nodeConnections.TryGetValue((Node)destinationNode, out var returnConnection);
        return returnConnection;
    } 
    #endregion

    [SerializeField] internal RectTransform _rectTransform;

    [SerializeField] NodeConnection _nodePathPrefab;

    // TODO Rename to NodeConnections
    [SerializeField] public Dictionary<Node, NodeConnection> _nodeConnections = new Dictionary<Node, NodeConnection>();


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
        StandardMovement.NodeReachedAction += MovableUnitNodeSet;
        NodeUnit.UnitDestroyedAction += UnitDestroyed;

    }

    public void OnDisable()
    {
        CardBase.CardSelectedAction -= CardSelected;
        AbstractPlayerManager.UnitSelectedAction -= UnitSelected;
        StandardMovement.NodeReachedAction -= MovableUnitNodeSet;
        NodeUnit.UnitDestroyedAction -= UnitDestroyed;
    }

    private void UnitDestroyed(NodeUnit unit)
    {
        if (unit.Movement.CurrentNode == this)
        {
            CurrentUnit = null;
        }
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
        var unitMovement = unit?.Movement;
        MovableUnitNodeSet(unitMovement, unitMovement?.CurrentNode);
    }
    private void MovableUnitNodeSet(StandardMovement unitMovement, Node node)
    {
        if (node != this) return;

        // TODO Refactor
        if (unitMovement)
        {
            NodeUnit unit = unitMovement.NodeUnit;
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
                    NodeConnection._currentlyPlacedNodePath._node1._nodeConnections.Add(this, NodeConnection._currentlyPlacedNodePath);
                    NodeConnection._currentlyPlacedNodePath._node2 = this;
                    _nodeConnections.Add(NodeConnection._currentlyPlacedNodePath._node1, NodeConnection._currentlyPlacedNodePath);
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