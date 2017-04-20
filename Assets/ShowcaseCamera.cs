using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowcaseCamera : MonoBehaviour {
	public ShowcaseScript[] showcasePoints;
	int currentShowcase; //the current showcase the camera is on
	public float moveSeconds; //how many seconds of movement between showcase locations
	float moveTime; //time before another input is possible, also dictates the rate of movement
	Quaternion currentRot; //rotation used in conjunction with looking at the current showcase

	bool creatingTweet; //whether or not you're in the center of the environment creating a new tweet
	public ShowcaseScript showcaseCenter; //center point used for creating your own Tweet Car
	public Material[] carColorOptions;
	public GameObject[] carModelOptions;
	int carControl; //which control set are you using for creating the car, used with switch statement
	int carColor; //iterator for carColorOptions
	int carModel; //iterator for carModelOptions
	public GameObject createTweetUI;
	InputField createTweetField; //input field used for writing a tweet, need reference for easier checking
	bool activeTweetText; //whether or not the player is editing the text

	// Use this for initialization
	void Start () {
		moveTime = moveSeconds;
		changeCar ();
		createTweetField = GetComponentInChildren<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		//Gross way of finding new quaternion to look towards
		Vector3 showcasePoint;
		if (creatingTweet) {
			showcasePoint = showcaseCenter.transform.position;
		} else {
			showcasePoint = showcasePoints[currentShowcase].transform.position;
		}
		currentRot = transform.rotation;
		transform.LookAt (showcasePoint);
		Quaternion newRot = transform.rotation;
		transform.rotation = currentRot;
		transform.rotation = Quaternion.Lerp (newRot, currentRot, moveTime/moveSeconds);
		//transform.rotation = Quaternion.RotateTowards (currentRot, newRot, 45 * Time.deltaTime);
		Vector3 newPoint;
		if (creatingTweet) {
			newPoint = showcasePoint + showcaseCenter.transform.right * 35 + showcaseCenter.transform.up * 10;
		} else {
			newPoint = showcasePoint + showcasePoints[currentShowcase].transform.right * 35 + showcasePoints[currentShowcase].transform.up * 10;
		}
		transform.position += (newPoint - transform.position) / moveSeconds * Time.deltaTime;

		//Handle moving around the showcase (moveTime is a delay for moving between timeline points)
		if (!creatingTweet) {
			//Switch to creating new tweet
			if (Input.GetKey (KeyCode.Q)) {
				if (ManageMovement ()) {
					creatingTweet = true;
				}
			}
			if (Input.GetKey (KeyCode.D)) {
				if (ManageMovement ()) {
					currentShowcase++;
					if (currentShowcase >= showcasePoints.Length) {
						currentShowcase = 0;
					}
				}
			}
			if (Input.GetKey (KeyCode.A)) {
				if (ManageMovement ()) {
					currentShowcase--;
					if (currentShowcase < 0) {
						currentShowcase = showcasePoints.Length - 1;
					}
				}
			}
		} else {
			if (!createTweetField.isFocused) {
				if (Input.GetKey (KeyCode.Q)) {
					if (ManageMovement ()) {
						creatingTweet = false;
						createTweetUI.SetActive (false);
					}
				}
				if (Input.GetKey (KeyCode.D)) {
					if (ManageMovement ()) {
						carColor++;
						if (carColor >= carColorOptions.Length) {
							carColor = 0;
						}
					}
				}
				if (Input.GetKey (KeyCode.S)) {
					if (ManageMovement ()) {
						carModel++;
						if (carModel >= carModelOptions.Length) {
							carModel = 0;
						}
						changeCar ();
					}
				}
				if (Input.GetKey (KeyCode.A)) {
					if (ManageMovement ()) {
						carColor--;
						if (carColor < 0) {
							carColor = carColorOptions.Length - 1;
						}
					}
				}
				if (Input.GetKey (KeyCode.W)) {
					if (ManageMovement ()) {
						carModel--;
						if (carModel < 0) {
							carModel = carModelOptions.Length - 1;
						}
						changeCar ();
					}
				}
				if (Input.GetKey (KeyCode.Tab)) {
					if (ManageMovement ()) {
						EventSystem.current.SetSelectedGameObject (createTweetField.gameObject, null);
					}
				}
			} else {
				if (Input.GetKey (KeyCode.Tab)) {
					if (ManageMovement ()) {
						EventSystem.current.SetSelectedGameObject (null, null);
					}
				}
			}

			//Color and Model update on the car
			MeshRenderer carMesh = showcaseCenter.myVehicle.GetComponent<MeshRenderer>();
			Material[] newMats = carMesh.materials;
			if (newMats [0].name == "CarBumper (Instance)") {
				newMats [1] = carColorOptions [carColor];
			} else {
				newMats [0] = carColorOptions [carColor];
			}
			carMesh.materials = newMats;

			if (moveTime <= 0 && !createTweetUI.activeInHierarchy) {
				createTweetUI.SetActive (true);
				createTweetField = GetComponentInChildren<InputField>();
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

	//Script used when creating a custom car in the  center showcase
	void changeCar() {
		Transform vehicleSpawn = showcaseCenter.myVehicle.transform;
		Destroy (showcaseCenter.myVehicle);
		showcaseCenter.myVehicle = (GameObject)Instantiate (carModelOptions [carModel], vehicleSpawn.position, vehicleSpawn.rotation);

	}
}
