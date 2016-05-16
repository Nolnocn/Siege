using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {

	private int m_resourceLeft = 100;
    public int health = 100;
	bool canGather = true;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if( m_resourceLeft <= 0 )
		{
			m_resourceLeft = 0;
			canGather = false;
		}
        health = m_resourceLeft;
	}

	public int GatherResource( int amtToGather )
	{
		if( canGather && m_resourceLeft > 0 )
		{
			if( amtToGather > m_resourceLeft )
			{
				int temp = m_resourceLeft;
				m_resourceLeft = 0;
				return temp;
			}
			else
			{
				m_resourceLeft -= amtToGather;
				return amtToGather;
			}
		}
		else
		{
			return 0;
		}
	}
}
