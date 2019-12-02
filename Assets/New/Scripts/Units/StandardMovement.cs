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
        var oldNode = CurrentNode;
        if (oldNode)
        {
            // TODO Turn into event
            oldNode.CurrentUnit = null;
            // Remove As Defender For All Connected Nodes
            foreach (var connection in oldNode._nodePaths.Values)
            {
                if (connection.Battle.Defender == this) connection.Battle.Defender = null;
            }
            //
        }

        _currentNode = value;
        transform.position = _currentNode.transform.position;
        CurrentNode.CurrentUnit = NodeUnit;
        NodeReachedAction.Invoke(this, CurrentNode);

        CurrentConnection = null;
    }

    [SerializeField] NodeConnection _currentConnection;
    public NodeConnection CurrentConnection { get { return _currentConnection; } private set { SetCurrentConnection(value); } }
    private void SetCurrentConnection(NodeConnection value)
    {
        if (CurrentConnection) CurrentConnection._nodePathVisuals.Highlighted = false;
        _currentConnection = value;
        if (CurrentConnection)
        {
            StartCoroutine(MoveAlongConnection());
        }
        if (CurrentConnection) CurrentConnection._nodePathVisuals.Highlighted = true;
    }


    [SerializeField] Node _nextNode;
    public Node NextNode { get { return _nextNode; } set { SetNextNode(value); } }
    private void SetNextNode(Node value)
    {
        _nextNode = value;

        CurrentConnection = NextNode.GetConnectionToNode(CurrentNode);
        CurrentConnection.Battle.Attacker = NodeUnit;

        float pathSpeedFactor = GetPathSpeedFactor(CurrentConnection);
        CurrentMovementSpeed = BaseMovementSpeed * pathSpeedFactor;

        if (NextNode.CurrentUnit && NextNode.CurrentUnit.Health && NodeUnit.Attack)
        {
            CurrentConnection.Battle.Defender = NextNode.CurrentUnit;
        }
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
        //DateManager.CurrentDayChangedAction += CurrentTurnChanged;
    }
    public void OnDisable()
    {
        //DateManager.CurrentDayChangedAction -= CurrentTurnChanged;
    }

    internal void Initialise(AbstractPlayerManager side, Node node)
    {
        _image.color = side.Colour;
        CurrentNode = node;
    }
    private void OnDestroy()
    {
        CurrentNode.CurrentUnit = null;
    }

    private float GetPathSpeedFactor(NodeConnection nextNodePath)
    {
        // TODO Temporary
        return 1f / nextNodePath._movementPointCost;
    }

    private IEnumerator MoveAlongConnection()
    {
        Vector2 startPos = CurrentNode.transform.position;
        Vector2 endPos = NextNode.transform.position;
        float currentTime = 0f, moveTime = 10f / CurrentMovementSpeed;
        while (currentTime < moveTime)
        {
            float timeFraction = currentTime / moveTime;
            //Debug.Log($"Time Fraction: {timeFration}");
            transform.position = Vector2.Lerp(startPos, endPos, timeFraction);
            yield return null;
            currentTime += Time.deltaTime;
        }
        CurrentNode = NextNode;
        NextNode = null;
        CurrentMovementSpeed = BaseMovementSpeed;
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
                NextNode = (Node)_path.Pop();
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
}
