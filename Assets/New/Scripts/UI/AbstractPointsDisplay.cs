using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractPointsDisplay : MonoBehaviour
{
    [SerializeField] Image _pointPrefab;
    [SerializeField] RectTransform _pointArea;
    [SerializeField] List<Image> _pointList = new List<Image>();

    public static AbstractPointsDisplay Instance { get; internal set; }

    private void Awake()
    {
        Instance = this;
    }
    public abstract void OnEnable();
    public abstract void OnDisable();

    protected void PointsChanged(int currentPoints, int totalPoints, bool isLocal)
    {
        if (isLocal)
        {
            // Clear Points
            foreach (var point in _pointList)
            {
                Destroy(point.gameObject);
            }
            _pointList.Clear();

            // Populate Points
            for (int i = 1; i <= totalPoints; i++)
            {
                var point = Instantiate(_pointPrefab, _pointArea);
                _pointList.Add(point);
                bool pointActive = currentPoints >= i;
                point.color = (pointActive) ? Color.yellow : Color.gray;
            }
        }
    }
}
