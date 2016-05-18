using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float hp = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0) {
			print (gameObject.name + ": died at " + Time.time);
			gameObject.SetActive(false);
		}
	}

    public void Heal(int amt)
    {
        if( hp < 100 )
        {
            hp += amt;
        }
    }
}
