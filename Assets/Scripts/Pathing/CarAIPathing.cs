using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CarAIPathing : MonoBehaviour {
	NavMeshAgent agent;
	public GameObject pathObject; //object that holds a path this car will follow
	public TextMesh hashtag; //this car's hashtag/trending topic
	public GameObject playerCam; //find the camera
	List<Transform> pathPoints; //transforms of a path defined by pathObject
	public int currentPoint; //current point # in its path
	// Use this for initialization

	void Awake () {
		hashtag = GetComponentInChildren<TextMesh> ();
		playerCam = GameObject.Find ("CameraTransform");
	}

	void Start () {
		pathPoints = pathObject.GetComponent<CarAIPath> ().path_objects;
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (pathPoints [currentPoint].position);
		//agent.updatePosition = false;
		//agent.updateRotation = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < 1f) {
			if (currentPoint + 1 == pathPoints.Count) {
				currentPoint = 0;
			} else {
				currentPoint++;
			}
			agent.SetDestination (pathPoints [currentPoint].position);
		}
		agent.nextPosition = transform.position;
		hashtag.gameObject.transform.LookAt (Camera.main.transform,transform.up);
		Vector3 hRot = hashtag.transform.rotation.eulerAngles;
		hashtag.transform.rotation = Quaternion.Euler (hRot.x, 0, hRot.z);
		print (playerCam.transform.position);
	}
}
