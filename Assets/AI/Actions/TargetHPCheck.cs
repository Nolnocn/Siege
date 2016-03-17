using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class TargetHPCheck : RAINAction
{

	GameObject detected;
	Health targetHealthscript;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		GameObject detected = ai.WorkingMemory.GetItem ("enemy") as GameObject;
		targetHealthscript = detected.GetComponent<Health> ();

		if(targetHealthscript.hp <= 0f)
		{
			return ActionResult.FAILURE;
		}


        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}