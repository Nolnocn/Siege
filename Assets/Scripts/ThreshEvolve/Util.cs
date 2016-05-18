using System;

namespace ThreshEvolve
{
	/* Put the Random object here because only need one and it's
	 * used everywhere.  Could have just put it in MainClass, but hey,
	 * utility classes are a thing, so...
	 */
	public static class Util
	{
		// Set up the Random generator here
		public static Random rand = new Random();
	}
}

