﻿using UnityEngine;
using System.Collections;
using Bayes;

public class BayesDeciderRef : MonoBehaviour {
    
    private BayesDecider bd;

	// Use this for initialization
	void Awake () {
        
        bd = new BayesDecider( "RepairTab.txt", "Repair Wall" );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnApplicationQuit()
	{
		bd.Tab2File();
	}

    public BayesDecider getBayesDecider()
    {
        return bd;
    }
}
