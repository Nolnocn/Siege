using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class moveAttack : RAINAction
{
	GameObject detected;

	GetComponents componentGetter;
	NavMeshAgent moverAgent;


	public override void Start(RAIN.Core.AI ai)
	{
		GameObject detected = ai.WorkingMemory.GetItem ("enemy") as GameObject;
		componentGetter = ai.Body.GetComponent<GetComponents> ();
		moverAgent = componentGetter.m_agent;
		moverAgent.destination = detected.transform.position;

		base.Start(ai);
	}

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}