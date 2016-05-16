using UnityEngine;
using System.Collections;
using RAIN;

public class CameraController : MonoBehaviour
{
	SmoothFollow smoothFollow;
	Transform emptyTrans;
	RAIN.Core.AIRig[] AiRig;
	public float speed = 10.0f;

	void Start()
	{
		smoothFollow = Camera.main.transform.GetComponent<SmoothFollow> ();
		smoothFollow.target = emptyTrans;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//RaycastHit hit;
			if (smoothFollow.target == emptyTrans) {
				Ray ray = (Camera.main.ScreenPointToRay (Input.mousePosition)); //create the ray
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.tag == "Defender") { //did we hit the defender?	
						
						AiRig = hit.transform.GetComponents<RAIN.Core.AIRig> ();
						//print(hit.transform.GetComponents<RAIN.Core.AIRig> ());

						//Turn on smoothfollow script on the hit target
						smoothFollow.target = hit.transform;
						for (int i = 0; i < AiRig.Length; i++) {
							AiRig[i].enabled = false;
						}

					}
				}
			}
		}
		else if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			Cursor.visible = true;
			smoothFollow.target = emptyTrans;
			if (AiRig != null) {
				for (int i = 0; i < AiRig.Length; i++) {
					AiRig [i].enabled = false;
				}
			}
		}
			
		Vector3 mouseOffset = new Vector3(-Input.GetAxis( "Mouse Y" ), Input.GetAxis( "Mouse X" ), 0f );
		Camera.main.transform.Rotate( mouseOffset * 2, Space.Self );

		Vector3 lookDir = Camera.main.transform.position + Camera.main.transform.forward;
		Camera.main.transform.LookAt( lookDir, Vector3.up );

		Vector3 moveDir = new Vector3( Input.GetAxis( "Horizontal" ), 0, Input.GetAxis( "Vertical" ) );
		transform.Translate( speed * Time.deltaTime * moveDir );

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
