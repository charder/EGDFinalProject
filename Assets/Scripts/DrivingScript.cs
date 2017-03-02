﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour {
	public float speed;
	float speedCurrent;
	float topSpeed;
	public float rotateSpeed;
	float rotateCurrent;
	float topRotate;
	// Use this for initialization
	void Start () {
		topSpeed = speed;
		topRotate = rotateSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			if (speedCurrent != topSpeed) {
				speedCurrent += Mathf.Min (0.2f * (topSpeed - speedCurrent), 0.5f);
			}
		} else if (Input.GetKey (KeyCode.S)) {
			if (speedCurrent != -topSpeed) {
				speedCurrent += Mathf.Max (0.2f * (-topSpeed - speedCurrent), -0.5f);
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
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			topRotate = rotateSpeed * 3;
			speedCurrent = speedCurrent * 0.75f;
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			topRotate = rotateSpeed;
			speedCurrent = speedCurrent / 0.75f;
		}
	transform.position += -1 * transform.right * Time.deltaTime * speedCurrent;
	}
}