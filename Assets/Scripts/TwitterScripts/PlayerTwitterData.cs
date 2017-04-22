using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Info about the player is stored here, including relevant twitter data as well as its visual components
/// </summary>
public class PlayerTwitterData : CarDataPassage {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	//IMPORTANT: The CarData class is defined in the script this script extends: CarDataPassage.cs
	//This Script is called by ShowcaseCamera.cs when the player's car is created
	public void ReceiveData(CarData mydata) {

	}

}
