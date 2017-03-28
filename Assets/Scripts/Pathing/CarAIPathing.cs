using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAIPathing : MonoBehaviour {
	NavMeshAgent agent;
	public GameObject pathObject; //object that holds a path this car will follow
	List<Transform> pathPoints; //transforms of a path defined by pathObject
	int currentPoint; //current point # in its path
	// Use this for initialization
	void Start () {
		pathPoints = pathObject.GetComponent<CarAIPath> ().path_objects;
		agent = GetComponent<NavMeshAgent> ();
		//agent.updatePosition = false;
		//agent.updateRotation = false;
		currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < 0.5f) {
			if (currentPoint + 1 == pathPoints.Count) {
				currentPoint = 0;
			} else {
				currentPoint++;
			}
			agent.SetDestination (pathPoints [currentPoint].position);
		}
		agent.nextPosition = transform.position;
	}
}
