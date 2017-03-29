using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour {
	Rigidbody myBody;
	//Keyboard Inputs
	bool forwardKey;
	bool backKey;
	bool leftKey;
	bool rightKey;
	bool slowKey;

	//Controller Inputs
	bool forwardButton; //drive gear
	bool backButton; //reverse gear
	float driveTrigger; 
	float brakeTrigger;

	// Use this for initialization
	void Awake() {
		myBody = GetComponent<Rigidbody> ();
	}
	void Update() {
		//Keyboard Inputs
		forwardKey = Input.GetKey(KeyCode.W);
		backKey = Input.GetKey(KeyCode.S);
		leftKey = Input.GetKey(KeyCode.A);
		rightKey = Input.GetKey (KeyCode.D);
		slowKey = Input.GetKey (KeyCode.LeftShift);

		//Controller Inputs
		forwardButton = Input.GetButton("A");
		backButton = Input.GetButton ("B");
		driveTrigger = Input.GetAxis ("RT");
		brakeTrigger = Input.GetAxis ("LT");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Ray ray = new Ray (transform.position, transform.right);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 3.5f)) {
			float proportionalHeight = (3.5f - hit.distance) / 3.5f;
			Vector3 appliedHoverHeight = transform.right * proportionalHeight * 65f;
		}
		if (forwardKey) {
			print ("moving forward");
			myBody.AddRelativeForce (-10f, 0f, 0,ForceMode.Acceleration);
		}
		if (backKey) {
			myBody.AddRelativeForce (10f, 0f, 0,ForceMode.Acceleration);
		}
		if (leftKey) {
			myBody.AddRelativeTorque (0, 0, -0.2f,ForceMode.VelocityChange);
		}
		if (rightKey) {
			myBody.AddRelativeTorque (0, 0, 0.2f, ForceMode.VelocityChange);
		}
	}
}
