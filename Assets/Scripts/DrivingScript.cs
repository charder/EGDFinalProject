using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour {
	public float speed;
	float speedCurrent;
	public float rotateSpeed;
	float rotateCurrent;
	// Use this for initialization
	void Start () {
		speedCurrent = speed;
		rotateCurrent = rotateSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W)) {
			transform.position += -1 * transform.right * Time.deltaTime * speedCurrent;
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.position += transform.right * Time.deltaTime * speedCurrent;
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (0, 0, rotateCurrent*-Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (0, 0, rotateCurrent*Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			rotateCurrent = rotateSpeed * 3;
			speedCurrent = speedCurrent * 0.75f;
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			rotateCurrent = rotateSpeed;
			speedCurrent = speedCurrent / 0.75f;
		}
	}
}
