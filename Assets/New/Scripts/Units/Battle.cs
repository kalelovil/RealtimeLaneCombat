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
    CanvasGroup _canvasGroup;
    [SerializeField] 
    TextMeshProUGUI _percentageValueText;
    #endregion

    private void Start()
    {

    }

    private void ParticipantsChanged()
    {
        if (Attacker && Defender)
        {
            Debug.Log("Attacker And Defender Set");
            if (_canvasGroup.alpha == 0f)
            {
                _canvasGroup.alpha = 1f;
            }
        }
        else
        {
            if (_canvasGroup.alpha == 1f)
            {
                _canvasGroup.alpha = 0f;
            }
        }
    }

    List<NodeUnit> _attackerSupport = new List<NodeUnit>(), _defenderSupport = new List<NodeUnit>();

    private void OnEnable()
    {
        DateManager.CurrentHourChangedAction += HourStep;
        NodeUnit.UnitDestroyedAction += UnitDestroyed;
    }

    private void OnDisable()
    {
        DateManager.CurrentHourChangedAction -= HourStep;
        NodeUnit.UnitDestroyedAction -= UnitDestroyed;
    }
    private void UnitDestroyed(NodeUnit unit)
    {
        if (Attacker == unit)
        {
            Attacker = null;
        }
        else if (Defender == unit)
        {
            Defender = null;
        }
        else if (_attackerSupport.Contains(unit))
        {
            _attackerSupport.Remove(unit);
        }
        else if (_defenderSupport.Contains(unit))
        {
            _defenderSupport.Remove(unit);
        }
    }

    private void HourStep(int hourNum)
    {
        if (_canvasGroup.alpha == 1f)
        {
            Attacker.Attack.DoDamage(Defender);
            if (Defender != null)
            {
                Defender.Attack.DoDamage(Attacker);
            }
        }
    }
}