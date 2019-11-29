using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    public static DateManager Instance { get; internal set; }

    #region Hour
    [Header("Hour")]
    [SerializeField] int _currentHourNum;
    public int CurrentHourNum { get { return _currentHourNum; } }
    internal static Action<int> CurrentHourChangedAction;
    #endregion

    #region Day
    [Header("Day")]
    [SerializeField] int _currentDayNum;
    public int CurrentDayNum { get { return _currentDayNum; } }
    internal static Action<int> CurrentDayChangedAction;
    #endregion

    static List<float> SPEED_TO_SECONDS_PER_DAY = new List<float>
    {
        5f,
        2.25f,
        1f,
        0.5f,
    };
    [SerializeField] int _speedIndex = 0;



    #region Sides
    [Header("Sides")]
    [SerializeField] List<AbstractPlayerManager> _sideList = new List<AbstractPlayerManager>();
    #endregion
    void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        CurrentHourChangedAction?.Invoke(0);
        CurrentDayChangedAction?.Invoke(1);
    }

    float _currentHourProgress = 0f;
    private void Update()
    {
        _currentHourProgress += Time.deltaTime;
        if (_currentHourProgress >= SPEED_TO_SECONDS_PER_DAY[_speedIndex])
        {
            _currentHourNum++;
            CurrentHourChangedAction?.Invoke(_currentHourNum);
            _currentHourProgress = 0f;
        }
    }
}
