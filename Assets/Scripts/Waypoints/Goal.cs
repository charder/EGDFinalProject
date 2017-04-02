using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	float alphaValue = .5f;
	float rate = .3f;

	bool disappearing = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (disappearing && alphaValue <= 0) {
			disappearing = false;
			gameObject.SetActive (false);
		} else if(disappearing){
			alphaValue -= rate * Time.deltaTime;
			foreach (Transform child in transform) {
				Color c = child.gameObject.GetComponent<SpriteRenderer> ().color;
				child.gameObject.GetComponent<SpriteRenderer> ().color = new Color (c.r, c.g, c.b, alphaValue);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			disappearing = true;
		}
	}

	public void Restart(){
		disappearing = false;
		alphaValue = .5f;
		foreach (Transform child in transform) {
			Color c = child.gameObject.GetComponent<SpriteRenderer> ().color;
			child.gameObject.GetComponent<SpriteRenderer> ().color = new Color (c.r, c.g, c.b, alphaValue);
		}
	}
}
