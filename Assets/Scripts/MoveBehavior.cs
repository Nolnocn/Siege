using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveBehavior : MonoBehaviour
{
	public float radius = 1.0f;
	public float maxSpeed = 5.0f;
	public float arriveDist = 1.0f;
	public float separationDist = 5.0f;
	public float obsAvoidDist = 10.0f;

	//bound half widths
	public float boundX = 50.0f;
	public float boundZ = 50.0f;

	//wander variables
	public float wanderRad = 1.0f;
	public float wanderDist = 5.0f;
	public float wanderRand = 3.0f;
	public float wanderAng = 0.2f;

	private Rigidbody m_rigidbody;
	private Vector3 m_dv;
	private int m_frames = 2;
	private int m_stageFrames = 5;

	void Start()
	{
		m_dv = Vector3.zero;
	}

	public void SetRigidBody( Rigidbody rb )
	{
		m_rigidbody = rb;
	}

	public Vector3 Seek( Vector3 targetPos )
	{
		m_dv = targetPos - transform.position;		
		m_dv = m_dv.normalized * maxSpeed;
		m_dv -= m_rigidbody.velocity;
		m_dv.y = 0;
		return m_dv;
	}

	public Vector3 Flee( Vector3 targetPos )
	{
		return -1.0f * Seek( targetPos );
	}

	public Vector3 Pursue( Rigidbody target )
	{
		Vector3 changeInPos = target.velocity * m_frames;
		Vector3 futurPos = target.transform.position + changeInPos;
		m_dv = Seek( futurPos );
		return m_dv;
	}

	public Vector3 Evade( Rigidbody target )
	{
		Vector3 changeInPos = target.velocity * m_frames;
		Vector3 futurPos = target.transform.position + changeInPos;
		m_dv = Flee( futurPos );
		return m_dv;
	}

	// creates an invisible circle a distance in front of
	// the mover and seeks a position along its radius
	public Vector3 Wander()
	{
		Vector3 target = transform.position + transform.forward * wanderDist;
		Quaternion rot = Quaternion.Euler(0, wanderAng, 0);
		Vector3 offset = rot * transform.forward;
		target += offset * wanderRad;
		wanderAng += Random.Range (-wanderRand, wanderRand);
		return Seek(target);
	}

	public Vector3 Arrive( Vector3 targetPos )
	{
		m_dv = targetPos - transform.position;

		float d = m_dv.magnitude;
		m_dv.Normalize();
		if ( d < arriveDist )
		{
			float m = 1 / d;
			m_dv *= m;
		}
		else
		{
			m_dv *= maxSpeed;
		}

		m_dv -= m_rigidbody.velocity;
		m_dv.y = 0;
		return m_dv;
	}

	public Vector3 Separate( List<Transform> others )
	{
		m_dv = Vector3.zero;
		foreach( Transform other in others )
		{
			if( other != transform )
			{
				float dist = Vector3.Distance( transform.position, other.position );
				if( dist < separationDist )
				{
					Vector3 targetPos = Seek( other.position );
					targetPos.Normalize();
					targetPos *= 1 / dist;
					m_dv += targetPos;
				}
			}
		}
		m_dv.Normalize();
		m_dv *= maxSpeed;
		return m_dv;
	}

	public Vector3 AvoidObstacle( Obstacle obs )
	{ 
		m_dv = Vector3.zero;
		float obRadius = obs.Radius;

		Vector3 vecToCenter = obs.Position - transform.position;
		vecToCenter.y = 0;
		float dist = vecToCenter.magnitude;

		if ( dist > obsAvoidDist + obRadius + radius )
		{
			return Vector3.zero;
		}

		if ( Vector3.Dot( vecToCenter, transform.forward ) < 0 )
		{
			return Vector3.zero;
		}

		float rightDotVTC = Vector3.Dot( vecToCenter, transform.right );

		if ( Mathf.Abs( rightDotVTC ) > radius + obRadius )
		{
			return Vector3.zero;
		}

		if ( rightDotVTC > 0 )
		{
			m_dv += transform.right * -maxSpeed * obsAvoidDist / dist;
		}
		else
		{
			m_dv += transform.right * maxSpeed * obsAvoidDist / dist;
		}
		return m_dv;	
	}

	public Vector3 FollowLeader( Transform leader )
	{
		Vector3 followPos = leader.position - ( leader.forward * separationDist );
		return Arrive( followPos );
	}

	public Vector3 Flow( FlowField flow )
	{
		m_dv = Vector3.zero;
		m_dv = flow.GetFlowDirection( transform.position ) * maxSpeed;
		m_dv.y = 0;

		return m_dv;
	}

	public bool OffStage( Vector3 center )
	{
		Vector3 changeInPos = m_rigidbody.velocity * m_stageFrames;
		Vector3 futurePos = m_rigidbody.position + changeInPos;

		if (futurePos.x < center.x - boundX) 
		{
			return true;
		} 
		else if (futurePos.x > center.x + boundX) 
		{
			return true;
		}
		else if (futurePos.z < center.z - boundZ) 
		{
			return true;
		}
		else if (futurePos.z > center.z + boundZ) 
		{
			return true;
		}

		return false;
	}

	// Path Following
	public Vector3 PathFollow( Path path )
	{
		// Adapted from Shiffman's Path Following
		Vector3 predict = m_rigidbody.velocity;
		predict.Normalize();
		predict *= maxSpeed;
		Vector3 predictLoc = transform.position + predict;

		Vector3 normal;
		Vector3 target = Vector3.zero;
		float worldRecord = 1000000;

		for( int i = 0; i < path.Waypoints.Length; i++ )
		{
			Vector3 a = path.Waypoints[ i % path.Waypoints.Length ].position;
			Vector3 b = path.Waypoints[ (i + 1) % path.Waypoints.Length ].position;

			Vector3 normalPoint = GetNormalPoint( predictLoc, a, b );

			Vector3 dir = b - a;

			if ( normalPoint.x < Mathf.Min( a.x, b.x ) || normalPoint.x > Mathf.Max( a.x, b.x ) 
				|| normalPoint.z < Mathf.Min( a.z, b.z ) || normalPoint.z > Mathf.Max( a.z, b.z ) )
			{
				normalPoint = b;

				a = path.Waypoints[ ( i + 1 ) % path.Waypoints.Length ].position;
				b = path.Waypoints[ ( i + 2 ) % path.Waypoints.Length ].position;
				dir = b - a;
			}

			float d = Vector3.Distance( predictLoc, normalPoint );

			if ( d < worldRecord )
			{
				worldRecord = d;
				normal = normalPoint;

				dir.Normalize();

				dir *= maxSpeed;
				target = normal;
				target += dir;

			}
		}

		if ( worldRecord > path.Radius )
		{
			return Seek( target );
		} 
		else
		{
			return Vector3.zero;
		}
	}

	private Vector3 GetNormalPoint( Vector3 p, Vector3 a, Vector3 b )
	{
		Vector3 ap = p - a;

		Vector3 ab = b - a;
		ab.Normalize();

		ab *= Vector3.Dot( ap, ab );
		Vector3 normalPoint = a + ab;
		return normalPoint;
	}
}
