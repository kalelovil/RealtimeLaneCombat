using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    #region Production
    [SerializeField] NodeUnit.UnitType _unitTypeProduced;

    [SerializeField] int _unitsPerDay;
    #endregion

    #region Time
    private void OnEnable()
    {
        DateManager.CurrentHourChangedAction += CurrentHourChanged;
        DateManager.CurrentDayChangedAction += CurrentDayChanged;
    }

    private void OnDisable()
    {
        DateManager.CurrentHourChangedAction -= CurrentHourChanged;
        DateManager.CurrentDayChangedAction -= CurrentDayChanged;
    }

    private void CurrentDayChanged(int dayNum)
    {
        throw new NotImplementedException();
    }

    private void CurrentHourChanged(int hourNum)
    {
        throw new NotImplementedException();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
