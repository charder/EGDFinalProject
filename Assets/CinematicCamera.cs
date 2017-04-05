using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour {
	bool moving = false;
	public Vector3 direction; //which direction to move the camera
	public float speed; //speed to move camera at (in world space coordinates per second)
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Pause/Resume
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (moving) {
				moving = false;
			} else {
				moving = true;
			}
		}
		if (moving) {
			transform.position = transform.position + direction * speed * Time.deltaTime;
		}
	}
}
