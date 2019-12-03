﻿using System;
using System.Collections.Generic;
using UnityEngine;

internal class DeckGenerator : MonoBehaviour
{
    [SerializeField] List<CardBase> _cardPrefabList;

    // Temp
    [SerializeField] List<CardBase> _onTopOfDeck;
    //

    internal Stack<CardBase> GenerateNewDeck(int numOfCards, Transform deckArea)
    {
        Stack<CardBase> newDeck = new Stack<CardBase>();

        for (int i = 0; i < numOfCards; i++)
        {
            int cardPrefabIndex = UnityEngine.Random.Range(0, _cardPrefabList.Count);
            var cardPrefabToAdd = _cardPrefabList[cardPrefabIndex];

            var newCard = Instantiate(cardPrefabToAdd, deckArea);
            newDeck.Push(newCard);
        }
        foreach (var cardOnTop in _onTopOfDeck)
        {
            var newCard = Instantiate(cardOnTop, deckArea);
            newDeck.Push(newCard);
        }

        return newDeck;
    }
}