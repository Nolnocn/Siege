using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Bayes
{
    public class BayesDecider
	{
		public static readonly double SQRT2PI = Math.Sqrt( 2.0 * Math.PI );

		private string dataPath;

		private List<Observation> obsTab = new List<Observation> ();
		private BayesDiscreteCondition[] discreteConditions;
		private BayesContinuousCondition[] continuousConditions;
		private BayesAction outcomeAction;

		public BayesDecider( string fname, string outcomeName )
		{
			dataPath = Application.dataPath + "/Data/" + fname;

			outcomeAction = new BayesAction( outcomeName );

			ReadObsTab();
		}

		public void SetConditions( BayesContinuousCondition[] contConds, BayesDiscreteCondition[] discConds )
		{
			continuousConditions = contConds;
			discreteConditions = discConds;
		}

		// Read the observations from text file fName.txt and add them to the list
		public void ReadObsTab()
		{
			try
			{
				using( StreamReader rdr = new StreamReader( dataPath ) )
				{
					string lineBuf = null;
					while ( ( lineBuf = rdr.ReadLine() ) != null )
					{
						string[] lineAra = lineBuf.Split(' ');
						ParseTabLine( lineAra );
					}
				}
			}
			catch
			{
				Debug.Log( "Problem reading and/or parsing observations in " + dataPath );
			}
		}

		public void ParseTabLine( string[] line )
		{
			List<int> contValues = new List<int>();
			List<int> discValues = new List<int>();
			int outcome = 0;

			for( int i = 0; i < line.Length; i++ )
			{
				string strValue = line[ i ];
				int numVal = int.Parse( strValue.Substring( 1 ) );

				switch( strValue[ 0 ] )
				{
				case 'c':
					contValues.Add( numVal );
					break;
				case 'd':
					discValues.Add( numVal );
					break;
				case 'o':
					outcome = numVal;
					break;
				default:
					Debug.Log( "Unknown string value!" );
					break;
				}
			}

			AddObservation( contValues.ToArray(), discValues.ToArray(), outcome );
		}

		// Add an observation to the list.
		// Used when reading the file and when adding new observations on the fly
		public void AddObservation( int[] contValues, int[] discValues, int outcome )
		{
			// Build an Observation struct
			Observation obs;
			obs.continuousValues = contValues;
			obs.discreteValues = discValues;
			obs.outcome = outcome;
			obsTab.Add( obs );
		}

		public void AddObservation( Observation obs )
		{
			obsTab.Add( obs );
		}

		// Dump obsTab to text file fName so it can be read next time
		public void Tab2File()
		{
			try
			{
				using( StreamWriter wtr = new StreamWriter( dataPath ) )
				{
					foreach( Observation obs in obsTab )
					{
						for( int i = 0; i < obs.continuousValues.Length; i++ )
						{
							wtr.Write( "c{0} ", obs.continuousValues[ i ] );
						}

						for( int i = 0; i < obs.continuousValues.Length; i++ )
						{
							wtr.Write( "d{0} ", obs.discreteValues[ i ] );
						}

						wtr.WriteLine ("o{0}", obs.outcome);
					}
				}
			}
			catch
			{
				Console.WriteLine( "Problem writing out the Observations to " + dataPath );
				Console.WriteLine( "File not changed." );
			}
		}

		// Dump obsTab to the Console window for debugging purposes
		public void Tab2Screen ()
		{
			string output = "------------------------------\n";

			for( int i = 0; i < discreteConditions.Length; i++ )
			{
				output += discreteConditions[ i ].Name + "\t";
			}

			for( int i = 0; i < continuousConditions.Length; i++ )
			{
				output += continuousConditions[ i ].Name + "\t";
			}

			output += outcomeAction.Name + "\n";

			foreach( Observation obs in obsTab )
			{
				for( int i = 0; i < obs.continuousValues.Length; i++ )
				{
					output += obs.discreteValues[ i ] + "\t";
				}

				for( int i = 0; i < obs.continuousValues.Length; i++ )
				{
					output += obs.continuousValues[ i ] + "\t";
				}

				//output += "\nOutcome Values: ";
				output += "\t" + ( obs.outcome == 0 ? "True" : "False" );
				output += "\n";
			}

			Debug.Log( output );
		}

		public void BuildStats ()
		{
			int[] outcomes = outcomeAction.GetCounts();

			int numCont = continuousConditions.Length;
			int numDisc = discreteConditions.Length;

			// Accumulate all the counts and sums
			foreach( Observation obs in obsTab )
			{
				int outcome = obs.outcome;

				for( int i = 0; i < numCont; i++ )
				{
					int[] sum = continuousConditions[ i ].GetSums();
					sum[ outcome ] += obs.continuousValues[ i ];
                    
                    int[] sumSq = continuousConditions[ i ].GetSqSums();
                    sumSq[ outcome ] += obs.continuousValues[ i ] * obs.continuousValues[ i ];
				}
					
				for( int i = 0; i < numDisc; i++ )
				{
					int[,] count = discreteConditions[ i ].GetCounts();
					int val = obs.discreteValues[ i ];
					count[ val, outcome ]++;
				}

				outcomes[ outcome ]++;
			}

			for( int i = 0; i < numCont; i++ )
			{
				continuousConditions[ i ].Init( outcomeAction );
			}

			for( int i = 0; i < numDisc; i++ )
			{
				discreteConditions[ i ].CalcProps( outcomeAction );
			}

			outcomeAction.CalcProps( obsTab.Count );
		}

		// Calculates probability of x in a normal distribution of
		// mean and stdDev.  This corrects a mistake in the pseudo-code,
		// which used a power function instead of an exponential.
		double GauProb( double mean, double stdDev, int x )
		{
			double xMinusMean = x - mean;
			return ( 1.0d / ( stdDev * SQRT2PI ) ) *
				Math.Exp( -1.0d * xMinusMean * xMinusMean / ( 2.0d * stdDev * stdDev ) );
		}

		// Bayes likelihood for four condition values and one action value
		// For each possible action value, call this with a specific set of four
		// condition values, and pick the action that returns the highest
		// likelihood as the most likely action to take, given the conditions.
		double CalcBayes( int[] contValues, int[] discValues, bool outcome )
		{
			int doIt = outcome ? 0 : 1;

			double like = 1.0;

			for( int i = 0; i < discreteConditions.Length; i++ )
			{
				double[,] props = discreteConditions[ i ].GetProportions();
                //Debug.Log( "Props: " + props );
				like *= props[ discValues[ i ], doIt ];
                //Debug.Log( "Like: " + like );
			}

			for( int i = 0; i < continuousConditions.Length; i++ )
			{
				double mean = continuousConditions[ i ].GetMean( doIt );
				double stdDev = continuousConditions[ i ].GetStdDev( doIt );
                
                //Debug.Log( "Mean: " + mean );
                //Debug.Log( "StdDev: " + stdDev );

				like *= GauProb( mean, stdDev, contValues[ i ] );
                //Debug.Log( "Like: " + like );
			}

            //Debug.Log( "Outcome: " + outcomeAction.GetProportion( doIt ) );
			like *= outcomeAction.GetProportion( doIt );
            //Debug.Log( "Like: " + like );
			return like;
		}

		// Decide whether to play or not.
		// Returns true if decision is to play, false o/w
		// Can turn on/off diagnostic output to Console by playing with "*/"
		public bool Decide( int[] contValues, int[] discValues )
		{
			double outcomeYes = CalcBayes( contValues, discValues, true );
			double outcomeNo = CalcBayes( contValues, discValues, false );
            
			/* To turn off output, remove this end comment ->
			double yesNno = outcomeYes + outcomeNo;
            string output = "";
			output += string.Format("playYes: {0}", outcomeYes);	// Use scientifice notation
			output += "\n";
            output += string.Format("playNo:  {0}", outcomeNo);		// for very small numbers
            output += "\n";
			output += string.Format("playYes Normalized: {0,6:F4}", outcomeYes / yesNno);
            output += "\n";
			output += string.Format("playNo  Normalized: {0,6:F4}", outcomeNo / yesNno);
            Debug.Log( output );
			/* */

			return outcomeYes > outcomeNo;
		}

		// Dump all the statistics to the Console for debugging purposes
		public void DumpStats ()
		{
			string output = "";
			output += "Overall Repair Outcomes:\n";

			int[] outcomeCounts = outcomeAction.GetCounts();
			double[] outcomeProps = outcomeAction.GetProportions();
			output += string.Format("#True  {0}  Proportion {1:F3}", outcomeCounts[0], outcomeProps[0]);
			output += "\n";
			output += string.Format("#False {0}  Proportion {1:F3}", outcomeCounts[1], outcomeProps[1]);
			output += "\n\n";

			for( int i = 0; i < continuousConditions.Length; i++ )
			{
				BayesContinuousCondition cc = continuousConditions[ i ];
				output += cc.Name + "\n";
				output += DumpContAttr( cc.GetMean( 0 ), cc.GetMean( 1 ), cc.GetStdDev( 0 ), cc.GetStdDev( 1 ) );
			}

			output += "\n";

			foreach( BayesDiscreteCondition dc in discreteConditions )
			{
				output += dc.Name + "\n";
				int[,] counts = dc.GetCounts();
				double[,] prps = dc.GetProportions();
				for( int i = 0; i < counts.GetLength( 0 ); i++ )
				{
					output += "Value: " + i;
					output += " True: ";
					output += string.Format(" Count: {0:D}", counts[i, 0]);
					output += string.Format(" Prop: {0:F4} ", prps[i, 0]);

					output += " False: ";
					output += string.Format(" Count: {0:D}", counts[i, 1]);
					output += string.Format(" Prop: {0:F4} ", prps[i, 1]);
					output += "\n";
				}
			}

			Debug.Log( output );
		}

		// Dump a continuous attribute's statistics
		private string DumpContAttr( double mT, double mF, double sdT, double sdF )
		{
			string o = "";
			o += " MeanT   MeanF   StDvT   StDvF\n";
			o += string.Format("{0,6:F2}  {1,6:F2} ", mT, mF);
			o += string.Format(" {0,6:F2}  {1,6:F2} ", sdT, sdF);
			o += "\n";

			return o;
		}
	}
}
