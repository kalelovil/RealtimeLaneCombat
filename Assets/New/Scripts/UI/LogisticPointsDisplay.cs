using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogisticPointsDisplay : AbstractPointsDisplay
{
    public override void OnEnable()
    {
        AbstractPlayerManager.CurrentLogisticPointsChangedAction += PointsChanged;
        AbstractPlayerManager.TotalLogisticPointsChangedAction += PointsChanged;
    }
    public override void OnDisable()
    {
        AbstractPlayerManager.CurrentLogisticPointsChangedAction -= PointsChanged;
        AbstractPlayerManager.TotalLogisticPointsChangedAction -= PointsChanged;
    }
}
