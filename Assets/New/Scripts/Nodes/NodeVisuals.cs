using System;
using UnityEngine;
using UnityEngine.UI;

internal class NodeVisuals : MonoBehaviour
{
    [SerializeField] Image _borderImage;

    [SerializeField] Color32 _notAvailableImageColor;
    [SerializeField] Color32 _availableImageColor;

    [SerializeField] Image _image;

    [SerializeField] Image _controllerHighlightImage;

    internal void SetSideVisualState(AbstractPlayerManager controllingSide)
    {
        _controllerHighlightImage.enabled = (controllingSide);
        if (controllingSide)
        {
            _controllerHighlightImage.color = controllingSide.Colour;
        }
    }

    internal void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    internal void SetPlayableOnVisualState(bool canBePlayedOn)
    {
        if (canBePlayedOn)
        {
            _image.color = _availableImageColor;
        }
        else if (!canBePlayedOn)
        {
            _image.color = _notAvailableImageColor;
        }
    }

    internal void SetMovableToVisualState(bool canBeMovedTo)
    {
        if (canBeMovedTo)
        {
            _image.color = _availableImageColor;
        }
        else if (!canBeMovedTo)
        {
            _image.color = _notAvailableImageColor;
        }
    }
}