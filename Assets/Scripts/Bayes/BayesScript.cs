using UnityEngine;
using System.Collections;
using Bayes;

public class BayesScript : MonoBehaviour {

	//BayesDecision bs;
	BayesDecider bd;

	void Start ()
	{
		// Note: We're using the Bayes namespace up above ^^

		// BayesDecider can handle 2 types of conditions
		// Discrete and Continuous

		// Discrete conditions are conditions with values that are 0-based and count up
		// ie True/False or an enumeration (like the outlook in the Golf example)
		// Discrete values are arbitrary (either 0 or 1 could be true),
		// just be consistent, start it at 0, and don't skip numbers (ie 0,1,2,3...)
		BayesDiscreteCondition outlook = new BayesDiscreteCondition( "Outlook", 3 );
		BayesDiscreteCondition windy = new BayesDiscreteCondition( "Windy" );

		// Continuous conditions are conditions with values that fall within a range
		// The values themselves don't really matter, but I don't think it can handle negative
		BayesContinuousCondition temp = new BayesContinuousCondition( "Temp" );
		BayesContinuousCondition hum = new BayesContinuousCondition( "Humidity" );

		// Here we assemble arrays of the discrete and continuous conditions to pass to the Decider
		// The order for discrete/coninuous conditions MUST be the same as the order in the data file!
		BayesDiscreteCondition[] discs = new BayesDiscreteCondition[ 2 ];
		discs[ 0 ] = outlook;
		discs[ 1 ] = windy;

		BayesContinuousCondition[] conts = new BayesContinuousCondition[ 2 ];
		conts[ 0 ] = temp;
		conts[ 1 ] = hum;

		// Here we create the decider,
		// and tell it what file to use and the name of the outcome (for debug printing)
		bd = new BayesDecider( "GolfTab.txt", "Play Golf" );

		// Then we give it the conditions
		bd.SetConditions( conts, discs  );

		bd.Tab2Screen(); // Prints out the table for reference
		bd.BuildStats(); // Uses the data from the table to calculate values for conditions

		// Take a look at the Golf data file (Assets/Data/GolfTab.txt) to see some of its quirks
		// Discrete values are prefixed by "d" and continuous with a "c"
		// The outcome starts with an "o"
		// The first column in the data file is the outlook, with is an enum (0 = sunny, 1 = overcast, 2 = rainy)
		// The second column is windy, which is boolean (0 = true, 1 = false)
		// The third and fourth column are temperature and humidity which are continuous
		// The final column is the outcome (play or not) which is boolean as well
		// It's important to keep the order consistent and use the correct prefix or it will break!
	}
    
	// For this example, hitting the spacebar will test a situation that is a "shouldn't play"
	// Then it adds the situation to the table as a "should play" to stack the odds
	// Eventually it will start saying yes
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
			Debug.Log( "Test a day that's sunny, windy, temp 50, humidity 90" );

			// This is where we assemble a situation for the Decider to test

			// We need to assemble an array of discrete and continuous values to test
			// Like how the conditions above had to be in the same order as the data file,
			// these values must be in the same order as the conditions that the values represent
			int[] discValues = new int[ 2 ];
			discValues[ 0 ] = 0; // Outlook, 0 = sunny
			discValues[ 1 ] = 0; // Windy, 0 = true

			int[] contValues = new int[ 2 ];
			contValues[ 0 ] = 66; // temperature
			contValues[ 1 ] = 90; // humidity

			// BayesDecider.Decide returns true/false to perform the action based on the values
			if( bd.Decide( contValues, discValues ) ) // <- We pass in the arrays we built above ^
			{
				Debug.Log("Let's play a round!");
			}
			else
			{
				Debug.Log( "Let's stay home." );
			}

			Debug.Log( "Added last case with true outcome" );

			// Now we add an observation with the values and an outcome
			// We have to decide the outcome (ie if a character dies, it was a bad outcome)
			bd.AddObservation( contValues, discValues, 0 );
			bd.Tab2Screen();
			bd.BuildStats(); // Then we rebuild the stats

			// You can also call BayesDecider.Tab2File to save the observation table
			// Just note that it will write over the existing file, so keeping a backup is a good idea
        }
    }
}
