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


    #region Logistic Points
    [Header("Logistic Points")]
    [SerializeField] int _totalLogisticPoints;
    public int TotalLogisticPoints { get { return _totalLogisticPoints; } set { SetTotalLogisticPoints(value); } }
    internal static Action<int, int> TotalLogisticPointsChangedAction;
    private void SetTotalLogisticPoints(int value)
    {
        _totalLogisticPoints = value;

        TotalLogisticPointsChangedAction?.Invoke(CurrentLogisticPoints, TotalLogisticPoints);

    }

    [SerializeField] int _currentLogisticPoints;
    public int CurrentLogisticPoints { get { return _currentLogisticPoints; } set { SetCurrentLogisticPoints(value); } }
    internal static Action<int, int> CurrentLogisticPointsChangedAction;
    private void SetCurrentLogisticPoints(int value)
    {
        _currentLogisticPoints = value;
        CurrentLogisticPointsChangedAction?.Invoke(CurrentLogisticPoints, TotalLogisticPoints);
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
        DateManager.CurrentDayChangedAction += CurrentDayChanged;
        NodeUnit.UnitDestroyedAction += UnitDestroyed;
    }


    private void OnDisable()
    {
        DateManager.CurrentDayChangedAction -= CurrentDayChanged;
        NodeUnit.UnitDestroyedAction -= UnitDestroyed;
    }

    private void CurrentDayChanged(int turnNum)
    {
        TotalLogisticPoints++;
        CurrentLogisticPoints = TotalLogisticPoints;
    }
    private void UnitDestroyed(NodeUnit unit)
    {
        if (unit.Side == this)
        {
            _units.Remove(unit);
        }
    }

    protected void Start()
    {

    }
}