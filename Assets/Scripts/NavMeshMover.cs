using UnityEngine;
using System.Collections;

public class NavMeshMover : MonoBehaviour 
{
    // simple point and click navmesh mover script
	private NavMeshAgent m_agent;

	// Use this for initialization
	void Start () 
    {
	
        m_agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
        if( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;

            if( Physics.Raycast( ray, out hit, 100 ) )
            {
                m_agent.SetDestination( hit.point );
            }
        }
	}
}
