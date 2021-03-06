﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour {
	public float speed;
	float speedCurrent;
	float topSpeed;
	float topSpeedMod;
	public float rotateSpeed;
	float rotateCurrent;
	float topRotate;
	float moveDir; //direction of movement (forward/backward for correct driving control)
	Rigidbody myBody;
	TwitterTrend showingTweet; //tweet shown in top-right corner when colliding with other car

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
		if (grounded && ((Input.GetButtonDown ("Y")) || Input.GetKeyDown (KeyCode.Space))) {
			print ("MIGHT AS WELL JUMP");
			myBody.AddForce(Vector3.up * 35000,ForceMode.Impulse);
			Quaternion.FromToRotation(this.transform.rotation.eulerAngles,new Vector3(-90,this.transform.rotation.y,this.transform.rotation.z));
		}
		if (Input.GetKey (KeyCode.W) || Input.GetButton ("A")) {
			moveDir = 1;
		} else if (Input.GetKey (KeyCode.S) || Input.GetButton ("B")) {
			moveDir = -1;
		}

		if (moveDir == 1 && (Input.GetKey (KeyCode.W) || (Input.GetAxis("RT") > 0.4f)) && grounded) {
			if (speedCurrent < topSpeedMod) {
				speedCurrent += Mathf.Min (0.2f * (topSpeed - speedCurrent), 0.5f);
			}
		} else if (moveDir == -1 && (Input.GetKey (KeyCode.S) || (Input.GetAxis("RT") > 0.4f)) && grounded) {
			if (speedCurrent > -topSpeedMod) {
				speedCurrent += Mathf.Max (0.2f * (-topSpeed - speedCurrent), -0.5f);
			}
		} else {
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
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (0, 0, Mathf.Sign (speedCurrent) * -topRotate * Time.deltaTime);
		} else if (Input.GetAxis ("LS_X") < -0.4f) {
			transform.Rotate (0, 0, Input.GetAxis ("LS_X") * Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		}
			
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (0, 0, Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		} else if (Input.GetAxis ("LS_X") > 0.4f) {
			transform.Rotate (0, 0, Input.GetAxis ("LS_X") * Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
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
		Vector3 forwardMovement = new Vector3 (transform.right.x, 0, transform.right.z);
		transform.position += -1 * forwardMovement * Time.deltaTime * speedCurrent;
		if (Mathf.Abs (transform.rotation.x) > 120 || Mathf.Abs (transform.rotation.z) > 30) {
			Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (Mathf.Sign (transform.rotation.eulerAngles.x) * 90, transform.rotation.eulerAngles.y, Mathf.Sign(transform.rotation.eulerAngles.z) * 30),20*Time.deltaTime);
		}
	}

	void FixedUpdate() {
		
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
