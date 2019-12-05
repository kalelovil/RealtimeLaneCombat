using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyPoint : NodeFeature, IHourStep
{
    public override NodeFeatureType FeatureType => NodeFeatureType.SupplyPoint;

    private void OnEnable()
    {
        DateManager.CurrentHourChangedAction += HourStep;
    }

    private void OnDisable()
    {
        DateManager.CurrentHourChangedAction -= HourStep;
    }

    public void HourStep(int hourNum)
    {
        //Node node = GetComponent<Node>();
        //node.ControllingSide.TotalLogisticPoints += 1;
    }
}
