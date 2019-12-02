using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUnit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] AbstractPlayerManager _side;
    public AbstractPlayerManager Side { get { return _side; } }

    internal void AddUpgrade(UpgradeCardAbility upgradeCardAbility)
    {
        switch (upgradeCardAbility.EffectStatType)
        {
            case UpgradeCardAbility.StatType.Health:
                _health.AddUpgrade((HealthUpgradeCardAbility)upgradeCardAbility);
                break;
            case UpgradeCardAbility.StatType.Attack:
                _attack.AddUpgrade((AttackUpgradeCardAbility)upgradeCardAbility);
                break;
            default:
                break;
        }
    }

    public enum UnitType
    {
        Infantry = 1,
        Artillery = 2,
        Tanks = 3,
    }
    public UnitType _unitType;

    [Header("Attack")]
    [SerializeField] private StandardAttack _attack;
    internal StandardAttack Attack => _attack;
    [Space(5)]

    [Header("Health")]
    [SerializeField] private StandardHealth _health;
    internal StandardHealth Health => _health;
    [Space(5)]


    [Header("Movement")]
    [SerializeField] private StandardMovement _movement;
    internal StandardMovement Movement => _movement;
    [Space(5)]

    [Header("Visual")]
    #region Visual
    [SerializeField] Outline _outline;

    [SerializeField] bool _highlighted;
    public bool Highlighted { get { return _highlighted; } internal set { SetHighlighted(value); } }

    private void SetHighlighted(bool value)
    {
        _highlighted = value;
        _outline.enabled = Highlighted;
    }
    #endregion

    internal void Initialise(Node node)
    {
        AbstractPlayerManager side = HumanPlayerManager.Instance;
        _side = side;
        if (_movement)
        {
            _movement.Initialise(side, node);
        }
    }
    private void Start()
    {
        Side._units.Add(this);
    }

    public static Action<NodeUnit> UnitDestroyedAction;
    private void OnDestroy()
    {
        UnitDestroyedAction?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerInputManager.Instance.UnitClicked(this);
    }
}