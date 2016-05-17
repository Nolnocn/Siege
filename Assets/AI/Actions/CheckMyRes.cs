using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class CheckMyRes : RAINAction
{
	
	GameObject detected;
	Builder builderScript;
	
	public override void Start(RAIN.Core.AI ai)
	{
		base.Start(ai);
		builderScript = ai.Body.GetComponent<Builder>();
	}
	
	public override ActionResult Execute(RAIN.Core.AI ai)
	{
		int numRes = builderScript.GetCurrentResources();

		if(numRes <= 0)
		{
			ai.WorkingMemory.SetItem("holdingRes", false);
            return ActionResult.SUCCESS;
		}
        else ai.WorkingMemory.SetItem("holdingRes", true);
		return ActionResult.SUCCESS;
	}
	
	public override void Stop(RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
}