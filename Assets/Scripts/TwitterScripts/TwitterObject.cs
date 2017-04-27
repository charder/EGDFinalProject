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
	public RawImage messageImage;

	public string profImageURL;


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

	public void InitializeWP (string username, string handle, string message, Texture2D profileImage, int retweets, int likes, Texture2D messImage){
		usernameText.text = username;
		handleText.text = handle;
		messageText.text = message;
		profImage.texture = profileImage;
		numRetweets.text = retweets.ToString ();
		numLikes.text = likes.ToString ();
		messageImage.texture = messImage;
	}

	void Test(){
		string user = "Caleb Harder";
		string hand = "@theHardestCaleb";
		string message = "May I have your attention, please? May I have your attention, please? Will the real Slim Shady please stand up?";
		int likes = 422;
		int retweets = 78;
		Initialize (user, hand, message, tempPic, retweets, likes);
	}

	void TestWP(){
		string user = "Caleb Harder";
		string hand = "@theHardestCaleb";
		string message = "May I have your attention, please? May I have your attention, please? Will the real Slim Shady please stand up?";
		int likes = 422;
		int retweets = 78;
		InitializeWP (user, hand, message, tempPic, retweets, likes, tempPic);
	}

	public void LoadProfileImage() {
		StartCoroutine (GetImage (profImageURL));
	}

	IEnumerator GetImage(string url)
	{
		Texture2D tex;
		WWW www = new WWW(url);

		//Wait for the Download
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
			Debug.Log("WWW Error: " + www.error);
		else
		{
			profImage.texture = www.texture;
			Debug.Log ("Great Success!");
		}
	}
}
