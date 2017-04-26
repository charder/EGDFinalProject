using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CarAIPathing : MonoBehaviour {
	NavMeshAgent agent;
	public GameObject pathObject; //object that holds a path this car will follow
	public TextMesh hashtag; //this car's hashtag/trending topic
	public Transform playerCam; //find the camera
	List<Transform> pathPoints; //transforms of a path defined by pathObject
	public int currentPoint; //current point # in its path

	float reactionDelay = 2f; //time to wait until movement begins

	public GameObject spawnPointPrefab;

	public TwitterTrend thisTrend;

	void Awake () {
		hashtag = GetComponentInChildren<TextMesh> ();
		hashtag.transform.localScale = new Vector3 (-hashtag.transform.localScale.x, hashtag.transform.localScale.y, hashtag.transform.localScale.z);
		playerCam = Camera.main.transform;
		agent = GetComponent<NavMeshAgent> ();
	}

	void Start () {
		//pathObject = pathObject.GetComponent<carSpawnPoint> ().pathToFollow;
		agent.Warp(transform.position);
		agent.SetDestination (pathPoints [currentPoint].position);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position,pathPoints[currentPoint].position) < 8f) {
			if (currentPoint + 1 == pathPoints.Count) {
				currentPoint = 0;
			} else {
				currentPoint++;
			}
			agent.SetDestination (pathPoints [currentPoint].position);
		}
		//agent.nextPosition = transform.position;
		hashtag.transform.LookAt(playerCam.position);
		Vector3 hRot = hashtag.transform.rotation.eulerAngles;
		//hashtag.transform.rotation = Quaternion.Euler (hRot.x, 0, hRot.z);
		//Press P to capture a paused version of the cars
		if (Input.GetKeyDown(KeyCode.P)) {
			GameObject carPoint = (GameObject)Instantiate (spawnPointPrefab, transform.position, transform.rotation);
			carSpawnPoint carPointComp = carPoint.GetComponent<carSpawnPoint> ();
			carPointComp.startNode = currentPoint;
			carPointComp.pathToFollow = pathObject;
		}
	}

	public void SetPathObject(GameObject obj) {
		pathObject = obj;
		pathPoints = pathObject.GetComponent<CarAIPath> ().path_objects;
		agent.SetDestination (pathPoints [currentPoint].position);
	}
}
