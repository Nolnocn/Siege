using UnityEngine;
using System.Collections;

public class GetComponents : MonoBehaviour {
	
	public NavMeshAgent m_agent;

	// Use this for initialization
	void Start () {
		m_agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
