using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentTurn_Panel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    private void OnEnable()
    {
        DateManager.CurrentDayChangedAction += CurrentTurnChanged;
    }

    private void OnDisable()
    {
        DateManager.CurrentDayChangedAction -= CurrentTurnChanged;
    }

    private void CurrentTurnChanged(int currentTurn)
    {
        _text.text = $"{currentTurn}";
    }
}
