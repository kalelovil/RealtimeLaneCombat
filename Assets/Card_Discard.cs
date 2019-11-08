using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card_Discard : MonoBehaviour, IPointerClickHandler
{
    public List<CardBase> _discardedCardsList = new List<CardBase>();
    [SerializeField] internal List<CardBase> CardsShown { get; private set; } = new List<CardBase>();
    [SerializeField] RectTransform _cardDisplayArea;

    [SerializeField] Image _highlightImage;

    public void OnEnable()
    {
        CardBase.CardSelectedAction += CardSelected;
    }
    public void OnDisable()
    {
        CardBase.CardSelectedAction -= CardSelected;
    }
    private void CardSelected(CardBase card)
    {
        if (card)
        {
            // TODO Can be discarded
            if (true)
            {
                _highlightImage.gameObject.SetActive(true);
                return;
            }
        }
        _highlightImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerInputManager.Instance.DiscardClicked(this);
    }

    internal void AddCard(CardBase cardBase)
    {
        cardBase.transform.SetParent(transform, false);
        ((RectTransform)cardBase.transform).anchoredPosition = new Vector2(0f, 0f);
        ((RectTransform)cardBase.transform).anchorMin = new Vector2(0.5f, 0.5f);
        ((RectTransform)cardBase.transform).anchorMax = new Vector2(0.5f, 0.5f);
        _discardedCardsList.Add(cardBase);

        Card_Hand.CardDiscardedAction.Invoke(cardBase);
    }

    #region Show and Unshow Cards
    internal void ShowCards(int numToShow)
    {
        for (int i = 0; (i < numToShow || numToShow == -1) && i < _discardedCardsList.Count; i++)
        {
            var card = _discardedCardsList[i];
            ShowCard(card);
        }
    }
    private void ShowCard(CardBase cardBase)
    {
        cardBase.transform.SetParent(_cardDisplayArea, false);
        CardsShown.Add(cardBase);
    }
    internal void UnshowCards()
    {
        foreach (var cardBase in CardsShown.ToList())
        {
            UnshowCard(cardBase);
        }
    }
    private void UnshowCard(CardBase cardBase)
    {
        cardBase.transform.SetParent(transform, false);
        ((RectTransform)cardBase.transform).anchoredPosition = new Vector2(0f, 0f);
        ((RectTransform)cardBase.transform).anchorMin = new Vector2(0.5f, 0.5f);
        ((RectTransform)cardBase.transform).anchorMax = new Vector2(0.5f, 0.5f);

        CardsShown.Remove(cardBase);
    }
    #endregion
}
