using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[CanEditMultipleObjects]
public class NodeConnection : MonoBehaviour
{
    [SerializeField] private Battle _battle;
    public Battle Battle => _battle;

    public float MaxMovementFraction => (Battle.Active) ? Battle.ProgressFraction : 1f;

    [SerializeField] internal LineRenderer _lineRenderer;
    [SerializeField] internal Node _node1, _node2;

    #region Feature
    [Header("Feature")]
    [SerializeField] internal PathFeature _pathFeature;
    #endregion

    #region Movement Cost
    [Header("Movement Cost")]
    [SerializeField] internal int _movementPointCost;
    [SerializeField] TextMeshProUGUI _movementPointText;
    #endregion
    [Space(10)]

    [SerializeField] internal NodePathVisuals _nodePathVisuals;

#if UNITY_EDITOR
    public void AddPathFeature(PathFeature pathFeaturePrefab)
    {
        if (_pathFeature)
        {
            DestroyImmediate(_pathFeature.gameObject);
            _pathFeature = null;
        }
        PathFeature pathFeature = PrefabUtility.InstantiatePrefab(pathFeaturePrefab, transform) as PathFeature;
        _pathFeature = pathFeature;
        pathFeature.SetPosition(this);
    }

    static internal NodeConnection _currentlyPlacedNodePath;

    [SerializeField] float _textYOffset;
#endif

    public void OnValidate()
    {
        if (IsInScene(_battle.gameObject)) _battle.transform.position = (_node1.transform.position + _node2.transform.position) / 2f;
    }
    bool IsInScene(GameObject gameobject)
    {
        return !String.IsNullOrEmpty(gameObject.scene.name);
    }

    public void Update()
    {
#if UNITY_EDITOR
        //_textYOffset = ((GameObject)PrefabUtility.GetCorrespondingObjectFromSource(gameObject)).transform.localPosition.y;

        if (_lineRenderer && _node1 && _node2)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new Vector3[] { _node1._rectTransform.anchoredPosition, _node2._rectTransform.anchoredPosition });

            Vector2 midPoint = ((_lineRenderer.GetPosition(0) + _lineRenderer.GetPosition(1)) / 2f);
            _movementPointText.transform.position = midPoint + new Vector2(0f, _textYOffset);
            _movementPointText.text = $"{_movementPointCost}";
        }

        // Position Path Feature
        if (_pathFeature)
        {
            _pathFeature.SetPosition(this);
            _nodePathVisuals.SetVisualStateForFeature(_pathFeature);
        }
        //
#endif
    }

    private void OnDestroy()
    {
        if (_node1)
        {
            _node1._nodeConnections.Remove(_node2);
        }
        if (_node2)
        {
            _node2._nodeConnections.Remove(_node1);
        }
    }
}
