using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerManager : AbstractPlayerManager
{

    public override LeftOrRight MapSide
    {
        get { return LeftOrRight.Right; }
    }
    public override Color Colour
    {
        get { return Color.red; }
    }

    protected override void HighlightUnit(NodeUnit unit)
    {
        //TODO Implement
    }

    protected override void SetSelectedUnit_OverrideMethod(NodeUnit value)
    {
        //TODO Implement
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
