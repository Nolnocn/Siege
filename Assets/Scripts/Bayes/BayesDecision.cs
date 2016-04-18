using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BayesDecision
{
	public static readonly string DATA_PATH =  "Assets/Data/";
	public static readonly double SQRT_2_PI = Math.Sqrt( 2.0 * Math.PI );

	// List of observations.  Initialized from the data file.
	// Added to with new observations during program run
	List<Observation> obsTab = new List<Observation> ();

	// All the little tables to store the counts, proportions,
	// sums, sums of squares, means, and standard deviations
	// for the 4 conditions and the action.  Used doubles for the proportions,
	// means and standard deviations to mitigate roundoff errors for products
	// of small probabilities.

	// Damage condition (discrete => Need to count frequencies)
	int[,] wallDmgCt = new int[3, 2];		// 3 values X 2 outcomes (play T or F)
	double[,] wallDmgPrp = new double[3, 2];

	// Distance condition (continuous) needs sum, sum of squares for mean, StdDev
	int[] distSum = new int[2];				// Stats for 2 outcomes (T or F)
	int[] distSumSq = new int[2];
	double[] distMean = new double[2];
	double[] distStdDev = new double[2];

	int[,] enemyNearCt = new int[2, 2];			// Discrete with 2 values (T or F)
	double[,] enemyNearPrp = new double[2, 2];	// 2 T/F X 2 outcomes T/F

	int[,] hasObjCt = new int[2, 2];			// Discrete with 2 values (T or F)
	double[,] hasObjPrp = new double[2, 2];	// 2 T/F X 2 outcomes T/F

	// Repair action (Boolean)
	int[] repairCt = new int[2];				// Outcome (play T or F)
	double[] repairPrp = new double[2];

	public BayesDecision()
	{
		Debug.Log( DATA_PATH );
		InitStats();
	}

	// Read the observations from text file fName.txt and add them to the list
	public void ReadObsTab (string fName)
	{
		try {
			using (StreamReader rdr = new StreamReader ( DATA_PATH + fName)) {
				string lineBuf = null;
				while ((lineBuf = rdr.ReadLine ()) != null)
				{
					string[] lineAra = lineBuf.Split (' ');

					// Map strings to correct data types for conditions & action
					// and Add the observation to List obsTab
					int wallDmgState = int.Parse( lineAra[0] );
					int dist = int.Parse( lineAra[1] );
					bool enemyNear = bool.Parse( lineAra[2] );
					bool haveObj = bool.Parse( lineAra[3] );
					bool repair = bool.Parse( lineAra[4] );

					AddObs ( (Observation.WallDamageState)wallDmgState, dist, enemyNear, haveObj, repair );
				}
			}
		}
		catch {
			Debug.Log(String.Format ("Problem reading and/or parsing observations in " + fName));
		}
	}

	public void AddObs( Observation.WallDamageState wallDmgState, int dist, bool enemyNear, bool hasObj, bool repair )
	{
		// Build an Observation struct
		Observation obs = new Observation( wallDmgState, dist, enemyNear, hasObj, repair );

		// Add it to the List
		obsTab.Add (obs);
	}

	public void Tab2File(string fName)
	{
		try {
			using (StreamWriter wtr = new StreamWriter ( DATA_PATH + fName)) {
				foreach (Observation obs in obsTab)
				{
					wtr.Write ("{0}", (int)obs.wallDmgState);
					wtr.Write (" {0}", obs.distToWall);
					wtr.Write (" {0}", obs.enemyNearby);
					wtr.Write (" {0}", obs.haveObjective);
					wtr.WriteLine (" {0}", obs.repair);
				}
			}
		}
		catch {
			Debug.Log(String.Format ("Problem writing out the Observations to " + fName));
			Debug.Log(String.Format ("File not changed."));
		}
	}

	public void Tab2Screen ()
	{
		Debug.Log ("DMG  Dist Enemy hasObj | Repair?");
		foreach (Observation obs in obsTab)
		{
			string log = String.Format("{0,-8}", obs.wallDmgState);
			log += " | " + obs.distToWall;
			log += " | " + String.Format("{0,-5}", obs.enemyNearby);
			log += " | " + String.Format("{0,-5}", obs.haveObjective);
			log += " | " + String.Format("{0,-5}", obs.repair);
			Debug.Log( log );
		}
	}

	// Build all the statistics needed for Bayes from the observations
	// in obsTab.  Presumably, this would be called during initialization
	// and not after every new observation has been added durng game play,
	// as it does a lot of crunching on doubles.  With a small obsTab
	// this may not be much of an issue, but as it grows O(n) with size of
	// obsTab, it could pork out with a lot of new observations added during
	// game play.
	//
	// Could implement an UpdateStats() method that incrementally bumped the
	// accumulators when a new observation is added before recalculating the
	// stats, which would eliminate the observation loop,
	// but there still would be some crunching going on.
	public void BuildStats ()
	{
		InitStats ();		// Reset all the accumulators

		// Accumulate all the counts and sums
		foreach (Observation obs in obsTab) {
			// Do this once
			int repOff = obs.repair ? 0 : 1;

			wallDmgCt [(int)obs.wallDmgState, repOff]++;

			distSum [repOff] += obs.distToWall;
			distSumSq [repOff] += obs.distToWall * obs.distToWall;

			enemyNearCt [obs.enemyNearby ? 0 : 1, repOff]++;
			hasObjCt [obs.haveObjective ? 0 : 1, repOff]++;

			repairCt [repOff]++;
		}

		// Calculate all the statistics
		CalcProps (wallDmgCt, repairCt, wallDmgPrp);

		distMean [0] = Mean (distSum [0], repairCt [0]);
		distMean [1] = Mean (distSum [1], repairCt [1]);
		distStdDev [0] = StdDev (distSumSq [0], distSum [0], repairCt [0]);
		distStdDev [1] = StdDev (distSumSq [1], distSum [1], repairCt [1]);

		CalcProps (enemyNearCt, repairCt, hasObjPrp);
		CalcProps (hasObjCt, repairCt, hasObjPrp);

		repairPrp [0] = (double)repairCt [0] / obsTab.Count;
		repairPrp [1] = (double)repairCt [1] / obsTab.Count;
	}

	// Bayes likelihood for four condition values and one action value
	// For each possible action value, call this with a specific set of four
	// condition values, and pick the action that returns the highest
	// likelihood as the most likely action to take, given the conditions.
	double CalcBayes ( Observation obs )
	{
		int repOff = obs.repair ? 0 : 1;
		double like = wallDmgPrp [(int)obs.wallDmgState, repOff] *
			GauProb (distMean [repOff], distStdDev [repOff], obs.distToWall) *
			enemyNearPrp [obs.enemyNearby ? 0 : 1, repOff] *
			hasObjPrp [obs.haveObjective ? 0 : 1, repOff] *
			repairPrp [repOff];
		return like;
	}

	// Decide whether to play or not.
	// Returns true if decision is to play, false o/w
	// Can turn on/off diagnostic output to Console by playing with "*/"
	public bool Decide( Observation.WallDamageState wallDmgState, int dist, bool enemyNear, bool hasObj )
	{
		// Build an Observation struct
		Observation obs = new Observation();
		obs.wallDmgState = wallDmgState;
		obs.distToWall = dist;
		obs.enemyNearby = enemyNear;
		obs.haveObjective = hasObj;

		double playYes = CalcBayes ( obs );
		double playNo = CalcBayes ( obs );

		return playYes > playNo;
	}

	// Need to (re)initialize accumulators now and then
	void InitStats ()
	{
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 2; j++) {
				wallDmgCt [i, j] = 0;
			}
		}

		for (int i = 0; i < distSum.Length; i++) {
			distSum [i] = 0;
			distSumSq [i] = 0;
		}

		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				enemyNearCt [i, j] = 0;
			}
		}

		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				hasObjCt [i, j] = 0;
			}
		}

		for (int i = 0; i < 2; i++) {
			repairCt [i] = 0;
		}
	}

	// Standard statistical functions.

	// Calculates the proportions for a discrete table of counts
	// Handles the 0-frequency problem by assigning an artificially
	// low value that is still greater than 0.
	private void CalcProps (int[,] counts, int[] n, double[,] props)
	{
		for (int i = 0; i < counts.GetLength (0); i++)
			for (int j = 0; j < counts.GetLength (1); j++)
				// Detects and corrects a 0 count by assigning a proportion
				// that is 1/10 the size of a proportion for a count of 1
				if (counts [i, j] == 0)
					props [i, j] = 0.1d / repairCt [j];	// Can't have 0
				else
					props [i, j] = (double)counts [i, j] / n [j];
	}

	private double Mean (int sum, int n)
	{
		return (double)sum / n;
	}

	private double StdDev (int sumSq, int sum, int n)
	{
		return Math.Sqrt((sumSq - (sum * sum) / (double)n) / (n - 1));
	}

	// Calculates probability of x in a normal distribution of
	// mean and stdDev.  This corrects a mistake in the pseudo-code,
	// which used a power function instead of an exponential.
	private double GauProb (double mean, double stdDev, int x)
	{
		double xMinusMean = x - mean;
		return (1.0d / (stdDev * SQRT_2_PI)) *
			Math.Exp (-1.0d * xMinusMean * xMinusMean / (2.0d * stdDev * stdDev));
	}

	// Dump all the statistics to the Console for debugging purposes
	public void DumpStats ()
	{
		Debug.Log(String.Format ("Overall Outcomes:"));
		Debug.Log(String.Format ("#True  {0}  Proportion {1:F3}",
				repairCt [0], repairPrp [0]));
		Debug.Log(String.Format ("#False {0}  Proportion {1:F3}",
					repairCt [1], repairPrp [1]));

		Debug.Log(String.Format ("Wall Dmg State: "));
		Debug.Log(String.Format ("Value    #T  p of T  #F  p of F"));
		for (int i = 0; i < 3; i++) {
			Debug.Log(String.Format ("{0,-8}", (Observation.WallDamageState)i));
			for (int j = 0; j < 2; j++) {
				Debug.Log(String.Format ("  {0:D}", wallDmgCt [i, j]));
				Debug.Log(String.Format ("  {0:F4} ", wallDmgPrp [i, j]));
			}
		}

		Debug.Log("Dist:");
		DumpContAttr (distMean [0], distMean [1], distStdDev [0], distStdDev [1]);

		Debug.Log("Enemy Nearby:");
		Debug.Log("Value    #T  p of T  #F  p of F");
		for (int i = 0; i < 2; i++)
		{
			if (i == 0)
				Debug.Log(String.Format ("{0,-8}", true));		// Kludgy way to show boolean
			else
				Debug.Log(String.Format ("{0,-8}", false));
			for (int j = 0; j < 2; j++)
			{
				Debug.Log(String.Format ("  {0:D}", enemyNearCt [i, j]));
				Debug.Log(String.Format ("  {0:F4} ", enemyNearPrp [i, j]));
			}
		}
			
		Debug.Log("Has Objective:");
		Debug.Log("Value    #T  p of T  #F  p of F");
		for (int i = 0; i < 2; i++)
		{
			if (i == 0)
				Debug.Log(String.Format ("{0,-8}", true));		// Kludgy way to show boolean
			else
				Debug.Log(String.Format ("{0,-8}", false));
			for (int j = 0; j < 2; j++)
			{
				Debug.Log(String.Format ("  {0:D}", hasObjCt [i, j]));
				Debug.Log(String.Format ("  {0:F4} ", hasObjPrp [i, j]));
			}
		}
	}

	// Dump a continuous attribute's statistics
	void DumpContAttr (double mT, double mF, double sdT, double sdF)
	{
		Debug.Log(String.Format (" MeanT   MeanF   StDvT   StDvF"));
		Debug.Log(String.Format ("{0,6:F2}  {1,6:F2} ", mT, mF));
		Debug.Log(String.Format (" {0,6:F2}  {1,6:F2} ", sdT, sdF));
	}
}
