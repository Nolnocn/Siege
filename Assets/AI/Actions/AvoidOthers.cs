using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Navigation;
using RAIN.Navigation.Graph;
using RAIN.Entities.Aspects;

[RAINAction]
public class AvoidCollision : RAINAction
{
    public Expression avoidRange;

    private Vector3 _target;
    private IList<RAINAspect> _targetsToAvoid;
    private float range;
    private Vector3 between;
    private Vector3 avoidVector;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        _targetsToAvoid = ai.WorkingMemory.GetItem<IList<RAINAspect>>("builders");


        if(!float.TryParse(avoidRange.ExpressionAsEntered, out range))
            range = 2f;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if(_targetsToAvoid.Count == 0)
            return ActionResult.SUCCESS;

        foreach(RAINAspect aspect in _targetsToAvoid) {
            if(IsTooClose(ai, aspect))
                DoAvoidance(ai, aspect);
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

    private bool IsTooClose(AI ai, RAINAspect aspect) {
        float dist = Vector3.Distance(ai.Kinematic.Position, aspect.Position);

        if(dist <= range)
            return true;

        return false;
    }

    private void DoAvoidance(AI ai, RAINAspect aspect) {
        between = ai.Kinematic.Position - aspect.Position;
        avoidVector = Vector3.Cross(Vector3.up, between);

        Vector3 avoidPoint;

        int direction = Random.Range(0, 100);

        avoidVector.Normalize();

        if(direction < 50)
            avoidVector *= -1;

        avoidPoint = GetPositionOnNavMesh(avoidVector, ai);

        if(avoidPoint == Vector3.zero)
        {
            avoidVector *= -1;
            avoidPoint = GetPositionOnNavMesh(avoidVector, ai);
        }

        if(avoidPoint == Vector3.zero)
        {
            Debug.Log("Avoid not possible!");

            // Change destination
            ai.WorkingMemory.SetItem("hasArrived", true);

            return;
        }


        ai.Motor.MoveTo(ai.Kinematic.Position + avoidPoint);
        Debug.Log (ai.Body.name + " is avoiding " + aspect.Entity.Form.name);
    }

    private Vector3 GetPositionOnNavMesh(Vector3 loc, AI ai) {
        Vector3 avoidPoint;
        RAIN.Navigation.Pathfinding.RAINPath myPath = null;

        int tries = 0;

        do {
            avoidPoint = new Vector3(loc.x + Random.Range(-0.8f, 0.8f),
                loc.y,
                loc.z + Random.Range(-0.8f, 0.8f));

            tries++;

            if(tries >= 1000)
                return Vector3.zero;
        } while(Vector3.Distance(loc, avoidPoint) > 1f && !ai.Navigator.GetPathTo(avoidPoint, 10, true, out myPath));

        return avoidPoint;
    }
}