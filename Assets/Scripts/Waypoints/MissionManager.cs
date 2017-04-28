using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour {
	public GameObject endGoal;
	public GameObject startLocation;

	public GameObject car;
	public GameObject navArrow;
	private NavMeshAgent arrowAgent;

	public float detectDistance = 10f;

	public bool doingMission = true;

	private float timerStartTime = 0f;
	private float timerTimeRemaining = 0f;

	public DestinationManager[] destinations;
	public float[] missionTimes = new float[1];

	private int currentMission = -1;
	private int dIndex = 0;

	public Text timer;
	public Text remainingTime;
	public GameObject timerObject;
	public GameObject missionSuccessObject;
	public GameObject missionFailedObject;

	private float turnOffOverlayTime = 0f;
	private bool overlayOn = false;

	public bool created = false;

	// Use this for initialization
	void Awake() {
		if (!created) {
			created = true;
		}
	}

	void Start () {
		/*
		arrowAgent = navArrow.GetComponent<NavMeshAgent> ();
		arrowAgent.destination = endGoal.transform.position;
		*/
		doingMission = false;
		navArrow.SetActive (false);

		//StartMission ();
	}
	
	// Update is called once per frame
	void Update () {
		if (doingMission && Input.GetKeyDown(KeyCode.P)) {
			timerTimeRemaining += 30f;
		}
		if (created) {
			Quaternion targetRotation = Quaternion.LookRotation (endGoal.transform.position - navArrow.transform.position);
			float str = 4;
			navArrow.transform.rotation = Quaternion.Lerp (navArrow.transform.rotation, targetRotation, str);

			if (doingMission) {
				timerTimeRemaining -= Time.deltaTime;
				timer.text = timerTimeRemaining.ToString ("F2");
				if (timerTimeRemaining < 0) {
					EndMission ();
					FailMission ();
				}
			}
			if (overlayOn && Time.time > turnOffOverlayTime) {
				overlayOn = false;
				missionSuccessObject.SetActive (false);
				missionFailedObject.SetActive (false);
			}
		}
	}

	public void StartMission(int type){
		if (!doingMission) {
			destinations = FindObjectsOfType<DestinationManager> ();
			timerObject.SetActive (true);
			dIndex = Random.Range (0, 4);
			endGoal = destinations [dIndex].goal.gameObject;
			doingMission = true;
			navArrow.SetActive (true);
			destinations [dIndex].Set (type);
			timerStartTime = Time.time;
			timerTimeRemaining = destinations [dIndex].GetComponent<DestinationManager> ().goalTime;
		}
	}

	public void EndMission(){
		if (doingMission) {
			doingMission = false;
			navArrow.SetActive (false);
			timerObject.SetActive (false);
		}
	}

	public void FailMission(){
		missionFailedObject.SetActive (true);
		turnOffOverlayTime = Time.time + 5f;
		overlayOn = true;
	}

	public void WinMission(){
		missionSuccessObject.SetActive (true);
		remainingTime.text = timerTimeRemaining.ToString ("F2");
		turnOffOverlayTime = Time.time + 5f;
		overlayOn = true;
		FindObjectOfType<TwitterPostInfo> ().PostToTwitter ();
	}
}
