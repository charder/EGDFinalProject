using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour {
	public float speed;
	float speedCurrent;
	float topSpeed;
	public float rotateSpeed;
	float rotateCurrent;
	float topRotate;
	float moveDir; //direction of movement (forward/backward for correct driving control)

	bool grounded = false; //whether or not the car is on the ground
	// Use this for initialization
	void Start () {
		moveDir = 1;
		topSpeed = speed;
		topRotate = rotateSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W) && grounded) {
			if (speedCurrent != topSpeed) {
				speedCurrent += Mathf.Min (0.2f * (topSpeed - speedCurrent), 0.5f);
				moveDir = 1;
			}
		} else if (Input.GetKey (KeyCode.S) && grounded) {
			if (speedCurrent != -topSpeed) {
				speedCurrent += Mathf.Max (0.2f * (-topSpeed - speedCurrent), -0.5f);
				moveDir = -1;
			}
		} else {
			if (Mathf.Abs(speedCurrent) < 1) {
				speedCurrent = 0;
			} else {
				speedCurrent = 0.95f * speedCurrent;
			}
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (0, 0, Mathf.Sign(speedCurrent)*-topRotate*Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (0, 0, Mathf.Sign(speedCurrent)*topRotate*Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.LeftShift) && grounded) {
			topRotate = rotateSpeed * 3;
			speedCurrent = speedCurrent * 0.75f;
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			topRotate = rotateSpeed;
			speedCurrent = speedCurrent / 0.75f;
		}
	transform.position += -1 * transform.right * Time.deltaTime * speedCurrent;
	}

	void FixedUpdate() {
		
	}

	//determine ground touch (there is a seperate trigger volume below the car that helps with this)
	void OnTriggerStay(Collider other) {
		if (other.gameObject != this.gameObject) {
			grounded = true;
		}
	}

	void OnTriggerExit(Collider other) {
		grounded = false;
	}
}
