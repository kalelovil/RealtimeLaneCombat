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
        Debug.Log($"Battle {this} Set Attacker {value}");
        _attacker = value;
        ParticipantsChanged();
    }
    #endregion

    #region Defender
    [SerializeField] NodeUnit _defender;
    internal NodeUnit Defender { get { return _defender; } set { SetDefender(value); } }
    void SetDefender(NodeUnit value)
    {
        Debug.Log($"Battle {this} Set Defender {value}");
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

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void ParticipantsChanged()
    {
        if (Attacker && Defender)
        {
            Debug.Log("Attacker And Defender Set");
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }
        else
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
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