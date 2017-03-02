using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour {
	public GameObject car;
	Vector3 localTransform; // position that must be maintained behind car
	// Use this for initialization
	void Start () {
		localTransform = transform.position - car.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = car.transform.position - transform.forward * 18 + transform.up * 3;;
		float tmpRot = (car.transform.localRotation.eulerAngles.y - 90) - transform.localRotation.eulerAngles.y;
		print (tmpRot);
		transform.Rotate (0,tmpRot,0);
		transform.eulerAngles = new Vector3 (21, transform.eulerAngles.y, 0);
	}
}
