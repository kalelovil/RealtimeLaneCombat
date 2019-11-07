using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentTurn_Panel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    private void OnEnable()
    {
        TurnManager.CurrentTurnChangedAction += CurrentTurnChanged;
    }

    private void OnDisable()
    {
        TurnManager.CurrentTurnChangedAction -= CurrentTurnChanged;
    }

    private void CurrentTurnChanged(int currentTurn)
    {
        _text.text = $"{currentTurn}";
    }
}
