using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; internal set; }

    #region Turn Number
    [Header("Turn")]
    [Range(1, 1000)]
    [SerializeField] int _currentTurnNum;
    public int CurrentTurnNum { get { return _currentTurnNum; } set { SetCurrentTurnNum(value); } }
    internal static Action<int> CurrentTurnChangedAction;
    private void SetCurrentTurnNum(int value)
    {
        _currentTurnNum = value;
        CurrentTurnChangedAction?.Invoke(CurrentTurnNum);
    }
    #endregion

    #region Sides
    [Header("Sides")]
    [SerializeField] List<AbstractPlayerManager> _sideList = new List<AbstractPlayerManager>();

    [SerializeField] int _currentSideIndex;
    public AbstractPlayerManager CurrentSide { get { return _sideList[_currentSideIndex]; } }
    internal static Action<AbstractPlayerManager, AbstractPlayerManager> CurrentSideChangedAction;
    public void NextSide()
    {
        if (_currentSideIndex == _sideList.Count-1)
        {
            CurrentTurnNum++;
        }
        var prevSide = CurrentSide;
        _currentSideIndex = (_currentSideIndex + 1) % _sideList.Count;
        SideUpdated(prevSide, CurrentSide);
    }
    private void SideUpdated(AbstractPlayerManager prevSide, AbstractPlayerManager currentSide)
    {
        Debug.Log("Side Updated");
        CurrentSideChangedAction?.Invoke(prevSide, CurrentSide);
    }
    #endregion
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SideUpdated(CurrentSide, CurrentSide);
        CurrentTurnChangedAction?.Invoke(CurrentTurnNum);
    }
}
