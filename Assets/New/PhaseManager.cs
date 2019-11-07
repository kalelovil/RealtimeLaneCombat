using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PhaseManager : MonoBehaviour
{
    public enum Phase
    {
        Card,
        Action
    }
    public Dictionary<Phase, float> _phaseTimeMap = new Dictionary<Phase, float>()
    {
        {  Phase.Card, 1f },
        {  Phase.Action, 3f },
    };
    [SerializeField] Phase _currentPhase;
    [SerializeField] float _currentPhaseTime;
    public Phase CurrentPhase { get { return _currentPhase; } set { SetCurrentPhase(value); } }
    private void SetCurrentPhase(Phase value)
    {
        _currentPhase = value;
        _currentPhaseTime = 0f;
        _phaseChangedAction.Invoke();
    }

    public float PhaseFraction { get { return GetPhaseFraction(); } }
    private float GetPhaseFraction()
    {
        return _currentPhaseTime / _phaseTimeMap[_currentPhase];
    }

    internal Action _phaseChangedAction;

    public static PhaseManager Instance;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _currentPhaseTime += Time.deltaTime;
        if (_currentPhaseTime > _phaseTimeMap[CurrentPhase])
        {
            int phaseNum = ((int)CurrentPhase + 1) % Enum.GetValues(typeof(Phase)).Length;
            CurrentPhase = (Phase)phaseNum;
        }
    }
}