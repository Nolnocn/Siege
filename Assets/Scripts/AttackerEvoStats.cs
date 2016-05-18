using UnityEngine;
using System.Collections;

public class AttackerEvoStats : MonoBehaviour
{
	public float damage;

	private float damageDealt = 0;
	private float timeSurvived = 0;

	private uint myHpChrom;
	private uint myDmgChrom;

	public float Damage
	{
		get { return damage; }
	}

	public float DamageDealt
	{
		get { return damageDealt; }
		set { damageDealt = value; }
	}

	public float TimeSurvived
	{
		get { return timeSurvived; }
	}

	public uint HpChrom
	{
		get { return myHpChrom; }
	}

	public uint DmgChrom
	{
		get { return myDmgChrom; }
	}

	public void Init( uint hpChrom, uint dmgChrom, float hp, float dmg )
	{
		myHpChrom = hpChrom;
		myDmgChrom = dmgChrom;

		damage = dmg;

		print( "New guy" );
		print( hp );
		print( dmg );

		GetComponent<Health>().hp = hp;
	}

	void Update()
	{
		timeSurvived += Time.deltaTime;
	}
}
