using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class WallHPCheck : RAINAction
{
	
	GameObject detected;
	Health wallScript;
	
	public override void Start(RAIN.Core.AI ai)
	{
		base.Start(ai);
	}
	
	public override ActionResult Execute(RAIN.Core.AI ai)
	{
		GameObject detected = ai.WorkingMemory.GetItem ("wall") as GameObject;
		wallScript = detected.GetComponent<Health> ();
		
        if(wallScript.hp <= 0 || wallScript.hp == 100)
        {
            ai.WorkingMemory.SetItem("wallHealth", wallScript.hp);
			return ActionResult.FAILURE;
		}
		
        ai.WorkingMemory.SetItem("wallHealth", wallScript.hp);
		return ActionResult.SUCCESS;
	}
	
	public override void Stop(RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
}