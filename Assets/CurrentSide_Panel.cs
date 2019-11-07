using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentSide_Panel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    private void OnEnable()
    {
        TurnManager.CurrentSideChangedAction += CurrentSideChanged;
    }

    private void OnDisable()
    {
        TurnManager.CurrentSideChangedAction -= CurrentSideChanged;
    }

    private void CurrentSideChanged(AbstractPlayerManager prevSide, AbstractPlayerManager currentSide)
    {
        _text.text = $"{currentSide.name}";
    }
}
