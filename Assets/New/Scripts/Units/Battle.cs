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
    [Range(0, 1)]
    [SerializeField] float _progressFraction;
    internal float ProgressFraction => _progressFraction;

    public bool Active { get { return Defender && Attacker; } }
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

    List<ArtilleryAttack> _attackerSupport = new List<ArtilleryAttack>();
    internal void AddAttackerSupportUnit(ArtilleryAttack artilleryAttack)
    {
        _attackerSupport.Add(artilleryAttack);
    }

    List<ArtilleryAttack>_defenderSupport = new List<ArtilleryAttack>();

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
        if (unit.Attack && unit.Attack is ArtilleryAttack)
        {
            if (_attackerSupport.Contains((ArtilleryAttack)unit.Attack))
            {
                _attackerSupport.Remove((ArtilleryAttack)unit.Attack);
            }
            else if (_defenderSupport.Contains((ArtilleryAttack)unit.Attack))
            {
                _defenderSupport.Remove(((ArtilleryAttack)unit.Attack));
            }
        }
        else
        {
            if (Attacker == unit)
            {
                Attacker = null;
            }
            else if (Defender == unit)
            {
                Defender = null;
            }
        }
    }

    private void HourStep(int hourNum)
    {
        if (Active)
        {
            Attacker.Attack.DoDamage(Defender);
            foreach (var attackSupport in _attackerSupport)
            {
                attackSupport.DoDamage(Defender);
            }

            if (Defender != null)
            {
                Defender.Attack.DoDamage(Attacker);
                UpdatePercentage();
            }
        }
    }

    private void UpdatePercentage()
    {
        float attackerStrength = CalculateAttackerStrength();
        float defenderStrength = CalculateDefenderStrength();

        float attackerFraction = attackerStrength / defenderStrength;
        float defenderFraction = defenderStrength / attackerStrength;

        float percentage = 50f;
        if (attackerFraction < 1f)
        {
            percentage = (attackerFraction * 50f);
        }
        else
        {
            percentage = 100f - (defenderFraction * 50f);
        }
        _progressFraction = percentage / 100f;
        _percentageValueText.text = $"{percentage:0}";
    }

    float CalculateAttackerStrength()
    {
        return Attacker.Health.CurrentHealthPoints;
    }
    float CalculateDefenderStrength()
    {
        return Defender.Health.CurrentHealthPoints;
    }
}