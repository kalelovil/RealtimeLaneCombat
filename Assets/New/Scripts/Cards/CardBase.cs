using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(CardBase))]
public class CardBase : MonoBehaviour, IPointerClickHandler
{
    // TODO separate data into serialisedObject
    [SerializeField] string _name;
    internal string Name { get { return _name; } }
    [SerializeField] TextMeshProUGUI _nameText;

    [SerializeField] int _cost;
    internal int Cost { get { return _cost; } }
    [SerializeField] TextMeshProUGUI _costText;

    #region Playable
    public enum Playable
    {
        Unplayable = 0,
        Playable = 1,
    }
    [SerializeField] Playable _isPlayable;
    public Playable IsPlayable { get { return _isPlayable; } set { SetIsPlayable(value); } }
    private void SetIsPlayable(Playable value)
    {
        _isPlayable = value;
        _cardVisuals.SetIsPlayableState(value);
    }
    #endregion

    #region State
    public enum CardState
    {
        InHand = 0,
        //InPlay = 1,
        Discarded = 2,
    }
    [SerializeField] CardState _currentState;
    public CardState CurrentState { get { return _currentState; } set { SetCurrentState(value); } }
    private void SetCurrentState(CardState value)
    {
        _currentState = value;

        switch (CurrentState)
        {
            case CardState.InHand:
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                IsPlayable = (Playable)Convert.ToInt32(CanBePlayed());
                break;
                /*
            case CardState.InPlay:
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                IsPlayable = (Playable)Convert.ToInt32(CanBePlayedBy(TurnManager.Instance.CurrentSide));
                break;
                */
            case CardState.Discarded:
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                IsPlayable = Playable.Unplayable;
                break;
            default:
                break;
        }

        _cardVisuals.SetCardState(value);
    }
    #endregion

    [SerializeField] CardAbility _cardAbility;

    [SerializeField] CardVisuals _cardVisuals;




    void Awake()
    {
        _cardVisuals = GetComponent<CardVisuals>();
    }

    public void OnValidate()
    {
        _nameText.text = Name;
        _costText.text = $"{Cost}";

        CardAbility ability = GetComponent<CardAbility>();
        if (ability)
        {
            _cardAbility = ability;
            ability.CardBase = this;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerInputManager.Instance.CardClicked(this);
    }

    internal void Discard(Card_Discard cardDiscard)
    {
        CurrentState = CardState.Discarded;
        cardDiscard.AddCard(this);
    }

    internal void Play(Node node)
    {
        if (CanBePlayedOn(node))
        {
            HumanPlayerManager.Instance.CurrentLogisticPoints -= Cost;
            _cardAbility.ActivateOn(node);
            Destroy(gameObject);
            Card_Hand.CardPlayedAction.Invoke(this);
        }
    }

    internal bool CanBePlayed()
    {
        bool canBePlayed = (Cost <= HumanPlayerManager.Instance.CurrentLogisticPoints);
        return canBePlayed;
    }
    internal bool CanBePlayedOn(Node node)
    {
        // TODO Complete
        if (_cardAbility && _cardAbility.CanActivateOn(node))
        {
            return true;
        }
        return false;
    }


    // TODO Refactor into a card manager class?
    static CardBase _selectedCard;
    internal static CardBase Selected_Card { get { return _selectedCard; } set { CardBase.SetSelectedCard(value); } }
    internal static Action<CardBase> CardSelectedAction;
    static void SetSelectedCard(CardBase value)
    {
        if (Selected_Card) Selected_Card.IsSelected = Selected.Unselected;
        _selectedCard = value;
        CardSelectedAction.Invoke(_selectedCard);
        if (Selected_Card) Selected_Card.IsSelected = Selected.Selected;
    }

    public enum Selected
    {
        Unselected = 0,
        Selected = 1,
    }
    [SerializeField] Selected _isSelected;
    public Selected IsSelected { get { return _isSelected; } private set { SetIsSelected(value); } }
    private void SetIsSelected(Selected value)
    {
        _isSelected = value;
        _cardVisuals.SetIsSelectedState(IsSelected);
    }
}