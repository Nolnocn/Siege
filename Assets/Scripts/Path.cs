using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour
{
	public float radius = 2.0f;

	private Transform[] m_waypoints;

	public Transform[] Waypoints
	{
		get
		{
			return m_waypoints;
		}
	}

	public float Radius
	{
		get
		{
			return radius;
		}
	}

	void Start ()
	{
		m_waypoints = new Transform[ transform.childCount ];
		CreatePath();
	}

	private void CreatePath()
	{
		for( int i = 0; i < m_waypoints.Length; i++ )
		{
			m_waypoints[i] = transform.FindChild( "Waypoint" + ( i + 1 ) );
		}
	}
}
