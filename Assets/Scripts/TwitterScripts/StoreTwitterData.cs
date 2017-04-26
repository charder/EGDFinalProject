using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class StoreTwitterData : MonoBehaviour {
	bool created = false; //whether or not this was the scene this object was created in
	GameObject twitterHandler = null;
	static Twitter.RequestTokenResponse m_RequestTokenResponse;
	static Twitter.AccessTokenResponse m_AccessTokenResponse;

	public Dictionary<string, int> trends;
	public TwitterTrend[] twitterTrends; //all information about trending topics

	public TwitterTweetPlus[] twitterTimeline;


	// You need to save access token and secret for later use.
	// You can keep using them whenever you need to access the user's Twitter account. 
	// They will be always valid until the user revokes the access to your application.
	const string PLAYER_PREFS_TWITTER_USER_ID           = "TwitterUserID";
	const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME  = "TwitterUserScreenName";
	const string PLAYER_PREFS_TWITTER_USER_TOKEN        = "TwitterUserToken";
	const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

	private const string CONSUMER_KEY = "bxF6XgC7nXVhEnGJPlLUkoA8c";
	private const string CONSUMER_SECRET = "jfM8RmslKt1Iq7MxZf7xuJJiuoz3LxkLltN6uPs77aUOWCYNnH";

	private static readonly string RequestTokenURL = "https://api.twitter.com/oauth/request_token";
	private static readonly string AuthorizationURL = "https://api.twitter.com/oauth/authenticate?oauth_token={0}";
	private static readonly string AccessTokenURL = "https://api.twitter.com/oauth/access_token";

	public delegate void TinyURLCallback(bool success, string result, string longurl);

	// Use this for initialization
	void Awake () {
		print (FindObjectsOfType<StoreTwitterData> ().Length);

		if (!created && FindObjectsOfType<StoreTwitterData>().Length == 1) {
			DontDestroyOnLoad (transform.gameObject);
			LoadTwitterUserInfo ();
			twitterHandler = this.gameObject;
			StartGetTrends ();
			StartGetTimeline ();
			created = true;
		} else if (!created) {
			Destroy (this.gameObject);
		}

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			twitterTimeline [0].PrintShit ();
		}
		//        if (Input.GetKeyDown (KeyCode.Escape)) {
		//            ClearUserInfo ();
		//        }

		//		if (Input.GetKeyDown (KeyCode.Tab)) {
		//			Debug.Log ("HERE WE GO");
		//			string url = "https://pbs.twimg.com/profile_images/848712566834593793/uRee6rwf.jpg";
		//			StartCoroutine (GetImage (url));
		//		}
		//
		//        if (Input.GetKeyDown (KeyCode.Tab)) {
		//            Debug.Log("Starting a tweet...");
		//            StartPostTweet ("Test tweet posted at " + System.DateTime.Now.TimeOfDay);
		//        }
		//
		//        if (Input.GetKeyDown (KeyCode.LeftShift)) {
		//            StartPostTweet ("herpaderp");
		//        }
		//        StartGetTimeline();
		//        StartPostRetweet ("854142358216204290");
		//        StartPostReply ("@The_Alpha_Kong IT'S YA BOY BOBBY HERE #FUCKEM", "854153398563872769");
		//        StartPostTweet ("Fake News");
		//        StartPostFavorite ("854768245181689858");
		//        StartGetHashtag ("FakeNews", 20);
	}

	void OnRequestTokenCallback(bool success, Twitter.RequestTokenResponse response)
	{
		if (success)
		{
			string log = "OnRequestTokenCallback - succeeded";
			log += "\n    Token : " + response.Token;
			log += "\n    TokenSecret : " + response.TokenSecret;
			print(log);

			m_RequestTokenResponse = response;

		}
		else
		{
			print("OnRequestTokenCallback - failed.");
		}
	}

	void OnAccessTokenCallback(bool success, Twitter.AccessTokenResponse response)
	{
		if (success)
		{
			string log = "OnAccessTokenCallback - succeeded";
			log += "\n    UserId : " + response.UserId;
			log += "\n    ScreenName : " + response.ScreenName;
			log += "\n    Token : " + response.Token;
			log += "\n    TokenSecret : " + response.TokenSecret;
			print(log);

			m_AccessTokenResponse = response;

			PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_ID, response.UserId);
			PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME, response.ScreenName);
			PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN, response.Token);
			PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET, response.TokenSecret);
		}
		else
		{
			print("OnAccessTokenCallback - failed.");
		}
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////
	public void StartGetTrends()
	{
		StartCoroutine(Twitter.API.GetTrends(CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
			new Twitter.GetTrendsCallback(this.OnGetTrends)));
	}

	void OnGetTrends(bool success, string results)
	{
		print("OnGetTrends - " + (success ? "SUCCESS" : "FAIL"));

		Debug.Log("GetTimeline - " + results);

		var tweets = JSON.Parse(results);
		trends = new Dictionary<string, int> ();

		//Debug.Log("# of Trends: " + tweets[0]["trends"].Count );

		for (int i=0; i< tweets[0]["trends"].Count; i++)
		{
			//Debug.Log("Trend #" + i + ": " + tweets[0]["trends"][i]["name"] + ", Volume: " + tweets[0]["trends"][i]["tweet_volume"]);
			trends.Add (tweets [0] ["trends"] [i] ["name"], tweets [0] ["trends"] [i] ["tweet_volume"].AsInt );
		}

		// trends is a dictionary of string int pairs, where the string is the trend and the int is thge volume
		// WARNING volume may be null
		// Do stuff with it here.

		//^^^^^^^^^^^^ Oh I will...


		CarAIPathing[] cars =  GetComponentsInChildren<CarAIPathing>();
		List<string> trendKeys = new List<string> ();
		foreach (var key in trends.Keys) {
			trendKeys.Add (key);
		}
		for (int i = 0; i < cars.Length; i++) {
			cars [i].hashtag.text = trendKeys [i];
		}
		List<int> trendValues = new List<int> ();
		foreach (var value in trends.Values) {
			trendValues.Add (value);
		}
		twitterTrends = new TwitterTrend [3];
		for (int i = 0; i < 3; i++) { //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ONLY DOING 1 TREND ATM
			twitterTrends[i] = new TwitterTrend();
			twitterTrends[i].trendStr = trendKeys [i];
			twitterTrends[i].trendVolume = trendValues [i];
			StartGetHashtag (trendKeys [i], 50);
		}

	}

	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartGetHashtag(string text, int num)
	{
		if( string.IsNullOrEmpty(text) )
		{
			Debug.LogWarning("StartGetHashtag: no hashtag.");
			return;
		}

		if (num > 0) {
			StartCoroutine (Twitter.API.GetHashtag (text, num, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
				new Twitter.GetTimelineCallback (this.OnGetHashtag)));
		}
	}

	void OnGetHashtag(bool success, string results)
	{
		print("OnGetHashtag - " + (success ? "SUCCESS" : "FAIL"));

		Debug.Log("GetTimeline - " + results);


		var tweets = JSON.Parse(results);

		string trendStr = tweets ["search_metadata"] ["query"].ToString ().Replace ("%23", "#");
		print ("Debug Trend String: " + trendStr);

		TwitterTrend thisTrend = null;
		for (int i = 0; i < twitterTrends.Length; i++) {
			if ("\"" + twitterTrends [i].trendStr + "\"" == trendStr) {
				print ("CAN YOU HEAR ME!");
				thisTrend = twitterTrends [i];
				break;
			}
		}
		//Debug.Log("# of Tweets: " + tweets["statuses"].Count);
		for (int i=0; i< tweets["statuses"].Count; i++)
		{
			thisTrend.trendUsers [i] = (string)tweets ["statuses"] [i] ["user"] ["name"];
			thisTrend.trendHandles [i] = (string)tweets ["statuses"] [i] ["user"] ["screen_name"];
			thisTrend.trendPictureURL [i] = (string)tweets ["statuses"] [i] ["user"] ["profile_image_url"];
			thisTrend.trendBodies[i] = (string)tweets["statuses"][i]["text"];
			if (tweets ["statuses"] [i] ["retweet_count"].AsInt != null) {
				thisTrend.trendRetweets [i] = tweets ["statuses"] [i] ["retweet_count"].AsInt;
			} else {
				thisTrend.trendRetweets [i] = 0;
			}
			if (tweets ["statuses"] [i] ["favorite_count"].AsInt != null) {
				thisTrend.trendLikes [i] = tweets ["statuses"] [i] ["favorite_count"].AsInt;
			} else {
				thisTrend.trendLikes [i] = 0;
			}
		}

		/*
		public string[] trendUsers;
		public string[] trendHandles; 
		public string[] trendPictureURL;
		public string[] trendBodies;
		public int[] trendLikes;
		public int[] trendRetweets;
		*/

		// Stauses is a list of tweets, do stuff with it here

	}

	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartPostTweet(string text) {
		StartCoroutine(Twitter.API.PostTweet(text, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
			new Twitter.PostTweetCallback(this.OnPostTweet)));
	}


	void OnPostTweet(bool success)
	{
		print("OnPostTweet - " + (success ? "succedded." : "failed."));
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartPostReply(string text, string replyID) {
		StartCoroutine(Twitter.API.PostTweet(text, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
			new Twitter.PostTweetCallback(this.OnPostTweet), replyID));
	}

	void OnPostReply(bool success) {
		print("OnPostTweet - " + (success ? "succedded." : "failed."));
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartGetTimeline() {
		StartCoroutine (Twitter.API.GetTimeline(26, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.GetTimelineCallback(this.OnGetTimeline)));
	}

	void OnGetTimeline(bool success, string results)
	{
		print("OnGetTimeline - " + (success ? "succedded." : "failed."));
		var tweets = JSON.Parse(results);
		twitterTimeline = new TwitterTweetPlus[26];

		print ("How many tweets we working with here? " + tweets.Count);
		for (int i = 0; i < tweets.Count; i++) {
			twitterTimeline [i] = new TwitterTweetPlus ();
			twitterTimeline [i].tweetUser = tweets [i] ["user"]["name"];
			twitterTimeline [i].tweetHandle = tweets [i] ["user"] ["screen_name"];
			twitterTimeline [i].tweetBody = tweets [i] ["text"];
			twitterTimeline [i].tweetPictureURL = tweets [i] ["user"] ["profile_image_url"];
			if (tweets [i] ["retweet_count"].AsInt != null) {
				twitterTimeline[i].tweetRetweets = tweets [i] ["retweet_count"].AsInt;
			} else {
				twitterTimeline[i].tweetRetweets = 0;
			}
			if (tweets [i] ["favorite_count"].AsInt != null) {
				twitterTimeline[i].tweetLikes = tweets [i] ["favorite_count"].AsInt;
			} else {
				twitterTimeline [i].tweetLikes = 0;
			}
			//ADD IMAGE FOR TWEET IF APPLICABLE

			//twitterTimeline[i].tweetImage = 

			// <<<<<<<<<<<<<<<<<<<<
		}
		ShowcaseCamera showcaseCameraRef = FindObjectOfType<ShowcaseCamera> ();
		if (showcaseCameraRef != null) {
			showcaseCameraRef.SetTwitterTimelineData (this);
		}

		//        for (int i = 0; i < tweets.Count; i++) {
		//            Debug.Log (tweets [i]["text"]);
		//        }

		//        Debug.Log(tweets[0]["text"]);
		//        var user = tweets [0] ["user"];
		//        Debug.Log (user["profile_image_url"]);

		//string tweets[i]["text"]
		//int tweets[i]["id"]
		//string tweets[i]["in_reply_to_screen_name"] //nullable, will always exist but may be null
		//string tweets[i]["in_reply_to_status_id"] //nullable, will always exist but may be null
		//int tweets[i]["in_reply_to_user_id"] //nullable, will always exist but may be null
		//int tweets[i]["quoted_status_id"] //Warning! may not exist
		//int tweets[i]["retweeted count"]
		//bool tweets[i]["retweeted"] // If retweeted by THIS account

		//int tweets[i]["favorite_count"]
		//bool tweets[i]["favorited"] // if by THIS account

		// var user = tweets[i]["user"];
		// string name = user["name"]; 
	}
	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartPostRetweet(string id) {
		StartCoroutine (Twitter.API.PostRetweet(id, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.PostRetweetCallback(this.OnPostRetweet)));
	}

	void OnPostRetweet(bool success) {
		print("OnPostRetweet - " + (success ? "succedded." : "failed."));
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartPostFavorite(string id) {
		StartCoroutine (Twitter.API.PostFavorite(id, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.PostFavoriteCallback(this.OnPostFavorite)));
	}

	void OnPostFavorite(bool success) {
		print("OnPostFavorite - " + (success ? "succedded." : "failed. xoxo"));
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////


	// Loading Stuff
	void LoadTwitterUserInfo()
	{
		m_AccessTokenResponse = new Twitter.AccessTokenResponse();

		m_AccessTokenResponse.UserId        = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_ID);
		m_AccessTokenResponse.ScreenName    = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
		m_AccessTokenResponse.Token         = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN);
		m_AccessTokenResponse.TokenSecret   = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET);

		if (!string.IsNullOrEmpty(m_AccessTokenResponse.Token) &&
			!string.IsNullOrEmpty(m_AccessTokenResponse.ScreenName) &&
			!string.IsNullOrEmpty(m_AccessTokenResponse.Token) &&
			!string.IsNullOrEmpty(m_AccessTokenResponse.TokenSecret))
		{
			string log = "LoadTwitterUserInfo - succeeded";
			log += "\n    UserId : " + m_AccessTokenResponse.UserId;
			log += "\n    ScreenName : " + m_AccessTokenResponse.ScreenName;
			log += "\n    Token : " + m_AccessTokenResponse.Token;
			log += "\n    TokenSecret : " + m_AccessTokenResponse.TokenSecret;
			print(log);
		}
	}

	public void ClearUserInfo()
	{
		PlayerPrefs.DeleteKey (PLAYER_PREFS_TWITTER_USER_ID);
		PlayerPrefs.DeleteKey (PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
		PlayerPrefs.DeleteKey (PLAYER_PREFS_TWITTER_USER_TOKEN);
		PlayerPrefs.DeleteKey (PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET);

		m_AccessTokenResponse = new Twitter.AccessTokenResponse ();
		m_RequestTokenResponse = null;

		Debug.Log ("User info cleared");
	}

	private const string TinyURLCreateURL = "tinyurl.com/api-create.php?url=";

	public static IEnumerator GetTinyUrl(string longurl, TinyURLCallback callback) {
		WWW web = new WWW( TinyURLCreateURL + longurl, null, null);
		yield return web;

		if (!string.IsNullOrEmpty (web.error)) {
			Debug.Log (string.Format ("TinyURL - failed. {0}\n{1}", web.error, web.text));
			callback (false, web.text, longurl);
		}
		else {
			callback (true, web.text, longurl);
		}
	}

}
	/*
	TwitterHandler twitterRef;
	//various data structures to grab from the twitter handler
	public List<string> trendKeys = new List<string> ();
	public List<int> trendValues = new List<int>();
	TwitterTrend[] twitterTrends = new TwitterTrend[50];

	// Use this for initialization
	void Start () {
		twitterRef = FindObjectOfType<TwitterHandler> ();		
		trendKeys = twitterRef.trendKeys;
		//trendValues = twitterRef.trendValues;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}*/


