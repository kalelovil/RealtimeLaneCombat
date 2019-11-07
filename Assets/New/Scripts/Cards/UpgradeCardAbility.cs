using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeCardAbility : CardAbility
{
    internal enum StatType
    {
        Health,
        Attack,
    }
    internal abstract StatType EffectStatType { get; }
    [SerializeField] internal int _effectValue;

    public override void ActivateOn(Node node)
    {
        var unitTarget = node.CurrentUnit;
        unitTarget.AddUpgrade(this);
    }
}
