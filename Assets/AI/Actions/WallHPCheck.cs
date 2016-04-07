using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class WallHPCheck : RAINAction
{
	
	GameObject detected;
	WallController wallScript;
	
	public override void Start(RAIN.Core.AI ai)
	{
		base.Start(ai);
	}
	
	public override ActionResult Execute(RAIN.Core.AI ai)
	{
		GameObject detected = ai.WorkingMemory.GetItem ("Wall") as GameObject;
		wallScript = detected.GetComponent<WallController> ();
		
		if(wallScript.health <= 0 || wallScript.health == 100)
		{
			return ActionResult.FAILURE;
		}
		
		ai.WorkingMemory.SetItem("wallHealth", wallScript.health);
		ai.WorkingMemory.ItemExists("wallHealth");
		Debug.Log(ai.WorkingMemory.ItemExists("wallHealth"));
		return ActionResult.SUCCESS;
	}
	
	public override void Stop(RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
}