using UnityEngine;
using System.Collections;

[ RequireComponent( typeof( Rigidbody ) ) ]
[ RequireComponent( typeof( MoveBehavior ) ) ]

public class Character : MonoBehaviour
{
	public FlowField flow; // Temporary for testing
	public Transform target;

	public float maxForce = 10.0f;
	public float maxSpeed = 2.0f;

	public float separationWt = 10.0f;
	public float flowWt = 0.0f;
	public float seekWt = 5.0f;

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
		Vector3 facing = m_rigidbody.velocity;
		facing.y = 0;
		transform.forward = facing;
		//m_rigidbody.velocity = Vector3.ClampMagnitude( m_rigidbody.velocity, maxSpeed );
	}

	private void CalculateSteeringForces()
	{
		Vector3 force = Vector3.zero;

		//force += -separationWt * ( m_movement.Separate(  ) - m_rigidbody.velocity );

		if( target != null )
		{
			force += seekWt * m_movement.Seek( target.position );
		}

		force += flowWt * m_movement.Flow( flow );

		force = Vector3.ClampMagnitude( force, maxForce );
		m_rigidbody.AddForce( force, ForceMode.Acceleration );
	}
}
