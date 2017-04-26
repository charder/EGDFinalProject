using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterTweetPlus : MonoBehaviour {
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

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PrintShit() {
		print ("Tweet User: " + tweetUser);
		print ("Tweet Handle" + tweetHandle);
		print ("Tweet Picture URL" + tweetPictureURL);
		print ("Tweet Body" + tweetBody);
		print ("Tweet Likes" + tweetLikes);
		print ("Tweet Retweets" + tweetRetweets);
	}

	public TwitterTweetPlus() {

	}
}
