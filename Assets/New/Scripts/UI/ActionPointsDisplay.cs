using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsDisplay : AbstractPointsDisplay
{
    public override void OnEnable()
    {
        AbstractPlayerManager.CurrentActionPointsChangedAction += PointsChanged;
        AbstractPlayerManager.TotalActionPointsChangedAction += PointsChanged;
    }
    public override void OnDisable()
    {
        AbstractPlayerManager.CurrentActionPointsChangedAction -= PointsChanged;
        AbstractPlayerManager.TotalActionPointsChangedAction -= PointsChanged;
    }
}
