using System.Collections;

public struct Observation
{
	public enum WallDamageState
	{
		NO_DMG,
		DAMAGED,
		BROKEN
	}

	public WallDamageState wallDmgState; // What is the wall's condition?
	public int distToWall; // How close is the wall?
	public bool enemyNearby; // Is it safe?
	public bool haveObjective; // Do I already have something to do?

	public bool repair;

	public Observation( WallDamageState dmgState, int dist, bool enemyNear, bool haveObj, bool doRepair )
	{
		wallDmgState = (WallDamageState)dmgState;
		distToWall = dist;
		enemyNearby = enemyNear;
		haveObjective = haveObj;
		repair = doRepair;
	}
}
