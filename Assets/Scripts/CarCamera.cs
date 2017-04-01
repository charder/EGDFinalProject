using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour {
	public GameObject car;
	Vector3 localTransform; // position that must be maintained behind car
	//The following rotates the camera every so often
	bool changeRotation;
	float rotateTime;
	public float rotSpeed; //speed of rotation in degrees per second
	public Shader flatShader;
	// Use this for initialization
	void Start () {
		GetComponent<Camera> ().RenderWithShader (flatShader, "OnPostRender");
		localTransform = transform.position - car.transform.position;
		changeRotation = false;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = car.transform.position - transform.forward * 14 + transform.up * 2f;
		Vector3 newRot = new Vector3 (car.transform.rotation.eulerAngles.x, car.transform.rotation.eulerAngles.y - 90, car.transform.rotation.eulerAngles.z);
		transform.rotation = Quaternion.RotateTowards (transform.rotation,Quaternion.Euler(newRot), rotSpeed * Time.deltaTime);

		/*
		float carRot = (car.transform.localRotation.eulerAngles.y - 90);
		if (carRot < 0) {
			carRot = 360 + carRot;
		}
		float myRot = transform.localRotation.eulerAngles.y;
		float tmpRot = carRot - myRot;
		
		if (Mathf.Abs (tmpRot) > 180) {
			tmpRot = (Mathf.Sign (tmpRot) * 360 - tmpRot);
		}
		
		if (Mathf.Abs (tmpRot) > 180) {
			print (tmpRot);
			tmpRot = Mathf.Sign (tmpRot) * (Mathf.Abs (tmpRot) - 360);
			print (tmpRot);
		}
		//print (carRot + ", " + myRot + ", " + tmpRot);
		//print (car.transform.localRotation.eulerAngles.y - 90 + ", " + transform.localRotation.eulerAngles.y);

		if (Mathf.Abs (tmpRot) > 180) {
			tmpRot = Mathf.Sign (tmpRot) * -1 * (Mathf.Abs (tmpRot) - 180);
		}
		
		float rotAmount = Mathf.Sign(tmpRot)*Mathf.Min (Mathf.Abs (tmpRot), rotSpeed * Time.deltaTime);
		transform.Rotate (0,rotAmount,0);
		*/
		transform.eulerAngles = new Vector3 (10, transform.eulerAngles.y, 0);
	}
}
