using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerManager : MonoBehaviour
{
    public enum LeftOrRight
    {
        Left,
        Right,
    }
    public abstract LeftOrRight MapSide { get; }
    public abstract Color Colour { get; }

    #region Action Points
    [Header("Action Points")]
    [SerializeField] int _totalActionPoints;
    public int TotalActionPoints { get { return _totalActionPoints; } set { SetTotalActionPoints(value); } }
    internal static Action<int, int> TotalActionPointsChangedAction;
    private void SetTotalActionPoints(int value)
    {
        _totalActionPoints = value;
        if (TurnManager.Instance.CurrentSide == this)
        {
            TotalActionPointsChangedAction?.Invoke(CurrentActionPoints, TotalActionPoints);
        }
    }

    [SerializeField] int _currentActionPoints;
    public int CurrentActionPoints { get { return _currentActionPoints; } set { SetCurrentActionPoints(value); } }
    internal static Action<int, int> CurrentActionPointsChangedAction;
    private void SetCurrentActionPoints(int value)
    {
        _currentActionPoints = value;
        if (TurnManager.Instance.CurrentSide == this)
        {
            CurrentActionPointsChangedAction?.Invoke(CurrentActionPoints, TotalActionPoints);
        }
    }
    #endregion

    #region Logistic Points
    [Header("Logistic Points")]
    [SerializeField] int _totalLogisticPoints;
    public int TotalLogisticPoints { get { return _totalLogisticPoints; } set { SetTotalLogisticPoints(value); } }
    internal static Action<int, int> TotalLogisticPointsChangedAction;
    private void SetTotalLogisticPoints(int value)
    {
        _totalLogisticPoints = value;
        if (TurnManager.Instance.CurrentSide == this)
        {
            TotalLogisticPointsChangedAction?.Invoke(CurrentLogisticPoints, TotalLogisticPoints);
        }
    }

    [SerializeField] int _currentLogisticPoints;
    public int CurrentLogisticPoints { get { return _currentLogisticPoints; } set { SetCurrentLogisticPoints(value); } }
    internal static Action<int, int> CurrentLogisticPointsChangedAction;
    private void SetCurrentLogisticPoints(int value)
    {
        _currentLogisticPoints = value;
        if (TurnManager.Instance.CurrentSide == this)
        {
            CurrentLogisticPointsChangedAction?.Invoke(CurrentLogisticPoints, TotalLogisticPoints);
        }
    }
    #endregion


    #region Units
    [Header("Units")]
    [SerializeField] internal List<NodeUnit> _units = new List<NodeUnit>();
    [SerializeField] protected NodeUnit _selectedUnit;
    public NodeUnit SelectedUnit { get { return _selectedUnit; } set { SetSelectedUnit(value); } }
    public static Action<NodeUnit> UnitSelectedAction;
    private void SetSelectedUnit(NodeUnit value)
    {
        SetSelectedUnit_BaseMethod(value);
        SetSelectedUnit_OverrideMethod(value);
    }
    private void SetSelectedUnit_BaseMethod(NodeUnit value)
    {
        if (SelectedUnit) SelectedUnit.Highlighted = false;
        _selectedUnit = value;
        UnitSelectedAction.Invoke(SelectedUnit);
        if (SelectedUnit)
        {
            SelectedUnit.Highlighted = true;
            CameraManager.Instance.FocusOn(SelectedUnit);
        }
    }

    protected abstract void HighlightUnit(NodeUnit unit);

    protected abstract void SetSelectedUnit_OverrideMethod(NodeUnit value);
    #endregion

    private void OnEnable()
    {
        TurnManager.CurrentSideChangedAction += CurrentSideChanged;
        TurnManager.CurrentTurnChangedAction += CurrentTurnChanged;
    }
    private void OnDisable()
    {
        TurnManager.CurrentSideChangedAction -= CurrentSideChanged;
        TurnManager.CurrentTurnChangedAction -= CurrentTurnChanged;
    }
    private void CurrentTurnChanged(int turnNum)
    {
        CurrentActionPoints = TotalActionPoints;

        TotalLogisticPoints++;
        CurrentLogisticPoints = TotalLogisticPoints;
    }
    private void CurrentSideChanged(AbstractPlayerManager prevSide, AbstractPlayerManager currentSide)
    {
        prevSide.SelectedUnit = null;
    }

    protected void Start()
    {
        CurrentActionPoints = TotalActionPoints;
    }
}