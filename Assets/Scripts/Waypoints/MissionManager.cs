﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MissionManager : MonoBehaviour {

	public GameObject endGoal;
	public GameObject startLocation;

	public GameObject car;
	public GameObject navArrow;
	private NavMeshAgent arrowAgent;

	public float detectDistance = 10f;

	public bool doingMission = true;


	// Use this for initialization
	void Start () {
		/*
		arrowAgent = navArrow.GetComponent<NavMeshAgent> ();
		arrowAgent.destination = endGoal.transform.position;
		*/
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
	}

}
