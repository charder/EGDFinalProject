using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShowcaseCamera : CarDataPassage {
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
	public GameObject createResponseUI;
	InputField createTweetField; //input field used for writing a tweet, need reference for easier checking
	InputField createReplyingField; //input field used for replying
	bool activeTweetText; //whether or not the player is editing the text

	int tweetOptionSelection; //iterator for tweetOptionCovers, and also determines which action is passed to the car upon creation
	public GameObject[] tweetOptionCovers; //covers to highlight the current action the player will be performing if they start now

	StoreTwitterData twitDatRef; //reference to the twitter data stuff for timeline visuals
	public GameObject[] playerCars; //prefabs for the player cars 


    public GameObject replyText = null;

	// Use this for initialization
	void Start () {
		moveTime = moveSeconds;
		changeCar ();
		createTweetField = createTweetUI.GetComponentInChildren<InputField>();
		createReplyingField = createResponseUI.GetComponentInChildren<InputField> ();
		createTweetUI.SetActive (false);

		twitDatRef = FindObjectOfType<StoreTwitterData> (); 

		//Handle option selection (0 = Like, 1 = Retweet, 2 = Reply)
		tweetOptionSelection = 0;
		tweetOptionCovers [tweetOptionSelection].SetActive (false); 
	}
	
	// Update is called once per frame
	void Update () {
		//Gross way of finding new quaternion to look towards
		Vector3 showcasePoint;
		if (creatingTweet) {
			showcasePoint = showcaseCenter.transform.position;
		} else {
			showcasePoint = showcasePoints[currentShowcase].transform.position;
			InputField tmpField = replyText.GetComponent<InputField> ();
			if (!tmpField.text.Contains ("@" + showcasePoints [currentShowcase].tweetStuff.tweetHandle + " ")) {
				tmpField.text = "@" + showcasePoints [currentShowcase].tweetStuff.tweetHandle + " ";
				tmpField.caretPosition = tmpField.text.Length;
            }
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
			if (!createReplyingField.isFocused) {
				/*
				//Switch to creating new tweet
				if (Input.GetKey (KeyCode.Q)) {
					if (ManageMovement ()) {
						creatingTweet = true;
						createTweetUI.SetActive (true);
						createResponseUI.SetActive (false);
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
				if (Input.GetKey (KeyCode.W)) {
					if (ManageMovement ()) {
						tweetOptionCovers [tweetOptionSelection].SetActive (true);
						tweetOptionSelection--;
						if (tweetOptionSelection < 0) {
							tweetOptionSelection = tweetOptionCovers.Length - 1;
						}
						tweetOptionCovers [tweetOptionSelection].SetActive (false);
					}
				}
				if (Input.GetKey (KeyCode.S)) {
					if (ManageMovement ()) {
						tweetOptionCovers [tweetOptionSelection].SetActive (true);
						tweetOptionSelection++;
						if (tweetOptionSelection >= tweetOptionCovers.Length) {
							tweetOptionSelection = 0;
						}
						tweetOptionCovers [tweetOptionSelection].SetActive (false);
					}
				}
				*/
				if (Input.GetKey (KeyCode.Tab)) {
					if (ManageMovement ()) {
						EventSystem.current.SetSelectedGameObject (createReplyingField.gameObject, null);
					}
				}
			} else {
				if (Input.GetKey (KeyCode.Tab)) {
					if (ManageMovement ()) {
						EventSystem.current.SetSelectedGameObject (null, null);
					}
				}
			}
		} else {
			if (!createTweetField.isFocused) {
				/*
				if (Input.GetKey (KeyCode.Q)) {
					if (ManageMovement ()) {
						creatingTweet = false;
						createTweetUI.SetActive (false);
						createResponseUI.SetActive (true);
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
				*/
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

	public void SetTwitterTimelineData(StoreTwitterData twitterRef) {
		twitDatRef = twitterRef;
		for (int i = 0; i < showcasePoints.Length; i++) {
			ShowcaseScript showcaseP = showcasePoints [i].GetComponent<ShowcaseScript> ();
			showcaseP.tweetStuff = twitDatRef.twitterTimeline [i];
			showcaseP.DisplayTweet ();
		}
	}

	public void TweetOptionCreate() {
		if (ManageMovement ()) {
			if (!creatingTweet) {
				creatingTweet = true;
				createTweetUI.SetActive (true);
				createResponseUI.SetActive (false);
			}
		}
	}

	public void TweetOptionSelection(int num) {
		if (ManageMovement ()) {
			if (creatingTweet) {
				creatingTweet = false;
				createTweetUI.SetActive (false);
				createResponseUI.SetActive (true);
			}
			tweetOptionCovers [tweetOptionSelection].SetActive (true);
			tweetOptionSelection = num;
			tweetOptionCovers [tweetOptionSelection].SetActive (false);
		}
	}

	public void ChangeCarColor(int rightleft) {
		if (rightleft == 0) {
			carColor++;
			if (carColor >= carColorOptions.Length) {
				carColor = 0;
			}
		} else {
			carColor--;
			if (carColor < 0) {
				carColor = carColorOptions.Length - 1;
			}
		}
	}

	public void ChangeCarModel(int rightleft) {
		if (rightleft == 0) {
			carModel++;
			if (carModel >= carModelOptions.Length) {
				carModel = 0;
			}
			changeCar ();
		} else {
			carModel--;
			if (carModel < 0) {
				carModel = carModelOptions.Length - 1;
			}
			changeCar ();
		}
	}

	public void MoveOnTimeline(int leftright) {
		if (ManageMovement ()) {
			if (leftright == 0) {
				currentShowcase++;
				if (currentShowcase >= showcasePoints.Length) {
					currentShowcase = 0;
				}
			} else {
				currentShowcase--;
				if (currentShowcase < 0) {
					currentShowcase = showcasePoints.Length - 1;
				}
			}
		}
	}



	//Which action is used when the game is started
	public void PassDataToCar(int option) {
		GameObject empty = new GameObject ();
		TwitterPostInfo relevantInfo = empty.AddComponent<TwitterPostInfo> ();
		DontDestroyOnLoad (empty);
		relevantInfo.postType = option;
		switch (option) {
		//Tweet
		case 0:
			relevantInfo.tweetBody = createTweetField.text;
			GameObject playerCarSpawn = (GameObject)Instantiate (playerCars [carModel]);
			DrivingScriptTest carDrive = playerCarSpawn.GetComponent<DrivingScriptTest> ();
			MeshRenderer carMesh = playerCarSpawn.GetComponentInChildren<MeshRenderer> ();
			//change car color
			Material[] updatedMat = carMesh.materials;
			if (carModel == 3) {
				updatedMat [1] = carColorOptions [carColor];
			} else {
				updatedMat [0] = carColorOptions [carColor];
			}
			carMesh.materials = updatedMat;

			//carDrive.carData = relevantInfo;
			DontDestroyOnLoad(playerCarSpawn);
			SceneManager.LoadScene("ModifiedCity");
			break;
		//Retweet
		case 1:
			ShowcaseScript currentTweetShow = showcasePoints [currentShowcase].GetComponent<ShowcaseScript> ();
			TwitterTweetPlus currentTimelineInfo = currentTweetShow.tweetStuff; //gets a reference to the info from the tweet we're dealing with
			relevantInfo.replyID = currentTimelineInfo.replyID;
			switch (tweetOptionSelection) {
			//Like
			case 0:
				relevantInfo.postType = 1;
				break;
			//Retweet
			case 1:
				relevantInfo.postType = 2;
				break;
			//Reply
			case 2:
				relevantInfo.postType = 3;
				relevantInfo.tweetBody = createReplyingField.text;
				break;
			}
			GameObject playerCarLoad = (GameObject)Instantiate (playerCars [currentTweetShow.carModelNum]);

			MeshRenderer carMeshLoad = playerCarLoad.GetComponentInChildren<MeshRenderer> ();
			//change car color
			Material[] updatedMatLoad = carMeshLoad.materials;
			if (currentTweetShow.carModelNum == 3) {
				updatedMatLoad [1] = carColorOptions [currentTweetShow.carColorNum];
			} else {
				updatedMatLoad [0] = carColorOptions [currentTweetShow.carColorNum];
			}
			carMeshLoad.materials = updatedMatLoad;

			DrivingScriptTest carDriving = playerCarLoad.GetComponent<DrivingScriptTest> ();
			//carDriving.carData = relevantInfo;
			DontDestroyOnLoad(playerCarLoad);
			SceneManager.LoadScene("ModifiedCity");
			break;
		}
	}
}
