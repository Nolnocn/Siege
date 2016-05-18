using UnityEngine;
using System.Collections;

public class AttackerSpawningScript : MonoBehaviour {

	public GameObject AttackerPrefab;

	float timeToSpawn = -10;
	public float spawnerIncrement = 15;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeToSpawn) {

			GameObject attacker = (GameObject) Instantiate (AttackerPrefab, transform.position, Quaternion.identity);
			timeToSpawn += spawnerIncrement;
		}
	}
}
