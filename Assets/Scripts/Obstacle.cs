using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
	private float m_radius = 1.0f;

	public float Radius
	{
		get
		{
			return m_radius;
		}
	}

	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
	}

	void Start()
	{
		CapsuleCollider col = GetComponent<CapsuleCollider>();

		if( col != null )
		{
			m_radius = col.radius * 2.0f;
		}
		
		m_radius *= transform.localScale.x;
	}
}
