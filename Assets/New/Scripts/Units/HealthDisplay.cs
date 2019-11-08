using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class HealthDisplay : MonoBehaviour
{
    [SerializeField] Image _healthPointPrefab;
    [SerializeField] RectTransform _healthPointArea;
    [SerializeField] List<Image> _healthPointList = new List<Image>();
    [SerializeField] Color _pointColor;

    private void OnValidate()
    {
        _pointColor = _healthPointPrefab.color;
    }
    internal void ValueChanged(float currentHealth, float totalHealth)
    {
        // Clear Points
        foreach (var point in _healthPointList)
        {
            Destroy(point.gameObject);
        }
        _healthPointList.Clear();

        // Populate Points
        for (int i = 1; i <= totalHealth; i++)
        {
            var healthPoint = Instantiate(_healthPointPrefab, _healthPointArea);
            _healthPointList.Add(healthPoint);
            bool healthPointActive = currentHealth >= i;
            healthPoint.color = (healthPointActive) ? _pointColor : Color.gray;
        }
    }
}