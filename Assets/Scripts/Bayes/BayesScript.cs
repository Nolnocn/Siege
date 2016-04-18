using UnityEngine;
using System.Collections;

public class BayesScript : MonoBehaviour {

	BayesDecision bs;

	void Start ()
	{
		bs = new BayesDecision();
		bs.ReadObsTab( "RepairTab.txt" );
		bs.Tab2Screen();
		bs.BuildStats();
		bs.DumpStats();

		Debug.Log( "Testing a broken wall, 100m away, with no enemies" );
		if (bs.Decide(Observation.WallDamageState.BROKEN, 10, false, false))
		{
			Debug.Log( "Going to repair!" );
		}
		else
		{
			Debug.Log( "Not my problem." );
		}
			
		//bs.AddObs( Observation.WallDamageState.BROKEN, 100, false, true, true );
		// Rebuild the statistics
		//bs.BuildStats();
		//bs.DumpStats();
		//bs.Tab2File("RepairTab.txt");
	}
}
