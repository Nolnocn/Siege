using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class UserIncterface : MonoBehaviour {

    public int defendersLeftWarning = 5;
    private int numDefenders = 0;
    private int numAttackers = 0;
    private int numBuilders = 0;
    private List<GameObject> defenders, attackers, builders;
    Text UIText;

	// Use this for initialization
	void Start () {
        UIText = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
        defenders = GameObject.FindGameObjectsWithTag("Defender").ToList();
        attackers = GameObject.FindGameObjectsWithTag("Attacker").ToList();
        builders = GameObject.FindGameObjectsWithTag("Builder").ToList();
        numDefenders = defenders.Count;
        numAttackers = attackers.Count;
        numBuilders = builders.Count;

        if (numDefenders <= defendersLeftWarning)
        {
            UIText.text = "<color=#ff0000ff>Defenders: " + numDefenders + "\n</color>Attackers: " + numAttackers + "\nBuilders: " + numBuilders;
        }
        else UIText.text = "Defenders: " + numDefenders + "\nAttackers: " + numAttackers + "\nBuilders: " + numBuilders;

	}
}
