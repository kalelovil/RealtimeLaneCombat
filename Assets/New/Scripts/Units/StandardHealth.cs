using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NodeUnit))]
public class StandardHealth : MonoBehaviour
{
    [SerializeField] int _totalHealthPoints;
    public int TotalHealthPoints { get { return _totalHealthPoints; } }
    [SerializeField] private int _currentHealthPoints;
    public int CurrentHealthPoints { get { return _currentHealthPoints; } set { SetCurrentNovementPoints(value); } }
    private void SetCurrentNovementPoints(int value)
    {
        _currentHealthPoints = Mathf.Max(0, value);
        if (_healthDisplay) _healthDisplay.PointsChanged(CurrentHealthPoints, TotalHealthPoints);
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
    [SerializeField] Transform _healthDisplayArea;
    [SerializeField] HealthDisplay _healthDisplayPrefab;
    [SerializeField] HealthDisplay _healthDisplay;
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
        if (!_healthDisplay)
        {
            _healthDisplay = Instantiate(_healthDisplayPrefab, _healthDisplayArea);
        }
        CurrentHealthPoints = TotalHealthPoints;
    }
}
