using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIPath : MonoBehaviour {
	public Color raycolor = Color.white;
	public List<Transform> path_objects = new List<Transform>();
	Transform[] theArray;

	void OnDrawGizmos() {
		Gizmos.color = raycolor;
		theArray = GetComponentsInChildren<Transform> ();
		path_objects.Clear ();

		foreach (Transform path_object in theArray) {
			if (path_object != this.transform) {
				path_objects.Add (path_object);
			}
		}

		for (int i = 0; i < path_objects.Count; i++) {
			Vector3 position = path_objects [i].position;
			if (i > 0) {
				Vector3 previous = path_objects [i - 1].position;
				Gizmos.DrawLine (previous, position);
				if (i == path_objects.Count - 1) {
					Gizmos.DrawLine(position,path_objects[0].position);
				}
				Gizmos.DrawWireSphere (position, 1);
			} else {
				Gizmos.DrawWireSphere (position, 3); //path start
			}
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
