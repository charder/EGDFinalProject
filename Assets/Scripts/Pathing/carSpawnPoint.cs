﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawnPoint : MonoBehaviour {
	public GameObject pathToFollow; //path of nodes the car will follow
	public GameObject[] carPrefabs; //which car prefab to spawn
	public Material[] carMaterials;
	public int startNode; //node on the path the spawned car will start on
	TwitterHandler twitHandler; //reference to the twitter handler script
	float delaySpawn = 2f;
	// Use this for initialization
	void Awake () {
		twitHandler = FindObjectOfType<TwitterHandler> ();
		//spawnCar ();
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, 2*Vector3.one);
	}

	// Update is called once per frame
	void Update () {
		if (delaySpawn > 0) {
			delaySpawn -= Time.deltaTime;
			if (delaySpawn <= 0) {
				spawnCar ();
			}
		}
	}

	void spawnCar() {
		int roll = Random.Range (0, carPrefabs.Length);
		int rollC = Random.Range (0, carMaterials.Length);
		GameObject tmp = (GameObject)Instantiate (carPrefabs [roll], transform.position, transform.rotation);
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
		int rollT = Random.Range (0, twitHandler.trendKeys.Count); //roll for trending topic to put on car <<<<<<< TEMPORARY?
		tmp.GetComponentInChildren<TextMesh>().text = twitHandler.trendKeys[rollT];
		carTmp.SetPathObject (pathToFollow);
		carTmp.currentPoint = startNode;
	}
}