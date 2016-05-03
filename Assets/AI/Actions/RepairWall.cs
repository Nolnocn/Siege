using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class RepairWall : RAINAction
{

	GameObject wall;
	WallController wallScript;
	Builder builderScript;

	public override void Start(RAIN.Core.AI ai)
	{
		base.Start(ai);
		builderScript = ai.Body.GetComponent<Builder>();
	}

	public override ActionResult Execute(RAIN.Core.AI ai)
	{
		wall = ai.WorkingMemory.GetItem ("wall") as GameObject;
		wallScript = wall.GetComponent<WallController> ();

		wallScript.Heal( builderScript.GiveResource() );

		return ActionResult.SUCCESS;
	}

	public override void Stop(RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
}