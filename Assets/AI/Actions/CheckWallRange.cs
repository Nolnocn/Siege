using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class CheckWallRange : RAINAction
{
	GameObject wall;
	GameObject builder;

	public override void Start( RAIN.Core.AI ai )
	{
		base.Start( ai );
		builder = ai.Body as GameObject;
		wall = ai.WorkingMemory.GetItem( "wall" ) as GameObject;
	}

	public override ActionResult Execute( RAIN.Core.AI ai )
	{
		float dist = Vector3.Distance( builder.transform.position, wall.transform.position );

		if ( dist > 5 )
		{
            ai.WorkingMemory.SetItem( "wallInRange", false );
            return ActionResult.FAILURE;
		}

        ai.WorkingMemory.SetItem( "wallInRange", true );
		return ActionResult.SUCCESS;
	}

	public override void Stop( RAIN.Core.AI ai )
	{
		base.Stop( ai );
	}
}