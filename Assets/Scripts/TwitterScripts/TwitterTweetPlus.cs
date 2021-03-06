﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterTweetPlus : MonoBehaviour{
	/// <summary>
	/// Class used as data storage for timeline
	/// </summary>

	//the following arrays store data for the important aspects of a tweet to be displayed
	public string tweetUser = null;
	public string tweetHandle = null; 
	public string tweetPictureURL = null;
	public string tweetBody = null;
	public int tweetLikes = 0;
	public int tweetRetweets = 0;

	//Not always a thing
	public string tweetImage = null;

	public Material carMaterial = null;
	public GameObject carModel = null;
	public int postType; //which type of twitter interaction this is
//	0 = Like
//	1 = Retweet
//	2 = Reply
//	3 = Tweet

	//REPLYING
	public string replyID = null; //id of the user that you can reply to


	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PrintShit() {
		
	}

	public TwitterTweetPlus() {

	}
}
