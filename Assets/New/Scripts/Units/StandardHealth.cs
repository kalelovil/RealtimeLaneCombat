using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NodeUnit))]
public class StandardHealth : UnitComponent
{
    [SerializeField] int _totalHealthPoints;
    public int TotalHealthPoints { get { return _totalHealthPoints; } set { SetTotalHealthPoints(value); } }
    private void SetTotalHealthPoints(int value)
    {
        _totalHealthPoints = Mathf.Max(0, value);
        if (_healthBar) _healthBar.UpdateBar(CurrentHealthPoints, TotalHealthPoints);
    }

    [SerializeField] private int _currentHealthPoints;
    public int CurrentHealthPoints { get { return _currentHealthPoints; } set { SetCurrentHealthPoints(value); } }
    private void SetCurrentHealthPoints(int value)
    {
        _currentHealthPoints = Mathf.Max(0, value);
        if (_currentHealthPoints == 0)
        {
            Destroy(NodeUnit.gameObject);
        }
        if (_healthBar) _healthBar.UpdateBar(CurrentHealthPoints, TotalHealthPoints);
    }

    internal List<HealthUpgradeCardAbility> _upgradeList = new List<HealthUpgradeCardAbility>();
    internal void AddUpgrade(HealthUpgradeCardAbility upgradeCardAbility)
    {
        _upgradeList.Add(upgradeCardAbility);
        _totalHealthPoints += upgradeCardAbility._effectValue;
        CurrentHealthPoints += upgradeCardAbility._effectValue;
    }

    #region Visual
    [Header("Visual")] 
    [SerializeField] SimpleHealthBar _healthBar;
    #endregion

    internal void TakeDamage(int attackPoints)
    {
        CurrentHealthPoints -= attackPoints;
        if (CurrentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TotalHealthPoints = _totalHealthPoints;
        CurrentHealthPoints = TotalHealthPoints;
    }
}
