using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	public GameObject endNode;
	public GameObject[] pathNodes;

	public GameObject[] waypointNodes;

	public GameObject car;

	public float detectDistance = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateWaypoints(){
		
	}

	void Pathfind(){
		
	}

	void FindClosestNode(){
		GameObject node = waypointNodes [0];
		float distance = Vector3.Distance (car.transform.position, node.transform.position);
		for (int i = 1; i < waypointNodes.Length; i++) {
			float tempDistance = Vector3.Distance (car.transform.position, waypointNodes[i].transform.position);

		}
	}
}
