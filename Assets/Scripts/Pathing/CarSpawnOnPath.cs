using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSpawnOnPath : MonoBehaviour {
	public GameObject pathToFollow; //path of nodes the car will follow
	public GameObject[] carPrefabs; //which car prefab to spawn
	public Material[] carMaterials;
	public int startNode; //node on the path the spawned car will start on
	public float startDelay; //delay on spawning the first car
	public float spawnInterval; //time between car spawns after first spawn
	public float spawnTime; //how long to spawn for before stopping
	float timePassed = 0;

	bool spawnStarted; //whether the script has started spawning
	private float timeTrack; 
	bool stop = false;
	StoreTwitterData twitHandler; //reference to the twitter handler script
	// Use this for initialization
	void Start () {
		twitHandler = FindObjectOfType<StoreTwitterData> ();
		spawnStarted = false;
		startDelay *= -1; //switch to negative value for countdown
		timeTrack = 0;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, 2*Vector3.one);
	}

	// Update is called once per frame
	void Update () {
		if (timePassed < spawnTime) {
			timePassed += Time.deltaTime;
			if (timePassed >= spawnTime) {
				stop = true;
			}
		}
		if (!stop) {
			if (!spawnStarted && timeTrack <= startDelay) {
				spawnStarted = true;
				timeTrack = spawnInterval;
				spawnCar ();
			} else if (spawnStarted && timeTrack <= 0) {
				timeTrack = spawnInterval;
				spawnCar ();
			}
			timeTrack -= Time.deltaTime;
		}
	}

	void spawnCar() {
		int roll = Random.Range (0, carPrefabs.Length);
		int rollC = Random.Range (0, carMaterials.Length);
		GameObject tmp = (GameObject)Instantiate (carPrefabs[roll],transform.position,Quaternion.identity);
		CarAIPathing carTmp = tmp.GetComponent<CarAIPathing> ();
		//tmp.GetComponentInChildren<MeshRenderer> ().materials [0] = carMaterials [rollC];
		MeshRenderer carMesh = tmp.GetComponentInChildren<MeshRenderer>();
		Material[] updatedMat = carMesh.materials;
		if (updatedMat [0].name == "CarBumper (Instance)") {
			updatedMat [1] = carMaterials [rollC];
		} else {
			updatedMat [0] = carMaterials [rollC];
		}

		tmp.GetComponentInChildren<MeshRenderer> ().materials = updatedMat;
		int rollT = Random.Range (0, twitHandler.twitterTrends.Length); //roll for trending topic to put on car <<<<<<< TEMPORARY?
		tmp.GetComponentInChildren<TextMesh>().text = twitHandler.twitterTrends[rollT].trendStr;
		carTmp.SetPathObject (pathToFollow);
	}
}
