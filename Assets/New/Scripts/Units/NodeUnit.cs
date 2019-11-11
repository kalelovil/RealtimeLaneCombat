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
                _standardHealth.AddUpgrade((HealthUpgradeCardAbility)upgradeCardAbility);
                break;
            case UpgradeCardAbility.StatType.Attack:
                _standardAttack.AddUpgrade((AttackUpgradeCardAbility)upgradeCardAbility);
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
    [SerializeField] internal StandardAttack _standardAttack;
    [Space(5)]

    [Header("Health")]
    [SerializeField] internal StandardHealth _standardHealth;
    [Space(5)]


    [Header("Movement")]
    [SerializeField] internal StandardMovement _standardMovement;
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
        if (_standardMovement)
        {
            _standardMovement.Initialise(side, node);
        }
    }
    private void Start()
    {
        Side._units.Add(this);
    }

    private void OnDestroy()
    {
        Side._units.Remove(this);
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