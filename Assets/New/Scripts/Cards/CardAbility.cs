using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAbility : MonoBehaviour
{
    CardBase _cardBase;
    internal CardBase CardBase { get { return _cardBase; } set { _cardBase = value; }}

    public abstract bool CanActivateOn(Node node);
    public abstract void ActivateOn(Node node);

}
