using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Battle : MonoBehaviour
{
    #region Attacker
    [SerializeField] NodeUnit _attacker;
    internal NodeUnit Attacker { get { return _attacker; } set { SetAttacker(value); } }
    void SetAttacker(NodeUnit value)
    {
        _attacker = value;
        ParticipantsChanged();
    }
    #endregion

    #region Defender
    [SerializeField] NodeUnit _defender;
    internal NodeUnit Defender { get { return _defender; } set { SetDefender(value); } }
    void SetDefender(NodeUnit value)
    {
        _defender = value;
        ParticipantsChanged();
    }
    #endregion

    #region Progress
    [Range(0, 100)]
    [SerializeField] float _progress;
    internal float Progress => _progress;
    #endregion

    #region Visual
    [Header("Visual")]
    [SerializeField] 
    TextMeshProUGUI _percentageValueText;
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

    private void HourStep(int hourNum)
    {
        Attacker._standardAttack.DoDamage(Defender);
        if (Defender != null)
        {
            Defender._standardAttack.DoDamage(Attacker);
        }
    }
}