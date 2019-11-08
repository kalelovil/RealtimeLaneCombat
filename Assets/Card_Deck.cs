using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Deck : MonoBehaviour
{
    [SerializeField] Stack<CardBase> _cardsInDeckStack = new Stack<CardBase>();

    [SerializeField] int _startingDeckSize;

    [SerializeField] Card_Hand _cardHand;

    [SerializeField] DeckGenerator _deckGenerator;

    private void Awake()
    {
        _cardsInDeckStack = _deckGenerator.GenerateNewDeck(_startingDeckSize, transform);
    }

    private void OnEnable()
    {
        DateManager.CurrentDayChangedAction += CurrentDayChanged;
    }

    private void OnDisable()
    {
        DateManager.CurrentDayChangedAction -= CurrentDayChanged;
    }

    private void CurrentDayChanged(int dayNum)
    {
        if (dayNum == 1)
        {
            while (_cardHand.CanAddCard)
            {
                DealCard();
            }
        }
        else if (_cardHand.CanAddCard)
        {
            DealCard();
        }
    }

    public void DealCard()
    {
        var card = _cardsInDeckStack.Pop();
        _cardHand.AddCard(card);
    }
}
