using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ RequireComponent( typeof( Rigidbody ) ) ]
[ RequireComponent( typeof( MoveBehavior ) ) ]

public class Character : MonoBehaviour
{
	public FlowField flow; // Temporary for testing
	public Transform target;
	public bool shouldWander = false;

	public float maxForce = 10.0f;
	public float maxSpeed = 2.0f;

	public float separationWt = 10.0f;
	public float flowWt = 0.0f;
	public float seekWt = 5.0f;
	public float wanderWt = 4.0f;
	public float avoidanceWt = 25.0f;

	public List<Transform> m_characters;
	private Obstacle[] m_obstacles;

	private Rigidbody m_rigidbody;
	private MoveBehavior m_movement;

	void Start ()
	{
		m_rigidbody = GetComponent<Rigidbody>();
		m_movement = GetComponent<MoveBehavior>();
		m_movement.SetRigidBody( m_rigidbody );
		m_rigidbody.drag = maxForce / maxSpeed;
	}
	
	void Update ()
	{
		CalculateSteeringForces();

		if( m_rigidbody.velocity != Vector3.zero )
		{
			Vector3 facing = m_rigidbody.velocity;
			facing.y = 0;
			transform.forward = facing;
		}
	}

	public void SetObstacles( Obstacle[] obstacles )
	{
		m_obstacles = obstacles;
	}

	public void SetCharacters( List<Transform> characters )
	{
		m_characters = characters;
	}

	private void CalculateSteeringForces()
	{
		Vector3 force = Vector3.zero;

		if( target != null )
		{
			force += seekWt * m_movement.Seek( target.position );
		}

		Vector3 obsAvoidForce = Vector3.zero;
		foreach( Obstacle o in m_obstacles )
		{
			obsAvoidForce += avoidanceWt * m_movement.AvoidObstacle( o );
		}
		force += obsAvoidForce;

		force += -separationWt * ( m_movement.Separate( m_characters ) - m_rigidbody.velocity );

		if( flow != null )
		{
			force += flowWt * m_movement.Flow( flow );
		}

		if( shouldWander )
		{
			force += wanderWt * m_movement.Wander();
		}

		force = Vector3.ClampMagnitude( force, maxForce );
		m_rigidbody.AddForce( force, ForceMode.Acceleration );
	}
}
