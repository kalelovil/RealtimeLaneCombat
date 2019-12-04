﻿using System;
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
    public float TimeOfLastHourUpdate = 0f;
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
        2f,
        1f,
        0.5f,
        0.25f,
        0.1f,
    };
    [SerializeField] int _speedIndex = 1;
    internal float CurrentSecondsPerDay=> SPEED_TO_SECONDS_PER_DAY[_speedIndex];



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
        HandleInputs();

        _currentHourProgress += Time.deltaTime;
        if (_currentHourProgress >= SPEED_TO_SECONDS_PER_DAY[_speedIndex])
        {
            _currentHourNum++;
            TimeOfLastHourUpdate = Time.time;
            CurrentHourChangedAction?.Invoke(_currentHourNum);
            _currentHourProgress = 0f;
        }
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
        {
            if (_speedIndex < SPEED_TO_SECONDS_PER_DAY.Count - 1) _speedIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            if (_speedIndex > 0) _speedIndex--;
        }
    }
}
