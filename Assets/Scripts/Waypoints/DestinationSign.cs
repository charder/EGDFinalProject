using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSign : MonoBehaviour {

	public GameObject WIFI;
	public GameObject tweet;
	public GameObject retweet;
	public GameObject reply;
	public GameObject favorite;

	public bool active;
	public int state = 0;
	//0 = Nothing (WIFI)
	//1 = tweet
	//2 = retweet
	//3 = reply
	//4 = favorite

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeSign(int newState, bool newActive){
		if (state != newState) {
			WIFI.SetActive (false);
			tweet.SetActive (false);
			retweet.SetActive (false);
			reply.SetActive (false);
			favorite.SetActive (false);

			state = newState;
			if (state == 0) {
				WIFI.SetActive (true);
			} else if (state == 1) {
				tweet.SetActive (true);
			} else if (state == 2) {
				retweet.SetActive (true);
			} else if (state == 3) {
				reply.SetActive (true);
			} else {
				favorite.SetActive (true);
			}
		}
	}
}
