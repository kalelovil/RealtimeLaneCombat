using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthUpgradeCardAbility : UpgradeCardAbility
{
    internal override StatType EffectStatType => StatType.Health;

    public override bool CanActivateOn(Node node)
    {
        var unitInNode = node.CurrentUnit;
        var unitHealth = unitInNode?.Health;
        var unitSide = unitInNode?.Side;

        return unitInNode && unitHealth;
    }
}
