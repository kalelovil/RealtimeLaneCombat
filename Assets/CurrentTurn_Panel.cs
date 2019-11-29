using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentTurn_Panel : MonoBehaviour
{
    [Header("Hour")]
    [SerializeField] TextMeshProUGUI _hourValueText;

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

    private void CurrentHourChanged(int currentHour)
    {
        _hourValueText.text = $"{currentHour}";
    }
    private void CurrentDayChanged(int currentDay)
    {
        // TODO Implement
    }
}
