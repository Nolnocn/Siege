using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;


[RAINAction]
public class attack : RAINAction
{
	GameObject detected;
	float attackCooldown = 2f;
	float attackTimer = 0f;
	Health targetHealthscript;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		if(attackTimer + attackCooldown<Time.time)
		{
			//GameObject detected = agent.WorkingMemory.GetItem.<GameObject>(“defenderAspect”);
			 GameObject detected = ai.WorkingMemory.GetItem ("enemy") as GameObject;

			if(detected == null)
			{return ActionResult.FAILURE;}

			Debug.Log(ai.WorkingMemory.GetItem ("enemy"));
			targetHealthscript = detected.GetComponent<Health> ();
			targetHealthscript.hp -= 10;
			Debug.Log("Enemy hit, current hp is " + targetHealthscript.hp);
			attackTimer=Time.time;

		}

		return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}