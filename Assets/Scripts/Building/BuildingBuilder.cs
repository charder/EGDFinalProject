using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBuilder : MonoBehaviour {

	public GameObject bottom;
	public GameObject story;
	public GameObject roof;

	public int numberMiddleFloors = 1;
	public float floorYOffset = 1.8f;

	// Use this for initialization
	void Start () {
		GameObject Bottom = (GameObject)Instantiate (bottom, transform.position, transform.rotation, transform);
		float height = 0;
		for (int i = 0; i < numberMiddleFloors; i++) {
			height += floorYOffset;
			Vector3 pos = new Vector3 (transform.position.x, transform.position.y + height, transform.position.z);
			GameObject Story = (GameObject)Instantiate (story, pos, transform.rotation, transform);
		}
		height += floorYOffset;
		Vector3 roofPos = new Vector3 (transform.position.x, transform.position.y + height, transform.position.z);
		GameObject Roof = (GameObject)Instantiate (roof, roofPos, transform.rotation, transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
