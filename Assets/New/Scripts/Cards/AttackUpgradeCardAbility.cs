using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AttackUpgradeCardAbility : UpgradeCardAbility
{
    internal override StatType EffectStatType => StatType.Attack;

    public override bool CanActivateOn(Node node)
    {
        var unitInNode = node.CurrentUnit;
        var unitAttack = unitInNode?._standardAttack;
        var unitSide = unitInNode?.Side;

        return unitInNode && unitAttack;
    }
}