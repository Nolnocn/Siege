using UnityEngine;
using System.Collections;

public class FlowField : MonoBehaviour
{
	public Transform destination;

	public float width = 10.0f;
	public float length = 10.0f;

	public int xRes = 10;
	public int zRes = 10;

	private Vector3[,] m_flow;
	private float m_cellW;
	private float m_cellL;

	private GameObject[,] gos;

	void Start ()
	{
		m_cellW = width / xRes;
		m_cellL = length / zRes;
		CreateFlowField();
	}

	void Update()
	{
		for(int i = 0; i < xRes; i++)
		{
			for(int j = 0; j < zRes; j++)
			{
				Vector3 pos = gos[ i, j ].transform.position;
				Debug.DrawLine( pos, pos + m_flow[ i, j ] * m_cellW * 0.5f, Color.red );
			}
		}
	}

	public Vector3 GetFlowDirection( Vector3 pos )
	{
		int i = Mathf.FloorToInt( ( pos.x - transform.position.x + width * 0.5f ) / ( width / xRes ) );
		int j = Mathf.FloorToInt( ( pos.z - transform.position.x + length * 0.5f ) / ( length / zRes ) );

		i = Mathf.Clamp( i, 0, xRes );
		j = Mathf.Clamp( j, 0, zRes );

		return m_flow[ i, j ];
	}

	private void CreateFlowField()
	{
		gos = new GameObject[ xRes, zRes ];
		Vector3 origin = transform.position - new Vector3( width * 0.5f, 0.0f, length * 0.5f );
		origin += new Vector3( m_cellW * 0.5f, 0.0f, m_cellL * 0.5f );
		m_flow = new Vector3[ xRes, zRes ];
		for(int i = 0; i < xRes; i++)
		{
			for(int j = 0; j < zRes; j++)
			{
				GameObject go = new GameObject( "Cell " + i + ", " + j );
				go.transform.parent = transform;
				go.transform.position = origin + new Vector3( i * m_cellW, 0.0f, j * m_cellL );
				gos[ i, j ] = go;
				m_flow[ i, j ] = destination.position - ( origin + new Vector3( i * m_cellW, 0.0f, j * m_cellL ) );
				m_flow[ i, j ].Normalize();
			}
		}
	}
}
