using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Entities.Aspects;
using RAIN.Core;

[RAINAction]
public class SelectResource : RAINAction
{
    IList<RAINAspect> resAspects;
    GameObject res, builder, closestRes;
    Resource resScript;
    float dist = 0;
    float closestDist = 9001;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        resAspects = ai.WorkingMemory.GetItem<IList<RAINAspect>>("resources");
        builder = ai.Body as GameObject;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        for( int i = 0; i < resAspects.Count; i++ )
        {
            res = resAspects[i].Entity.Form.gameObject;
            resScript = res.GetComponent<Resource>();
            dist = Vector3.Distance(builder.transform.position, res.transform.position);

            if (resScript.health > 0 && dist < closestDist)
            {
                closestRes = res;
                closestDist = dist;
            }
        }

        ai.WorkingMemory.SetItem("resourcePosition", closestRes.transform.position);
        ai.WorkingMemory.SetItem("resourceSeen", closestRes);
        ai.WorkingMemory.SetItem("resource", closestRes);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}