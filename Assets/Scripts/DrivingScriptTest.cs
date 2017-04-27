using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingScriptTest: MonoBehaviour {
	public float speed;
	float speedCurrent;
	float topSpeed;
	float topSpeedMod;
	public float rotateSpeed;
	//Acceleration and Decceleration paired with speedCurrent and topSpeedMod for driving
	public float acceleration;
	public float decceleration;
	float rotateCurrent;
	float topRotate;
	float moveDir; //direction of movement (forward/backward for correct driving control)
	Rigidbody myBody;

	//Drag when on the ground and in the air (tonight)
	public float groundedDrag;
	public float flyingDrag;
	//Angular drags both when drifting and not
	public float driftAngDrag;
	public float regularAngDrag;

	float boostCurrent; //current time left on boost cooldown
	public float boostCooldown; //overall cooldown of boost (from point of activation)
	public float boostDuration; //duration of boost
	float boostTime; //current boost duration
	public float boostAmount; //amount of force applied in boost

	//Dark covers for forward and reverse to indicate if you're going forward or are in reverse
	public Image forwardCover;
	public Image reverseCover; 
	//Boost bar cover and light bar
	public Image boostCover;
	public Transform boostBar; //this is a transform because the only thing we need from it is it's x-scale
	public Color boostingColor; //color to display while boosting
	public Color boostCooldownColor; //color to display while not boosting

	public AudioSource soundCarSustain;
	float sustainPitch = 0.1f; //pitch of sustain sound (makes it go up and down in rev)
	float sustainPitchBonus = 0; //bonus added to sustain sound from boosting

	public float collisionForce; //required force to play collision sound
	public AudioSource collisionSound;
	public TwitterTrend currentTrend; //trend being displayed in top-right corner
	public TwitterObject cornerTweet; //tweet Object that shows up in the top-right corner
	int tweetNum = 0;

	bool grounded = false; //whether or not the car is on the ground
	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody> ();
		moveDir = 1;
		topSpeed = speed;
		topSpeedMod = topSpeed; // modified top speed (changed by sharp turns and boost)
		topRotate = rotateSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		//JUMPING
		if (grounded && ((Input.GetButtonDown ("Y")) || Input.GetKeyDown (KeyCode.Space))) {
			print ("MIGHT AS WELL JUMP");
			//myBody.AddForce(Vector3.up * 35000,ForceMode.Impulse);
		}
		//Controller Switch Gears
		if (Input.GetKey (KeyCode.W) || Input.GetButton ("A")) {
			moveDir = 1;
			forwardCover.color = new Color (forwardCover.color.r, forwardCover.color.g, forwardCover.color.b, 0);
			reverseCover.color = new Color (reverseCover.color.r, reverseCover.color.g, reverseCover.color.b, 0.6f);
		} else if (Input.GetKey (KeyCode.S) || Input.GetButton ("B")) {
			moveDir = -1;
			forwardCover.color = new Color (forwardCover.color.r, forwardCover.color.g, forwardCover.color.b, 0.6f);
			reverseCover.color = new Color (reverseCover.color.r, reverseCover.color.g, reverseCover.color.b, 0);
		}

		//Slow over time
		/*
		else {
			if (Mathf.Abs(speedCurrent) < 1) {
				speedCurrent = 0;
			} else {
				if (grounded) {
					speedCurrent = (1 - Time.deltaTime * 0.5f) * speedCurrent;
				} else {
					speedCurrent = (1 - Time.deltaTime * 0.3f) * speedCurrent;
				}
			}
		}
		*/

		//drifting/slowing
		float leftTrigger = Input.GetAxis ("LT");
		if (Input.GetKey (KeyCode.LeftShift)) {
			myBody.angularDrag = driftAngDrag;
			//topRotate = rotateSpeed * 2;
			topSpeedMod = topSpeed * 0.85f;
		} else if (leftTrigger > 0.4f) {
			myBody.angularDrag = regularAngDrag + (driftAngDrag - regularAngDrag) * leftTrigger; //modify angulardrag based on 0-1 input
			//topRotate = rotateSpeed * (3 * leftTrigger);
			topSpeedMod = topSpeed * (1 - 0.15f * leftTrigger);
		} else {
			myBody.angularDrag = regularAngDrag;
			//topRotate = rotateSpeed;
			topSpeedMod = topSpeed;
		}

		//rotation adjustment (keep upright whenever possible)
		if (transform.rotation.eulerAngles.x < 250 || transform.rotation.eulerAngles.x > 290) {
			print (transform.rotation.eulerAngles.x);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 50 * Time.deltaTime);
		}

		//boosting
		if (boostCurrent <= 0 && (Input.GetKey (KeyCode.Space) || Input.GetButton ("Y"))) {
			boostCurrent = boostCooldown;
			boostTime = boostDuration;
		} else if (boostCurrent > 0) {
			boostCurrent -= Time.deltaTime;
			boostBar.localScale = new Vector3 (Mathf.Max(0, (boostCooldown - boostCurrent - boostDuration) / (boostCooldown - boostDuration)), boostBar.localScale.y, boostBar.localScale.z);
			boostCover.color = boostCooldownColor;
			sustainPitchBonus = Mathf.Max (0, sustainPitchBonus - 0.4f * Time.deltaTime);
		}
		if (boostTime > 0) {
			boostBar.localScale = new Vector3 (Mathf.Max(0, boostTime / boostDuration), boostBar.localScale.y, boostBar.localScale.z);
			boostCover.color = boostingColor;
			boostTime -= Time.deltaTime;
			sustainPitchBonus = 0.2f;
			topSpeedMod = 1.5f * topSpeed; //50% increase in boost
			//myBody.AddForce (-transform.right * boostAmount * Time.deltaTime * (myBody.drag/groundedDrag), ForceMode.VelocityChange);
		}

		if (Mathf.Abs(speedCurrent) > topSpeedMod) {
			speedCurrent -= Mathf.Min (speedCurrent - topSpeedMod, decceleration*Time.deltaTime);
		}
		soundCarSustain.pitch = sustainPitch;
	}

	void FixedUpdate() {
		//hover
		if (grounded) {
			myBody.drag = groundedDrag;
			//myBody.AddForce (Vector3.up * 10, ForceMode.Acceleration);
		} else {
			myBody.drag = flyingDrag;
		}
		//Drive
		if (moveDir == 1 && (Input.GetKey (KeyCode.W) || (Input.GetAxis ("RT") > 0.4f)) && grounded) {
			if (speedCurrent < topSpeedMod) {
				speedCurrent += Mathf.Min (topSpeedMod - speedCurrent, acceleration*Time.deltaTime);
			}
			sustainPitch = 0.1f + 0.2f * (speedCurrent/topSpeed);
			myBody.AddForce (-transform.right * speedCurrent, ForceMode.VelocityChange);
		}
		//Reverse
		else if (moveDir == -1 && (Input.GetKey (KeyCode.S) || (Input.GetAxis ("RT") > 0.4f)) && grounded) {
			if (speedCurrent > -topSpeedMod) {
				speedCurrent += Mathf.Min (topSpeedMod - speedCurrent, acceleration*Time.deltaTime);
			}
			sustainPitch = 0.1f + 0.1f * (speedCurrent/topSpeed);
			myBody.AddForce (transform.right * speedCurrent, ForceMode.VelocityChange);
		} else {
			if (sustainPitch > 0.1f) {
				sustainPitch = Mathf.Max (0.1f, sustainPitch - 0.4f * Time.fixedDeltaTime);
			}
			if (speedCurrent > 0 && speedCurrent <= topSpeedMod) {
				speedCurrent -= Mathf.Min (speedCurrent, decceleration * Time.deltaTime);
			}
		}
		//Turn Left
		if (Input.GetKey (KeyCode.A)) {
			myBody.AddTorque (-transform.forward * rotateSpeed,ForceMode.VelocityChange);
			//transform.Rotate (0, 0, Mathf.Sign (speedCurrent) * -topRotate * Time.deltaTime);
		} else if (Input.GetAxis ("LS_X") < -0.4f) {
			myBody.AddTorque (transform.forward*rotateSpeed*Input.GetAxis("LS_X"),ForceMode.VelocityChange);
			//transform.Rotate (0, 0, Input.GetAxis ("LS_X") * Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		}
		//Turn Right
		if (Input.GetKey (KeyCode.D)) {
			myBody.AddTorque (transform.forward * rotateSpeed,ForceMode.VelocityChange);
			//transform.Rotate (0, 0, Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		} else if (Input.GetAxis ("LS_X") > 0.4f) {
			myBody.AddTorque (transform.forward*rotateSpeed*Input.GetAxis("LS_X"),ForceMode.VelocityChange);
			//transform.Rotate (0, 0, Input.GetAxis ("LS_X") * Mathf.Sign (speedCurrent) * topRotate * Time.deltaTime);
		}
		//Vector3 forwardMovement = new Vector3 (transform.right.x, 0, transform.right.z);
		//myBody.velocity = -100 * forwardMovement * Time.deltaTime * speedCurrent;
	}

	//determine ground touch (there is a seperate trigger volume below the car that helps with this)
	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Ground") {
			grounded = true;
		}
	}

	void OnTriggerExit(Collider other) {
		grounded = false;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.relativeVelocity.magnitude > collisionForce) {
			AudioSource.PlayClipAtPoint (collisionSound.clip, collision.contacts [0].point);
			speedCurrent *= 0.6f; //sharp speed reduction upon hitting objects
			//if colliding with another car, show the associated tweet
		}
		if (collision.gameObject.tag == "AICar") {
			CarAIPathing otherCar = collision.gameObject.GetComponent<CarAIPathing> ();
			//ensure other car has this script, otherwise ignore
			if (otherCar != null) {
				UpdateCorner (otherCar, otherCar.thisTrend);
			}
		}
	}

	void UpdateCorner(CarAIPathing carPathing, TwitterTrend newTrend) {
		//out with the old and in with the new
		print ("THAT'S ME IN THE CORNER");
		if (currentTrend == null || (currentTrend.trendStr != newTrend.trendStr || carPathing.tweetNum != tweetNum)) {
			currentTrend = newTrend;
			tweetNum = carPathing.tweetNum;
			cornerTweet.usernameText.text = currentTrend.trendUsers [tweetNum];
			cornerTweet.handleText.text = currentTrend.trendHandles [tweetNum];
			cornerTweet.messageText.text = currentTrend.trendBodies [tweetNum];
			cornerTweet.numLikes.text = currentTrend.trendLikes [tweetNum].ToString();
			cornerTweet.numRetweets.text = currentTrend.trendRetweets [tweetNum].ToString ();
		}
	}
}
