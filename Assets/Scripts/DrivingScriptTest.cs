using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScriptTest: MonoBehaviour {
	public float speed;
	float speedCurrent;
	float topSpeed;
	float topSpeedMod;
	public float rotateSpeed;
	float rotateCurrent;
	float topRotate;
	float moveDir; //direction of movement (forward/backward for correct driving control)
	Rigidbody myBody;

	public float groundedDrag;
	public float flyingDrag;

	bool grounded = false; //whether or not the car is on the ground
	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody> ();
		moveDir = 1;
		topSpeed = speed;
		topSpeedMod = topSpeed; // modified top speed (changed by sharp turns)
		topRotate = rotateSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		//JUMPING
		if (grounded && ((Input.GetButtonDown ("Y")) || Input.GetKeyDown (KeyCode.Space))) {
			print ("MIGHT AS WELL JUMP");
			//myBody.AddForce(Vector3.up * 35000,ForceMode.Impulse);
		}
		//Controller Switch Gears
		if (Input.GetKey (KeyCode.W) || Input.GetButton ("A")) {
			moveDir = 1;
		} else if (Input.GetKey (KeyCode.S) || Input.GetButton ("B")) {
			moveDir = -1;
		}

		//Slow over time
		else {
			if (Mathf.Abs(speedCurrent) < 1) {
				speedCurrent = 0;
			} else {
				if (grounded) {
					speedCurrent = (1 - Time.deltaTime * 0.5f) * speedCurrent;
				} else {
					speedCurrent = (1 - Time.deltaTime * 0.3f) * speedCurrent;
				}
			}
		}

		float leftTrigger = Input.GetAxis ("LT");
		if (Input.GetKey (KeyCode.LeftShift)) {
			topRotate = rotateSpeed * 2;
			topSpeedMod = topSpeed * 0.85f;
		} else if (leftTrigger > 0.4f) {
			topRotate = rotateSpeed * (3 * leftTrigger);
			topSpeedMod = topSpeed * (1 - 0.15f * leftTrigger);
		} else {
			topRotate = rotateSpeed;
			topSpeedMod = topSpeed;
		}
		if (Mathf.Abs(speedCurrent) > topSpeedMod) {
			speedCurrent = topSpeedMod * Mathf.Sign(speedCurrent);
		}
		/*
		if (!grounded) {
			Vector3 forwardMovement = new Vector3 (transform.right.x, 0, transform.right.z);
			transform.position += -1 * forwardMovement * Time.deltaTime * speedCurrent;
		} else {
			transform.position += -1 * transform.right * Time.deltaTime * speedCurrent;
		}
		*/
		//myBody.velocity = -1 * forwardMovement * Time.deltaTime * speedCurrent;
		if (transform.rotation.eulerAngles.x < 260 || transform.rotation.eulerAngles.x > 280) {
			print (transform.rotation.eulerAngles.x);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 50 * Time.deltaTime);
		}
	}

	void FixedUpdate() {
		//hover
		if (grounded) {
			myBody.drag = groundedDrag;
			//myBody.AddForce (Vector3.up * 10, ForceMode.Acceleration);
		} else {
			myBody.drag = flyingDrag;
		}
		//Drive
		if (moveDir == 1 && (Input.GetKey (KeyCode.W) || (Input.GetAxis("RT") > 0.4f)) && grounded) {
			if (speedCurrent < topSpeedMod) {
				myBody.AddForce(-transform.right * topSpeedMod,ForceMode.VelocityChange);
				//speedCurrent += Mathf.Min (0.2f * (topSpeed - speedCurrent), 0.5f);
			}
		}
		//Reverse
		else if (moveDir == -1 && (Input.GetKey (KeyCode.S) || (Input.GetAxis("RT") > 0.4f)) && grounded) {
			if (speedCurrent > -topSpeedMod) {
				myBody.AddForce(transform.right * topSpeedMod,ForceMode.VelocityChange);
				//speedCurrent += Mathf.Max (0.2f * (-topSpeed - speedCurrent), -0.5f);
			}
		} 
		//Turn Left
		if (Input.GetKey (KeyCode.A)) {
			myBody.AddTorque (-transform.forward * rotateSpeed,ForceMode.VelocityChange);
			//transform.Rotate (0, 0, Mathf.Sign (speedCurrent) * -topRotate * Time.deltaTime);
		} else if (Input.GetAxis ("LS_X") < -0.4f) {
			myBody.AddTorque (-transform.forward*rotateSpeed*Input.GetAxis("LS_X"));
			//transform.Rotate (0, 0, Input.GetAxis ("LS_X") * Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		}
		//Turn Right
		if (Input.GetKey (KeyCode.D)) {
			myBody.AddTorque (transform.forward * rotateSpeed,ForceMode.VelocityChange);
			//transform.Rotate (0, 0, Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		} else if (Input.GetAxis ("LS_X") > 0.4f) {
			myBody.AddTorque (transform.forward*rotateSpeed*Input.GetAxis("RS_X"));
			//transform.Rotate (0, 0, Input.GetAxis ("LS_X") * Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		}
		//Vector3 forwardMovement = new Vector3 (transform.right.x, 0, transform.right.z);
		//myBody.velocity = -100 * forwardMovement * Time.deltaTime * speedCurrent;
	}

	//determine ground touch (there is a seperate trigger volume below the car that helps with this)
	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Ground") {
			grounded = true;
		}
	}

	void OnTriggerExit(Collider other) {
		grounded = false;
	}
}
