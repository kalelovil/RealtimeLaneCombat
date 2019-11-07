using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SummonCardAbility : CardAbility
{
    [SerializeField] NodeUnit _unitPrefab;

    public override bool CanActivateOn(Node node)
    {
        if (node.NodeFeature)
        {
            if (node.ControllingSide == TurnManager.Instance.CurrentSide)
            {
                if (!node.CurrentUnit)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override void ActivateOn(Node node)
    {
        NodeUnit unit = Instantiate(_unitPrefab, MapManager.Instance._unitParent);
        unit.Initialise(TurnManager.Instance.CurrentSide, node);
    }
}
