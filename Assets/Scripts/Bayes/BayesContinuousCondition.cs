using System;

namespace Bayes
{
	public class BayesContinuousCondition
	{
		private string m_name;
		private int[] m_sum = new int[ 2 ];
		private int[] m_sumSq = new int[ 2 ];
		private double[] m_mean = new double[ 2 ];
		private double[] m_stdDev = new double[ 2 ];

		public string Name
		{
			get { return m_name; }
		}

		public BayesContinuousCondition( string name )
		{
			m_name = name;
		}

		/*public BayesContinuousCondition( int[] sum, int[] sumSq )
		{
			m_sum = sum;
			m_sumSq = sumSq;
		}*/

		public void Init( BayesAction outcome )
		{
			int length = m_sum.Length;

			for( int i = 0; i < length; i++ )
			{
				m_mean[ i ] = Mean( m_sum[ i ], outcome.GetCounts()[ i ] );
				m_stdDev[ i ] = StdDev( m_sumSq[ i ], m_sum[ i ], outcome.GetCount( i ) );
			}
		}

		public int[] GetSums()
		{
			return m_sum;
		}

		public int[] GetSqSums()
		{
			return m_sumSq;
		}

		public double GetMean( int index )
		{
			return m_mean[ index ];
		}

		public double[] GetMeans()
		{
			return m_mean;
		}

		public double GetStdDev( int index )
		{
			return m_stdDev[ index ];
		}

		public double[] GetStdDevs()
		{
			return m_stdDev;
		}

		private static double Mean( int sum, int n )
		{
			return (double)sum / n;
		}

		private static double StdDev( int sumSq, int sum, int n )
		{
			return Math.Sqrt( ( sumSq - ( sum * sum ) / (double)n ) / ( n - 1 ) );
		}
	}
}
