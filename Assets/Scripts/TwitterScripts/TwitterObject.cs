using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwitterObject : MonoBehaviour {

	public Text usernameText;
	public Text handleText;
	public Text messageText;
	public RawImage profImage;
	public Text numRetweets;
	public Text numLikes;

	public Texture2D tempPic;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Initialize (string username, string handle, string message, Texture2D profileImage, int retweets, int likes){
		usernameText.text = username;
		handleText.text = handle;
		messageText.text = message;
		profImage.texture = profileImage;
		numRetweets.text = retweets.ToString ();
		numLikes.text = likes.ToString ();
	}

	void Test(){
		string user = "Caleb Harder";
		string hand = "@theHardestCaleb";
		string message = "May I have your attention, please? May I have your attention, please? Will the real Slim Shady please stand up?";
		int likes = 422;
		int retweets = 78;
		Initialize (user, hand, message, tempPic, retweets, likes);
	}
}
