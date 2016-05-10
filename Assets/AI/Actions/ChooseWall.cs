using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Entities.Aspects;
using RAIN.Core;

[RAINAction]
public class ChooseWall : RAINAction
{
    IList<RAINAspect> wallAspects;
    GameObject wall;
    WallController wallScript;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        wallAspects = ai.WorkingMemory.GetItem<IList<RAINAspect>>("walls");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        for( int i = 0; i < wallAspects.Count; i++ )
        {
            wall = wallAspects[i].Entity.Form.gameObject;
            wallScript = wall.GetComponent<WallController>();

            //Debug.Log("wall 2 health: " + wallScript.health);
            if (wallScript.health < 100)
            {
                ai.WorkingMemory.SetItem("wallPosition", wall.transform.position);
                ai.WorkingMemory.SetItem("wallSeen", wall);
                ai.WorkingMemory.SetItem("wall", wall);
                return ActionResult.SUCCESS;
            }
        }

        return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}