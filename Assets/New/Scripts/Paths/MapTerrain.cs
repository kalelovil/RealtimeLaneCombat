using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapTerrain : PathFeature
{
    public enum MapTerrainType
    {
        Forest = 1,
        Hills = 2,
        Mountains = 3,
        Swamp = 4,
        Water = 5,
    }
    static List<int> _typeAttackPenaltyList = new List<int>(5)
    {
        0,
        -2,
        -4,
        -4,
        -6,
    };
    static List<List<NodeUnit.UnitType>> _unitTypesBlocked = new List<List<NodeUnit.UnitType>>(5)
    {
        new List<NodeUnit.UnitType>(){ },
        new List<NodeUnit.UnitType>(){ },
        new List<NodeUnit.UnitType>(){ NodeUnit.UnitType.Tanks},
        new List<NodeUnit.UnitType>(){ },
        new List<NodeUnit.UnitType>(){ NodeUnit.UnitType.Infantry, NodeUnit.UnitType.Artillery, NodeUnit.UnitType.Tanks},
    };
    [SerializeField] MapTerrainType _type;
    public MapTerrainType Type { get { return _type; } }

    [SerializeField] Color _lineColor;
    public Color LineColor { get { return _lineColor; } internal set { _lineColor = value; } }

    public override int GetAttackPenalty()
    {
        return _typeAttackPenaltyList[(int)Type-1];
    }
    public override int GetMovePenalty()
    {
        throw new NotImplementedException();
    }
    public override bool GetIsMoveBlocked(NodeUnit unit)
    {
        return _unitTypesBlocked[(int)Type-1].Contains(unit._unitType) && !unit._standardMovement._canTraverseTerrainMap.ContainsKey(Type);
    }
}