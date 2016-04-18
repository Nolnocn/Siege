using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class moveWaypoint : RAINAction
{

	RAIN.Motion.MoveLookTarget detected;
	//RAIN.Motion.MoveLookTarget
	GetComponents componentGetter;
	NavMeshAgent moverAgent;


    public override void Start(RAIN.Core.AI ai)
    {
		detected = RAIN.Motion.MoveLookTarget.GetTargetFromVariable(ai.WorkingMemory, "nextPosition");
		Debug.Log(detected.Position);
		componentGetter = ai.Body.GetComponent<GetComponents> ();
		moverAgent = componentGetter.m_agent;
		//moverAgent.destination = detected.Position;

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