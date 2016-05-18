using UnityEngine;
using System.Collections;

using ThreshEvolve;

public class AttackerWaveSpawner : MonoBehaviour
{
	public AttackerEvoStats attackerPrefab;
	public Transform[] spawnPoints;
	public int popSize = 10;

	public int dmgChromLeng = 6;
	public int hpChromLeng = 5;

	private int dmgNumChromVals;
	private int hpNumChromVals;

	private int numSpawned = 0;

	private ThreshPop dmgPop;
	private ThreshPop hpPop;

	private AttackerEvoStats[] pop;

	private float checkTime = 5.0f;

	private bool allSpawned = false;

	// Use this for initialization
	void Start()
	{
		string basePath = Application.dataPath + "/Data/";
		hpNumChromVals = 1 << hpChromLeng;
		dmgNumChromVals = 1 << dmgChromLeng;
		dmgPop = new ThreshPop( dmgNumChromVals, popSize, basePath + "AtkDmg.txt", 0.9, 0.1 );
		hpPop = new ThreshPop( hpNumChromVals, popSize, basePath + "AtkHp.txt", 0.9, 0.1 );
		StartWave();
	}

	void Update()
	{
		if( allSpawned )
		{
			checkTime -= Time.deltaTime;

			if( checkTime <= 0 )
			{
				int numActive = 0;
				foreach( AttackerEvoStats attacker in pop )
				{
					if( attacker.gameObject.activeInHierarchy )
					{
						numActive++;
					}
				}

				if( numActive == 0 )
				{
					EndWave();
				}

				checkTime = 5.0f;
			}
		}
	}

	private void StartWave()
	{
		allSpawned = false;
		StartCoroutine( SpawnWave() );
	}

	private IEnumerator SpawnWave()
	{
		pop = new AttackerEvoStats[ popSize ];
		for( int i = 0; i < popSize; i++ )
		{
			yield return new WaitForSeconds( 1.0f );
			pop[ i ] = Instantiate( attackerPrefab ) as AttackerEvoStats;
			pop[ i ].transform.position = spawnPoints[ i % spawnPoints.Length ].position;

			uint hpChrom = hpPop.CheckOut();
			uint dmgChrom = dmgPop.CheckOut();

			float hp = Gen2Phen( hpChrom, 10f, 200f, hpNumChromVals );
			float dmg = Gen2Phen( dmgChrom, 5f, 35f, dmgNumChromVals );

			pop[ i ].Init( hpChrom, dmgChrom, hp, dmg );
		}

		allSpawned = true;
	}

	private void EndWave()
	{
		for( int i = 0; i < pop.Length; i++ )
		{
			int hpFit = Mathf.RoundToInt( pop[ i ].TimeSurvived * 0.5f );
			hpPop.CheckIn( pop[ i ].HpChrom, hpFit );

			int dmgFit = Mathf.RoundToInt( pop[ i ].DamageDealt * 0.5f );
			dmgPop.CheckIn( pop[ i ].HpChrom, dmgFit );

			//Destroy( pop[ i ].gameObject );
		}

		dmgPop.WritePop();
		hpPop.WritePop();

		StartWave();
	}

	private float Gen2Phen( uint gen, float lb, float ub, int nChromVals )
	{
		float step = ( ub - lb ) / nChromVals;
		print( step );
		print( gen );
		return (gen * step + lb);
	}
}
