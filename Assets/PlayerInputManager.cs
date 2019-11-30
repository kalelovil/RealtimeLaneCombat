using kalelovil.utility.pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    NodeUnit SelectedUnit { get { return HumanPlayerManager.Instance.SelectedUnit; } set { HumanPlayerManager.Instance.SelectedUnit = value; } }
    CardBase SelectedCard { get { return CardBase.Selected_Card; } }

    public static PlayerInputManager Instance { get; internal set; }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void UnitClicked(NodeUnit nodeUnit)
    {
        Debug.Log($"Unit Clicked: {nodeUnit}");

        if ((!CardBase.Selected_Card || !CardBase.Selected_Card.CanBePlayedOn(nodeUnit.Movement.CurrentNode))
            && nodeUnit.Side == HumanPlayerManager.Instance)
        {
            CardBase.Selected_Card = null;
            SelectedUnit = nodeUnit;
        }
        else if (nodeUnit.Movement)
        {
            NodeClicked(nodeUnit.Movement.CurrentNode);
        }
    }

    internal void CardClicked(CardBase card)
    {
        Debug.Log($"Card Clicked: {card}");

        SelectedUnit = null;
        CardBase.Selected_Card = card;
    }
    internal void DiscardClicked(Card_Discard cardDiscard)
    {
        Debug.Log($"Discard Clicked: {cardDiscard}");
        if (CardBase.Selected_Card)
        {
            CardBase.Selected_Card.Discard(cardDiscard);
            CardBase.Selected_Card = null;
        }
        else
        {
            if (cardDiscard.CardsShown.Count == 0)
            {
                cardDiscard.ShowCards(-1);
            }
            else
            {
                cardDiscard.UnshowCards();
            }
        }
    }

    internal void NodeClicked(Node clickedNode)
    {
        Debug.Log($"Node Clicked: {clickedNode}");
        if (SelectedUnit)
        {
            var path = Pathfinder.GetPathOfTypeForUnit
            (
                SelectedUnit.Movement.CurrentNode,
                clickedNode, Pathfinder.PathfindingType.Ground,
                SelectedUnit
            );
            if (path != null)
            {
                SelectedUnit.Movement.SetPath(path);
            }
        }
        else if (SelectedCard && SelectedCard.CanBePlayed() && SelectedCard.CanBePlayedOn(clickedNode))
        {
            SelectedCard.Play(clickedNode);
            CardBase.Selected_Card = null;
        }
    }

    /*
    internal IEnumerator UnitNodeActionCoroutine(Node targetNode)
    {
        if (
            targetNode.CurrentUnit
            && SelectedUnit._standardAttack 
            && SelectedUnit._standardAttack.CanInitiateAttack
            && targetNode.CurrentUnit._standardHealth
            && targetNode.CurrentUnit.Side != SelectedUnit.Side
            && SelectedUnit._standardAttack.NodeInRange(targetNode)
        ) {
            yield return SelectedUnit.StartCoroutine(SelectedUnit._standardAttack.AttackCoroutine(targetNode.CurrentUnit._standardHealth, StandardAttack.AttackType.Instigation));
            if (targetNode.CurrentUnit._standardHealth.CurrentHealthPoints > 0)
            {
                if (SelectedUnit._standardAttack.TriggersRetaliation && targetNode.CurrentUnit._standardAttack.NodeInRange(SelectedUnit._standardMovement.CurrentNode))
                {
                    yield return targetNode.CurrentUnit.StartCoroutine(targetNode.CurrentUnit._standardAttack.AttackCoroutine(SelectedUnit._standardHealth, StandardAttack.AttackType.Retaliation));
                }
            }
        }
        else if (SelectedUnit._standardMovement && !SelectedUnit._standardMovement.CurrentConnection)
        {
            //var path = SelectedUnit._standardMovement.GetPathToNode(node);
            var path = Pathfinder.GetPathOfTypeForUnit
            (
                SelectedUnit._standardMovement.CurrentNode, 
                targetNode, Pathfinder.PathfindingType.Ground, 
                SelectedUnit
            );
            if (path != null)
            {
                SelectedUnit._standardMovement.SetPath(path);
            }
        }
    }
    */
}