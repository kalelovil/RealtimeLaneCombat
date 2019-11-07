using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Node))]
public abstract class NodeFeature : MonoBehaviour
{
    #region Range
    [SerializeField] int _range;
    public int Range { get { return _range; } set { _range = value; } }
    #endregion

    public enum NodeFeatureType
    {
        Null = 0,
        Airfield = 1,
        SupplyPoint = 2,
    }
    public abstract NodeFeatureType FeatureType {get;}

    [SerializeField] protected Sprite _sprite;

    protected void OnValidate()
    {
        Node node = GetComponent<Node>();
        node._nodeVisuals.SetSprite(_sprite);
    }

    void Awake()
    {
        Node node = GetComponent<Node>();
        node.NodeFeature = this;
    }
}