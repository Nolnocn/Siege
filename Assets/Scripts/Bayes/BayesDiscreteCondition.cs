using UnityEngine;
using System.Collections;

namespace Bayes
{
	public class BayesDiscreteCondition
	{
		private string m_name;
		private int[,] m_counts;
		private double[,] m_proportions;

		public string Name
		{
			get { return m_name; }
		}

		public BayesDiscreteCondition( string name, int numValues = 2 )
		{
			m_name = name;
			m_counts = new int[ numValues, 2 ];
			m_proportions = new double[ numValues, 2];
		}

		public int[,] GetCounts()
		{
			return m_counts;
		}

		public double[,] GetProportions()
		{
			return m_proportions;
		}

		public void CalcProps( BayesAction outcome )
		{
			int[] n = outcome.GetCounts();

			for (int i = 0; i < m_counts.GetLength( 0 ); i++ )
			{
				for( int j = 0; j < m_counts.GetLength( 1 ); j++ )
				{
					// Detects and corrects a 0 count by assigning a proportion
					// that is 1/10 the size of a proportion for a count of 1
					if ( m_counts[ i, j ] == 0 )
					{
						m_proportions[ i, j ] = 0.1d / n[ j ];	// Can't have 0
					}
					else
					{
						m_proportions[ i, j ] = (double)m_counts[ i, j ] / n[ j ];
					}
				}
			}
		}
	}
}