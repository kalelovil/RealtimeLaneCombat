using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryAttack : StandardAttack
{
    internal override bool NodeInRange(Node node)
    {
        foreach (var path in node._nodeConnections)
        {
            if (base.NodeInRange(path.Value._node1) || base.NodeInRange(path.Value._node2))
            {
                return true;
            }
        }
        return false;
    }

    internal override bool TriggersRetaliation => false;
}
