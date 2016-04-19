using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GatherResources : RAINAction
{

    GameObject detected;
    Resource resScript;
    Builder builderScript;
    int gatherRate;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        builderScript = ai.Body.GetComponent<Builder>();
        gatherRate = builderScript.gatherRate;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        GameObject detected = ai.WorkingMemory.GetItem ("resource") as GameObject;
        resScript = detected.GetComponent<Resource> ();

		builderScript.RecieveResource( resScript.GatherResource( gatherRate ) );
        //Debug.Log(builderScript);

        //if(resScript.health <= 0 || resScript.health == 100)
        //{
        //    return ActionResult.FAILURE;
        //}

        //ai.WorkingMemory.SetItem("wallHealth", resScript.health);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}