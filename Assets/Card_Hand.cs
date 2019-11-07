using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Card_Hand : MonoBehaviour
{
    [SerializeField] List<CardBase> _cardsInHandList = new List<CardBase>();
    [SerializeField] int _maxHandSize;

    public static Action<CardBase> CardPlayedAction;
    public static Action<CardBase> CardDiscardedAction;

    public void OnEnable()
    {
        AbstractPlayerManager.CurrentLogisticPointsChangedAction += LogisticPointsChanged;
        CardPlayedAction += RemoveCard;
        CardDiscardedAction += RemoveCard;
    }

    public void OnDisable()
    {
        AbstractPlayerManager.CurrentLogisticPointsChangedAction -= LogisticPointsChanged;
        CardPlayedAction -= RemoveCard;
        CardDiscardedAction -= RemoveCard;
    }

    public bool CanAddCard => _cardsInHandList.Count < _maxHandSize;

    internal void AddCard(CardBase card)
    {
        card.transform.SetParent(transform);
        _cardsInHandList.Add(card);
    }

    private void RemoveCard(CardBase card)
    {
        if (_cardsInHandList.Contains(card))
        {
            _cardsInHandList.Remove(card);
        }
    }

    private void LogisticPointsChanged(int current, int total)
    {
        foreach (var card in _cardsInHandList)
        {
            card.IsPlayable = (CardBase.Playable)Convert.ToInt32(card.CanBePlayedBy(TurnManager.Instance.CurrentSide));
        }
    }

    private void OnValidate()
    {
        _cardsInHandList = transform.GetComponentsInChildren<CardBase>().ToList();
    }
}
