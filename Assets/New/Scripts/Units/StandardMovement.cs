using System;
using System.Collections;
using System.Collections.Generic;
using kalelovil.utility.pathfinding;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NodeUnit))]
public class StandardMovement : UnitComponent
{
    [SerializeField] float _baseMovementSpeed;
    public float BaseMovementSpeed { get { return _baseMovementSpeed; } }
    [SerializeField] private float _currentMovementSpeed;
    public float CurrentMovementSpeed { get { return _currentMovementSpeed; } set { SetCurrentNovementSpeed(value); } }
    private void SetCurrentNovementSpeed(float value)
    {
        _currentMovementSpeed = Mathf.Max(0, value);
        if (_movementDisplay) _movementDisplay.ValueChanged(CurrentMovementSpeed, BaseMovementSpeed);
    }

    [SerializeField] Node _currentNode;
    public Node CurrentNode { get { return _currentNode; } set { SetCurrentNode(value); } }
    public static Action<StandardMovement, Node> NodeReachedAction;
    private void SetCurrentNode(Node value)
    {
        _connectionMovementFraction = 0f;
        var oldNode = CurrentNode;
        if (oldNode)
        {
            // TODO Turn into event
            oldNode.CurrentUnit = null;
            // Remove As Defender For All Connected Nodes
            foreach (var connection in oldNode._nodeConnections.Values)
            {
                if (connection.Battle.Defender == this) connection.Battle.Defender = null;
            }
            //
        }

        _currentNode = value;
        if (CurrentNode)
        {
            transform.position = _currentNode.transform.position;
            CurrentNode.CurrentUnit = NodeUnit;
            foreach (var connection in CurrentNode._nodeConnections.Values)
            {
                if (NodeUnit.Attack is ArtilleryAttack)
                {
                    if (connection._node1 == CurrentNode)
                    {
                        connection.Battle.Defender = connection._node2.CurrentUnit;
                    }
                    else if (connection._node2 == CurrentNode)
                    {
                        connection.Battle.Defender = connection._node1.CurrentUnit;
                    }
                    connection.Battle.AddAttackerSupportUnit((ArtilleryAttack)NodeUnit.Attack);
                }
            }
            NodeReachedAction.Invoke(this, CurrentNode);

            CurrentConnection = null;
        }
    }

    [SerializeField] NodeConnection _currentConnection;
    public NodeConnection CurrentConnection { get { return _currentConnection; } private set { SetCurrentConnection(value); } }
    private void SetCurrentConnection(NodeConnection value)
    {
        if (CurrentConnection)
        {
            CurrentConnection._nodePathVisuals.Highlighted = false;
            if (CurrentConnection.Battle.Attacker == NodeUnit)
            {
                CurrentConnection.Battle.Attacker = null;
            }
        }

        _currentConnection = value;

        if (CurrentConnection)
        {
            CurrentNode = null;

            CurrentConnection._nodePathVisuals.Highlighted = true;

            CurrentConnection.Battle.Attacker = NodeUnit;

            float pathSpeedFactor = GetPathSpeedFactor(CurrentConnection);
            CurrentMovementSpeed = BaseMovementSpeed * pathSpeedFactor;

            if (NextNode.CurrentUnit && NextNode.CurrentUnit.Health && NodeUnit.Attack)
            {
                CurrentConnection.Battle.Defender = NextNode.CurrentUnit;
            }
        }
        else
        {
            CurrentMovementSpeed = BaseMovementSpeed;
        }

    }

    [SerializeField] Node _nextNode;
    public Node NextNode { get { return _nextNode; } set { SetNextNode(value); } }
    private void SetNextNode(Node value)
    {
        _nextNode = value;
        CurrentConnection = (NextNode) ? NextNode.GetConnectionToNode(CurrentNode) : null;
    }

    internal Dictionary<MapTerrain.MapTerrainType, CanTraverseTerrain> _canTraverseTerrainMap = new Dictionary<MapTerrain.MapTerrainType, CanTraverseTerrain>();

    [Header("Visual")]
    #region Visual
    [SerializeField] Image _image;
    [SerializeField] Animator _animator;
    [Space(10)]
    [SerializeField] Transform _movementDisplayArea;
    [SerializeField] HealthDisplay _movementDisplayPrefab;
    [SerializeField] HealthDisplay _movementDisplay;
    #endregion

    public void OnEnable()
    {
        DateManager.CurrentHourChangedAction += HourStep;
    }
    public void OnDisable()
    {
        DateManager.CurrentHourChangedAction -= HourStep;
    }

    internal void Initialise(AbstractPlayerManager side, Node node)
    {
        _image.color = side.Colour;
        CurrentNode = node;
    }

    private float GetPathSpeedFactor(NodeConnection nextNodePath)
    {
        // TODO Temporary
        return 1f / nextNodePath._movementPointCost;
    }

    float _connectionMovementFraction;
    float _lastMovementAt;
    private void HourStep(int hourNum)
    {
        if (CurrentConnection)
        {
            _lastMovementAt = Time.time;
               _connectionMovementFraction = Mathf.MoveTowards
            (
                _connectionMovementFraction,
                CurrentConnection.MaxMovementFraction,
                (CurrentMovementSpeed / 100f)
            );

            if (_connectionMovementFraction == 1f)
            {
                CurrentNode = NextNode;
                NextNode = null;
            }
        }
    }

    private void Update()
    {
        if (CurrentConnection)
        {
            InterpolateFrameMovement();
        }
    }

    private void InterpolateFrameMovement()
    {
        float visualConnectionMovementFraction = Mathf.MoveTowards
        (
            _connectionMovementFraction,
            CurrentConnection.MaxMovementFraction,
            (CurrentMovementSpeed / 100f) * DateManager.Instance.CurrentFrameAsFractionOfHourStep()
        );
        //Debug.Log($"Movement Fraction: {visualConnectionMovementFraction}");

        if (NextNode == CurrentConnection._node1)
        {
            visualConnectionMovementFraction = 1f - visualConnectionMovementFraction;
        }
        Vector2 visualPosition = Vector2.Lerp
        (
            CurrentConnection._lineRenderer.GetPosition(0),
            CurrentConnection._lineRenderer.GetPosition(1),
            visualConnectionMovementFraction
        );
        transform.position = visualPosition;
    }


    Path _path;
    internal void SetPath(Path path)
    {
        _path = path;
        if (_path != null) StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        while (_path._pathNodeStack.Count > 0)
        {
            if (!CurrentConnection)
            {
                var NextConnection = ((Node)_path.Peek()).GetConnectionToNode(CurrentNode);
                if (!NextConnection.Battle.Attacker)
                {
                    NextNode = (Node)_path.Pop();
                }
            }
            yield return null;
        }
        _path = null;
    }

    private void Start()
    {
        if (!_movementDisplay)
        {
            _movementDisplay = Instantiate(_movementDisplayPrefab, _movementDisplayArea);
        }
        CurrentMovementSpeed = BaseMovementSpeed;
    }

    private void OnValidate()
    {
        if (CurrentNode)
        {
            transform.position = CurrentNode.transform.position;
        }
    }
}
