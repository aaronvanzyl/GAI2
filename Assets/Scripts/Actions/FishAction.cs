using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAction : SimpleItemAction
{
    public FishAction(int actorID, int giverID, int itemID) : base(actorID, giverID, itemID)
    {
    }

    public override Action Clone()
    {
        return new FishAction(actorID, giverID, itemID);
    }


}
