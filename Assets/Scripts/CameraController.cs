using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	void Update()
	{
		if( Input.GetMouseButtonDown( 0 ) )
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			//Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
			
		Vector3 mouseOffset = new Vector3(-Input.GetAxis( "Mouse Y" ), Input.GetAxis( "Mouse X" ), 0f );
		Camera.main.transform.Rotate( mouseOffset * 2, Space.Self );
		Vector3 lookDir = Camera.main.transform.position + Camera.main.transform.forward;
		Camera.main.transform.LookAt( lookDir, Vector3.up );

		Vector3 moveDir = new Vector3( Input.GetAxis( "Horizontal" ), 0, Input.GetAxis( "Vertical" ) );
		transform.Translate( 5f * Time.deltaTime * moveDir );

		//StayInBounds();
	}

	/*private void StayInBounds()
	{
		if(transform.position.x > 100) {
			transform.position = new Vector3(100, transform.position.y, transform.position.z);
		}
		else if(transform.position.x < 0) {
			transform.position = new Vector3(0, transform.position.y, transform.position.z);
		}

		if(transform.position.z > 200) {
			transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		}
		else if(transform.position.z < 0) {
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		}

		if(transform.position.y > 10) {
			transform.position = new Vector3(transform.position.x, 10, transform.position.z);
		}
		else if(transform.position.y < 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}*/
}
