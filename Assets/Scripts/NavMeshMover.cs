using UnityEngine;
using System.Collections;

public class NavMeshMover : MonoBehaviour 
{
    // simple point and click navmesh mover script
	private NavMeshAgent m_agent;

	private GameObject m_desitinationMarker;

	// Use this for initialization
	void Start () 
    {
		m_desitinationMarker = GameObject.CreatePrimitive( PrimitiveType.Sphere );
		m_desitinationMarker.SetActive( false );
		m_desitinationMarker.transform.localScale = Vector3.one * 0.5f;
		m_desitinationMarker.GetComponent<Collider>().enabled = false;
		m_agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		if( Input.GetMouseButtonDown( 1 ) )
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;

			if( Physics.Raycast( ray, out hit, 100 ) )
			{
				m_agent.SetDestination( hit.point );
				m_desitinationMarker.transform.position = hit.point;
				m_desitinationMarker.SetActive( true );
			}
		}
	}
}
