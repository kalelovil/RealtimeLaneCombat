using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    public static DateManager Instance { get; internal set; }

    #region Hour Number
    [Header("Hour")]
    [SerializeField] int _currentHourNum;
    public int CurrentHourNum { get { return _currentHourNum; } }

    internal static Action<int> CurrentHourChangedAction;
    internal static Action<int> CurrentDayChangedAction;
    #endregion

    #region Sides
    [Header("Sides")]
    [SerializeField] List<AbstractPlayerManager> _sideList = new List<AbstractPlayerManager>();
    #endregion
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentDayChangedAction?.Invoke(1);
    }

    private void FixedUpdate()
    {
        _currentHourNum++;
        CurrentHourChangedAction?.Invoke(_currentHourNum);
    }
}
