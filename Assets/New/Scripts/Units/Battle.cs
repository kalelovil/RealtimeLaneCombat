using System;
using System.Collections.Generic;
using UnityEngine;

internal class Battle : MonoBehaviour
{
    #region Attacker;
    [SerializeField] NodeUnit _attacker;
    internal NodeUnit Attacker { get { return _attacker; } set { SetAttacker(value); } }
    void SetAttacker(NodeUnit value)
    {
        _attacker = value;
        ParticipantsChanged();
    }
    #endregion

    #region Defender;
    [SerializeField] NodeUnit _defender;
    internal NodeUnit Defender { get { return _defender; } set { SetDefender(value); } }
    void SetDefender(NodeUnit value)
    {
        _defender = value;
        ParticipantsChanged();
    }
    #endregion

    private void ParticipantsChanged()
    {
        if (Attacker && Defender)
        {
            if (!enabled) enabled = true;
        }
        else
        {
            if (enabled) enabled = false;
        }
    }

    List<NodeUnit> _attackerSupport = new List<NodeUnit>(), _defenderSupport = new List<NodeUnit>();

    private void OnEnable()
    {
        DateManager.CurrentHourChangedAction += HourStep;
    }
    private void OnDisable()
    {
        DateManager.CurrentHourChangedAction -= HourStep;
    }

    private void Initialise()
    {
        _attackerUnit = attacker;
        _defenderUnit = defender;
    }

    private void HourStep(int hourNum)
    {
        _attackerUnit._standardAttack.DoDamage(_defenderUnit);
        if (_defenderUnit != null)
        {
            _defenderUnit._standardAttack.DoDamage(_attackerUnit);
        }
    }
}