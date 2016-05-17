using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using Bayes;

[RAINAction]
public class CheckBayes : RAINAction
{
    BayesBuilder bayes;
    GameObject builder, gate;
    Health gateScript;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        builder = ai.Body as GameObject;
        bayes = builder.GetComponent<BayesBuilder>();
        gate = ai.WorkingMemory.GetItem("wall") as GameObject;
        gateScript = gate.GetComponent<Health>();
        bayes.gate = gateScript;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        bayes.TestBayes();

        if (bayes.doingIt)
        {
            return ActionResult.SUCCESS;
        }
        else
            return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}