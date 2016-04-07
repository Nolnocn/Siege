using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {

	int m_resourceLeft = 100;
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
	}

	public int GatherResource( int amtToGather )
	{
		if( canGather && m_resourceLeft > 0 )
		{
			if( amtToGather > m_resourceLeft )
			{
				return m_resourceLeft;
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
