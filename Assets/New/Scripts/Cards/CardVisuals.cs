using System;
using UnityEngine;
using UnityEngine.UI;

internal class CardVisuals : MonoBehaviour
{
    #region IsPlayable
    [SerializeField] CanvasGroup _backgroundImage;
    internal void SetIsPlayableState(CardBase.Playable value)
    {
        switch (value)
        {
            case CardBase.Playable.Unplayable:
                SetUnplayableVisualState();
                break;
            case CardBase.Playable.Playable:
                SetPlayableVisualState();
                break;
            default:
                break;
        }
    }

    private void SetUnplayableVisualState()
    {
        _backgroundImage.alpha = 0.5f;
    }

    private void SetPlayableVisualState()
    {
        _backgroundImage.alpha = 1f;
    }
    #endregion


    #region IsSelected
    [SerializeField] Image _highlightImage;

    [SerializeField] Vector2 _selectedScale;
    Vector2 _unSelectedScale;
    internal void SetIsSelectedState(CardBase.Selected value)
    {
        switch (value)
        {
            case CardBase.Selected.Unselected:
                SetUnselectedVisualState();
                break;
            case CardBase.Selected.Selected:
                SetSelectedVisualState();
                break;
            default:
                break;
        }
    }

    private void SetSelectedVisualState()
    {
        // Increase Size If Selected
        transform.localScale = _selectedScale;
        // Turn On Outline Image If Selected
        _highlightImage.gameObject.SetActive(true);
    }

    private void SetUnselectedVisualState()
    {
        // Increase Size If Selected
        transform.localScale = _unSelectedScale;
        // Turn On Outline Image If Selected
        _highlightImage.gameObject.SetActive(false);
    }
    #endregion

    #region IsDiscarded
    [SerializeField] Image _discardedImage;
    internal void SetCardState(CardBase.CardState value)
    {
        switch (value)
        {
            case CardBase.CardState.InHand:
                SetInHandVisualState();
                break;
                /*
            case CardBase.CardState.InPlay:
                SetInHandOrPlayVisualState();
                break;
                */
            case CardBase.CardState.Discarded:
                SetDiscardedVisualState();
                break;
            default:
                break;
        }
    }
    private void SetInHandVisualState()
    {
        _discardedImage.gameObject.SetActive(false);
    }
    private void SetDiscardedVisualState()
    {
        _discardedImage.gameObject.SetActive(true);
    }
    #endregion

    void Start()
    {
        _unSelectedScale = transform.localScale;
    }
}