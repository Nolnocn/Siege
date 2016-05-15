using UnityEngine;
using System.Collections;

namespace Bayes
{
	public class BayesAction
	{
		private string m_name;
		private int[] m_counts = new int[ 2 ]; // Outcome (T or F)
		private double[] m_proportions = new double[ 2 ];

		public string Name
		{
			get { return m_name; }
		}

		public BayesAction( string name )
		{
			m_name = name;
		}

		/*public BayesAction( int numTrue, int numFalse )
		{
			m_counts[ 0 ] = numTrue;
			m_counts[ 1 ] = numFalse;

			int total = numTrue + numFalse;
			m_proportions[ 0 ] = (double)numTrue / total;
			m_proportions[ 1 ] = (double)numFalse / total;
		}*/

		public void CalcProps( int total )
		{
			m_proportions[ 0 ] = (double)m_counts[ 0 ] / total;
			m_proportions[ 1 ] = (double)m_counts[ 1 ] / total;
		}

		public int GetCount( int index )
		{
			return m_counts[ index ];
		}

		public int[] GetCounts()
		{
			return m_counts;
		}

		public double GetProportion( int index )
		{
			return m_proportions[ index ];
		}

		public double[] GetProportions()
		{
			return m_proportions;
		}

		public override string ToString ()
		{
			string toStringed = string.Format( "[BayesAction: Name={0}]", Name );
			toStringed += "\nCounts True: " + m_counts[ 0 ] + " | False: " + m_counts[ 1 ];
			toStringed += "\nProps  True: " + m_proportions[ 0 ] + " | False: " + m_proportions[ 1 ];
			return toStringed;
		}
	}
}
