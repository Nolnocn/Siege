using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour 
{
    public int health = 100;
    private bool m_destroyed = false;
    public GameObject m_wall_destroyed;

	// Use this for initialization
	void Start() 
    {
	}
	
	// Update is called once per frame
	void Update() 
    {
        checkHealth();

        if( m_destroyed )
        {
            m_wall_destroyed.SetActive( true );
            gameObject.SetActive( false );
        }
	}

    public void TakeDamage( int damage )
    {
        health -= damage;
    }

    private void checkHealth()
    {
        if( health <= 0 )
        {
            health = 0;
            m_destroyed = true;
        }
    }
}
