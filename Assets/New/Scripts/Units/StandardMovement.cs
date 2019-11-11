using System;
using System.Collections;
using System.Collections.Generic;
using kalelovil.utility.pathfinding;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NodeUnit))]
public class StandardMovement : MonoBehaviour
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
    public static Action<StandardMovement, Node> UnitNodeSetAction;
    private void SetCurrentNode(Node value)
    {
        var oldNode = CurrentNode;
        if (oldNode) oldNode.CurrentUnit = null;

        _currentNode = value;
        transform.position = _currentNode.transform.position;
        CurrentNode.CurrentUnit = GetComponent<NodeUnit>();
        UnitNodeSetAction.Invoke(this, CurrentNode);

        if (oldNode && oldNode != CurrentNode)
        {
            _image.transform.position = oldNode.transform.position;
            StartCoroutine(ImageCatchUp());
        }
    }

    [SerializeField] NodeConnection _currentPath;
    public NodeConnection CurrentConnection { get { return _currentPath; } set { SetCurrentPath(value); } }
    private void SetCurrentPath(NodeConnection value)
    {
        if (CurrentConnection) CurrentConnection._nodePathVisuals.Highlighted = false;
        _currentPath = value;
        if (CurrentConnection) CurrentConnection._nodePathVisuals.Highlighted = true;
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

    internal void Move()
    {
        if (_path._pathNodeStack.Count > 0)
        {
            var nextNode = (Node)_path.Peek();
            // TODO Refactor this to avoid GetComponent
            var unit = GetComponent<NodeUnit>();
            //
            if (nextNode.CurrentUnit && unit._standardAttack)
            {
                PlayerInputManager.Instance.StartCoroutine(PlayerInputManager.Instance.UnitNodeActionCoroutine(nextNode));
            }
            else
            {
                nextNode = (Node)_path.Pop();
                var nextConnection = nextNode.GetConnectionToNode(CurrentNode);
                float pathSpeedFactor = GetPathSpeedFactor(nextConnection);
                CurrentMovementSpeed = BaseMovementSpeed * pathSpeedFactor;

                CurrentNode = (Node)nextNode;
                CurrentConnection = nextConnection;
            }
        }
    }

    private float GetPathSpeedFactor(NodeConnection nextNodePath)
    {
        // TODO Temporary
        return 1f / nextNodePath._movementPointCost;
    }

    private IEnumerator ImageCatchUp()
    {
        float delta = Vector2.Distance(_image.transform.position, transform.position);
        Vector2 startPos = _image.transform.position, endPos = transform.position;
        float currentTime = 0f, moveTime = 10f / CurrentMovementSpeed;
        while (currentTime < moveTime)
        {
            float timeFraction = currentTime / moveTime;
            //Debug.Log($"Time Fraction: {timeFration}");
            _image.transform.position = Vector2.Lerp(startPos, endPos, timeFraction);
            yield return null;
            currentTime += Time.deltaTime;
        }
        _image.transform.position = transform.position;
        CurrentConnection = null;
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
                Move();
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
