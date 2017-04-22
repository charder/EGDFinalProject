using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseScript : MonoBehaviour {
	public GameObject myVehicle;
	ShowcaseCamera showcaseRef;
	// Use this for initialization
	void Awake () {
		showcaseRef = FindObjectOfType<ShowcaseCamera> (); //get reference to showcaseCamera
		int rollM = Random.Range(0,showcaseRef.carModelOptions.Length);
		Transform vehicleSpawn = myVehicle.transform;
		Destroy (myVehicle);
		myVehicle = (GameObject)Instantiate (showcaseRef.carModelOptions [rollM], vehicleSpawn.position, vehicleSpawn.rotation);

		int rollC = Random.Range(0,showcaseRef.carColorOptions.Length);
		//Color and Model update on the car
		MeshRenderer carMesh = myVehicle.GetComponent<MeshRenderer>();
		Material[] newMats = carMesh.materials;
		if (newMats [0].name == "CarBumper (Instance)") {
			newMats [1] = showcaseRef.carColorOptions [rollC];
		} else {
			newMats [0] = showcaseRef.carColorOptions [rollC];
		}
		carMesh.materials = newMats;
		myVehicle.transform.Rotate (Vector3.forward * Random.Range(-180,180)); //give a pseudo random range of rotation to start
	}
	
	// Update is called once per frame
	void Update () {
		myVehicle.transform.Rotate (Vector3.forward * 5 * Time.deltaTime);
	}
}
