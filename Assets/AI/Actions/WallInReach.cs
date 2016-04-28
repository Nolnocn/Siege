using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class WallInReach : RAINAction
{
	
	Vector3 builderPos;
	WallController wallScript;
	
	public override void Start(RAIN.Core.AI ai)
	{
		base.Start(ai);
		builderPos = ai.Body.transform.position;
	}
	
	public override ActionResult Execute(RAIN.Core.AI ai)
	{
		GameObject wall = ai.WorkingMemory.GetItem ("Wall") as GameObject;
		Vector3 wallPos = wall.transform.position;
		float dist = Vector3.Distance(builderPos, wallPos);
		
		if(wallScript.health <= 0 || wallScript.health == 100)
		{
			return ActionResult.FAILURE;
		}
		
		ai.WorkingMemory.SetItem("wallHealth", wallScript.health);
		return ActionResult.SUCCESS;
	}
	
	public override void Stop(RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
}