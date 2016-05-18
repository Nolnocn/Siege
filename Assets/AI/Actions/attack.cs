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

	//tRig.AI.WorkingMemory.SetItem<SoldierAmmo>("MyAmmo", _startingAmmo);

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
			if (detected == null) {
				 detected = ai.WorkingMemory.GetItem ("gate") as GameObject;
			}

			/*if(detected.activeSelf == false )
			{
				Debug.Log ("In detected == null");
				detected = null;
				//RAIN.Core.AI.AIInit();
				return ActionResult.FAILURE;
			}*/

			//Debug.Log(ai.WorkingMemory.GetItem ("enemy"));
			targetHealthscript = detected.GetComponent<Health> ();
			targetHealthscript.hp -= 15;
			if (targetHealthscript.hp < 0) {
				detected = null;
				//return ActionResult.SUCCESS;
				return ActionResult.FAILURE;
				//ai.WorkingMemory.Clear ();
				//ai.WorkingMemory.SetItem<GameObject>("gate", detected);
				//ai.WorkingMemory.SetItem<GameObject>("enemy", detected);

				//ai.WorkingMemory.SetItem<GameObject>("gateSeen", detected);
				//ai.WorkingMemory.SetItem<GameObject>("enemySeen", detected);

				//RAIN.Core.AIRig.AI.Mind.AIInit ();

			}
			Debug.Log(ai.Body.name + ": Enemy hit, current hp is " + targetHealthscript.hp);
			attackTimer=Time.time;

		}

		return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}