using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathVisuals : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    #region Highlighted
    [SerializeField] bool _highlighted;
    public bool Highlighted { get { return _highlighted; } internal set { SetHighlighted(value); } }
    private void SetHighlighted(bool value)
    {
        _highlighted = value;
        var lineColor = (Highlighted ? Color.yellow : Color.white);
        _lineRenderer.startColor = lineColor;
        _lineRenderer.endColor = lineColor;
    }
    #endregion

    #region Feature Visual State
    internal void SetVisualStateForFeature(PathFeature feature)
    {
        if (feature is MapTerrain)
        {
            _lineRenderer.startColor = ((MapTerrain)feature).LineColor;
            _lineRenderer.endColor = ((MapTerrain)feature).LineColor;
        }
    }
    #endregion
}
