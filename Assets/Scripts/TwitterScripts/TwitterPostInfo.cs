using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterPostInfo : MonoBehaviour{
	/// <summary>
	/// Class used as data storage for timeline
	/// </summary>

	//the following arrays store data for the important aspects of a tweet to be displayed
	public string tweetBody = null;

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

	public void PostToTwitter() {
		StoreTwitterData twitRef =  FindObjectOfType<StoreTwitterData> ();
		switch (postType) {
		//Tweeting
		case 0:
			twitRef.StartPostTweet (tweetBody);
			break;
			//Like
		case 1:
			twitRef.StartPostFavorite (replyID);
			break;
			//Retweet
		case 2:
			twitRef.StartPostRetweet (replyID);
			break;
			//Reply
		case 3:
			twitRef.StartPostReply (tweetBody, replyID);
			break;
		}
	}

	public TwitterPostInfo() {

	}
}
