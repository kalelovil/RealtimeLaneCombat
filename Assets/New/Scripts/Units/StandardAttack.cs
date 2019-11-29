using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NodeUnit))]
[RequireComponent(typeof(StandardMovement))]
public class StandardAttack : MonoBehaviour
{
    [SerializeField] int _attackPoints;

    StandardMovement _standardMovement;

    internal enum AttackType
    {
        Instigation = 1,
        Retaliation = 2,
    }

    internal void DoDamage(NodeUnit defender)
    {
        defender._standardHealth.TakeDamage(_attackPoints);
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

    private void Awake()
    {
        _standardMovement = GetComponent<StandardMovement>();
    }

    internal virtual bool NodeInRange(Node node)
    {
        foreach (var path in node._nodePaths)
        {
            if (path.Value._node1 == _standardMovement.CurrentNode || path.Value._node2 == _standardMovement.CurrentNode)
            {
                if (_standardMovement.CurrentMovementSpeed >= path.Value._movementPointCost)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /*
    private void StartBattleAgainst(StandardHealth value)
    {
        if (value)
        {
            // TODO Refactor to avoid GetComponent
            var attacker = GetComponent<NodeUnit>();
            var defender = value.GetComponent<NodeUnit>();
            //
            _currentBattle = Instantiate(_battlePrefab);
            _currentBattle.Initialise(attacker, defender);
        }
    }
    */

    private IEnumerator AttackAnimationCoroutine(StandardHealth otherUnitHealth, AttackType attackType)
    {
        Debug.Log($"{name} Attacked {otherUnitHealth.name} for {_attackPoints} Damage.");

        // Turn ParticleSystem Towards Target Unit
        _combatParticleSystem.transform.right = otherUnitHealth.transform.position - _combatParticleSystem.transform.position;

        var main = _combatParticleSystem.main;
        // Radian conversion stuff
        main.startRotation = -1 * (_combatParticleSystem.transform.localRotation.eulerAngles.z * Mathf.PI / 180f);

        // Play ParticleSystem for X Seconds
        _combatParticleSystem.Play();
        yield return new WaitForSeconds(2f);
        _combatParticleSystem.Stop();

        otherUnitHealth.TakeDamage(_attackPoints);

        // TODO Should attack cost equal path cost?
        if (attackType == AttackType.Instigation)
        {
            _standardMovement.CurrentMovementSpeed -= 1;
        }
        //
    }
}
