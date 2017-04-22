using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : MonoBehaviour {

	public DestinationSign[] signs = new DestinationSign[1];
	public Goal goal;

	public MissionManager mm;

	// Use this for initialization
	void Start () {
		Unset ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//type
	//1 = tweet
	//2 = retweet
	//3 = reply
	//4 = favorite
	public void Set(int type){
		for (int s = 0; s < signs.Length; s++) {
			DestinationSign ds = signs [s];
			ds.ChangeSign (type, true);
			goal.gameObject.SetActive (true);
			goal.Restart ();
		}
	}

	public void Unset(){
		for (int s = 0; s < signs.Length; s++) {
			DestinationSign ds = signs [s];
			ds.ChangeSign (0, false);
			goal.gameObject.SetActive (false);
		}
	}

	public void Victory(){
		mm = FindObjectOfType<MissionManager> ();
		mm.EndMission ();
		mm.WinMission ();
		Unset ();
	}
}
