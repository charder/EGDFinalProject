using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseCamera : MonoBehaviour {
	public ShowcaseScript[] showcasePoints;
	int currentShowcase; //the current showcase the camera is on
	public float moveSeconds; //how many seconds of movement between showcase locations
	float moveTime; //time before another input is possible, also dictates the rate of movement
	Quaternion currentRot; //rotation used in conjunction with looking at the current showcase

	// Use this for initialization
	void Start () {
		moveTime = moveSeconds;
		
	}
	
	// Update is called once per frame
	void Update () {
		//Gross way of finding new quaternion to look towards
		Vector3 showcasePoint = showcasePoints[currentShowcase].transform.position;
		currentRot = transform.rotation;
		transform.LookAt (showcasePoint);
		Quaternion newRot = transform.rotation;
		transform.rotation = currentRot;
		transform.rotation = Quaternion.Lerp (newRot, currentRot, moveTime/moveSeconds);
		//transform.rotation = Quaternion.RotateTowards (currentRot, newRot, 45 * Time.deltaTime);
		Vector3 newPoint = showcasePoint + showcasePoints[currentShowcase].transform.right * 35 + showcasePoints[currentShowcase].transform.up * 10;
		transform.position += (newPoint - transform.position) / moveSeconds * Time.deltaTime;

		//Handle moving around the showcase (moveTime is a delay for moving between timeline points)
		if (Input.GetKey (KeyCode.D)) {
			if (ManageMovement()) {
				currentShowcase++;
				if (currentShowcase >= showcasePoints.Length) {
					currentShowcase = 0;
				}
			}
		}
		if (Input.GetKey (KeyCode.A)) {
			if (ManageMovement()) {
				currentShowcase--;
				if (currentShowcase < 0) {
					currentShowcase = showcasePoints.Length - 1;
				}
			}
		}
		if (moveTime > 0) {
			moveTime -= Time.deltaTime;
		}
	}

	bool ManageMovement() {
		if (moveTime <= 0) {
			moveTime = moveSeconds;
			return true;
		}
		return false;
	}
}
