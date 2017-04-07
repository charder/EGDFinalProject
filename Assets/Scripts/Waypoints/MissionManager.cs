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

	public GameObject[] destinations = new GameObject[1];
	public float[] missionTimes = new float[1];

	private int currentMission = -1;

	public Text timer;
	public Text remainingTime;
	public GameObject timerObject;
	public GameObject missionSuccessObject;
	public GameObject missionFailedObject;

	private float turnOffOverlayTime = 0f;
	private bool overlayOn = false;

	// Use this for initialization
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
		/**
		navArrow.transform.localPosition = new Vector3 (.35f, 0, 2.6f);
		arrowAgent.destination = endGoal.transform.position;
		NavMeshPath path = new NavMeshPath ();
		arrowAgent.CalculatePath (endGoal.transform.position, path);
		if (path.status == NavMeshPathStatus.PathPartial) {
			arrowAgent.path = path;
		}
		arrowAgent.Resume ();
		**/
		Quaternion targetRotation = Quaternion.LookRotation (endGoal.transform.position - navArrow.transform.position);
		float str = 4;
		navArrow.transform.rotation = Quaternion.Lerp (navArrow.transform.rotation, targetRotation, str);

		if (doingMission) {
			timerTimeRemaining -= Time.deltaTime;
			timer.text = timerTimeRemaining.ToString ("F2");
			if (timerTimeRemaining < 0) {
				print ("YOU SUCK");
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

	public void StartMission(){
		timerObject.SetActive (true);
		currentMission++;
		endGoal = destinations [currentMission].GetComponent<DestinationManager>().goal.gameObject;
		doingMission = true;
		navArrow.SetActive (true);
		destinations [currentMission].GetComponent<DestinationManager> ().Set (1);
		timerStartTime = Time.time;
		timerTimeRemaining = missionTimes [currentMission];
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
	}
}
