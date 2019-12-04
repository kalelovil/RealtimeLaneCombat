using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NodeUnit))]
[RequireComponent(typeof(StandardMovement))]
public class StandardAttack : UnitComponent
{
    [SerializeField] int _attackPoints;

    internal enum AttackType
    {
        Instigation = 1,
        Retaliation = 2,
    }

    internal void DoDamage(NodeUnit recipient)
    {
        recipient.Health.TakeDamage(_attackPoints);
        StopAllCoroutines();
        StartCoroutine(AttackAnimationCoroutine(recipient.Health));
    }

    internal List<AttackUpgradeCardAbility> _upgradeList = new List<AttackUpgradeCardAbility>();
    internal void AddUpgrade(AttackUpgradeCardAbility upgradeCardAbility)
    {
        _upgradeList.Add(upgradeCardAbility);
        _attackPoints += upgradeCardAbility._effectValue;
    }

    // Overriden by Subclasses
    internal virtual bool CanInitiateAttack => true;
    internal virtual bool CanRetaliateAttack => true;
    internal virtual bool TriggersRetaliation => true;
    //

    //internal static Action<int> CurrentActionPointsChangedAction;

    #region Visual
    [Header("Visual")]
    [SerializeField] ParticleSystem _combatParticleSystem;
    #endregion

    internal virtual bool NodeInRange(Node node)
    {
        foreach (var path in node._nodePaths)
        {
            if (path.Value._node1 == NodeUnit.Movement.CurrentNode || path.Value._node2 == NodeUnit.Movement.CurrentNode)
            {
                if (NodeUnit.Movement.CurrentMovementSpeed >= path.Value._movementPointCost)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator AttackAnimationCoroutine(StandardHealth otherUnitHealth)
    {
        Debug.Log($"{name} Attacked {otherUnitHealth.name} for {_attackPoints} Damage.");

        // Turn ParticleSystem Towards Target Unit
        _combatParticleSystem.transform.right = otherUnitHealth.transform.position - _combatParticleSystem.transform.position;

        var main = _combatParticleSystem.main;
        // Radian conversion stuff
        main.startRotation = -1 * (_combatParticleSystem.transform.localRotation.eulerAngles.z * Mathf.PI / 180f);

        // Play ParticleSystem for X Seconds
        _combatParticleSystem.Play();
        yield return new WaitForSeconds(DateManager.Instance.CurrentSecondsPerDay);
        _combatParticleSystem.Stop();
    }
}
