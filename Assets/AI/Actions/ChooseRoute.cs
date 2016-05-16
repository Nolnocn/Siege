using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class ChooseRoute : RAINAction
{
    GameObject routeRef, builder;
    string route;
    float dist;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        routeRef = ai.WorkingMemory.GetItem("routeRef") as GameObject;
        builder = ai.Body as GameObject;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        dist = Vector3.Distance(builder.transform.position, routeRef.transform.position);

        if (dist > 30)
        {
            ai.WorkingMemory.SetItem("route", "BuilderRoute");
        }
        else
        {
            ai.WorkingMemory.SetItem("route", "BuilderRouteInner");
        }
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}