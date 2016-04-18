using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour {

    private bool m_holdingResources = false;
    private int m_numResHolding = 0;
    public int gatherRate = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (m_numResHolding == 0)
        {
            m_holdingResources = false;
        }
        else
        {
            m_holdingResources = true;
        }
	}

    void RecieveResource(int res)
    {
        m_numResHolding = res;
    }

    int GiveResource()
    {
        int temp = m_numResHolding;
        m_numResHolding = 0;
        return temp;
    }
}
