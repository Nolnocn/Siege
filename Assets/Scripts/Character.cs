using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public float wanderWt = 4.0f;
	public float avoidanceWt = 25.0f;

	private Rigidbody m_rigidbody;
	private MoveBehavior m_movement;

	//List<GameObject> obstacles;
	public GameObject[] obstacles;
	void Start ()
	{
		m_rigidbody = GetComponent<Rigidbody>();
		m_movement = GetComponent<MoveBehavior>();
		m_movement.SetRigidBody( m_rigidbody );
		m_rigidbody.drag = maxForce / maxSpeed;
		//obstacles = new List<GameObject> ();
		obstacles = GameObject.FindGameObjectsWithTag ("obstacle");
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
			//print (seekWt * m_movement.Seek( target.position ));
		}
		Vector3 obsAvoidForce = Vector3.zero;
		foreach (GameObject o in obstacles) {
			
			obsAvoidForce += avoidanceWt * m_movement.AvoidObstacle (o.transform.position);
			//print("checking to avoid: " + o.name);

		}
		//print (obsAvoidForce);
		force += obsAvoidForce;


		// commented out flow to test wander.
		//force += flowWt * m_movement.Flow( flow );
		force += wanderWt * m_movement.Wander();

		force = Vector3.ClampMagnitude( force, maxForce );
		m_rigidbody.AddForce( force, ForceMode.Acceleration );


	}
}
