using UnityEngine;
using System.Collections;
using Bayes;

public class BayesDeciderRef : MonoBehaviour {

    private BayesDecider bd;

	// Use this for initialization
	void Start () {
        
        bd = new BayesDecider( "RepairTab.txt", "Repair Wall" );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public BayesDecider getBayesDecider()
    {
        return bd;
    }
}
