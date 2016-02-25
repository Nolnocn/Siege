using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour
{
	public GameObject wall;
	public GameObject destroyedObstacle;

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			wall.SetActive( !wall.activeInHierarchy );
			destroyedObstacle.SetActive( !destroyedObstacle.activeInHierarchy );
		}
	}
}
