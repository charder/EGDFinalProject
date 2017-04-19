using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

public class TwitterHandler: MonoBehaviour {


    GameObject twitterHandler = null;
    Twitter.AccessTokenResponse m_AccessTokenResponse = new Twitter.AccessTokenResponse();
	public Dictionary<string, int> trends;
	public List<string> trendKeys = new List<string> ();

    // You need to save access token and secret for later use.
    // You can keep using them whenever you need to access the user's Twitter account. 
    // They will be always valid until the user revokes the access to your application.
    const string PLAYER_PREFS_TWITTER_USER_ID           = "TwitterUserID";
    const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME  = "TwitterUserScreenName";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN        = "TwitterUserToken";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

	// Use this for initialization
	void Start () {
        LoadTwitterUserInfo ();
        twitterHandler = this.gameObject;
		StartGetTrends ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
        if (Input.GetKeyDown (KeyCode.Q)) {
            Debug.Log ("Getting tweets...");
            StartGetHashtag ("#FakeNews", 5);
        }

        if (Input.GetKeyDown (KeyCode.W)) {
            Debug.Log ("Getting trends...");
            StartGetTrends ();
        }

        */

        if (Input.GetKeyDown (KeyCode.E)) {
            //Debug.Log ("Posting something to twitter...");
			StartPostFavorite ("854142358216204290");
            //StartGetTimeline();
        }

        if (Input.GetKeyDown (KeyCode.W)) {
            //Debug.Log ("Posting something to twitter...");
            StartGetTimeline();
        }
	}

    /// //////////////////////////////////////////////////////////////////////////////////////////
    public void StartGetTrends()
    {
        Demo d = twitterHandler.GetComponent<Demo> ();
        StartCoroutine(Twitter.API.GetTrends(d.CONSUMER_KEY, d.CONSUMER_SECRET, m_AccessTokenResponse,
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

		//Send the first couple to the preset cars for the demo!!! #HARDCODED
		CarAIPathing[] cars =  GetComponentsInChildren<CarAIPathing>();
		trendKeys = new List<string> ();
		foreach (var key in trends.Keys) {
			trendKeys.Add (key);
		}
		for (int i = 0; i < cars.Length; i++) {
			cars [i].hashtag.text = trendKeys [i];
		}

    }

    /// //////////////////////////////////////////////////////////////////////////////////////////

    public void StartGetHashtag(string text, int num)
    {
        Demo d = twitterHandler.GetComponent<Demo> ();
        if( string.IsNullOrEmpty(text) )
        {
            Debug.LogWarning("StartGetHashtag: no hashtag.");
            return;
        }

        if (num > 0) {
            StartCoroutine (Twitter.API.GetHashtag (text, num, d.CONSUMER_KEY, d.CONSUMER_SECRET, m_AccessTokenResponse,
                new Twitter.GetTimelineCallback (this.OnGetHashtag)));
        }
    }

    void OnGetHashtag(bool success, string results)
    {
        print("OnGetHashtag - " + (success ? "SUCCESS" : "FAIL"));

        Debug.Log("GetTimeline - " + results);


        var tweets = JSON.Parse(results);
        List<string> statuses = new List<string> ();

        //Debug.Log("# of Tweets: " + tweets["statuses"].Count);
        for (int i=0; i< tweets["statuses"].Count; i++)
        {
            //Debug.Log("Tweet #" + i + tweets["statuses"][i]["text"]);
            statuses.Add (tweets ["statuses"] [i] ["text"]);
        }

        // Stauses is a list of tweets, do stuff with it here

    }

    /// //////////////////////////////////////////////////////////////////////////////////////////

    public void StartPostTweet(string text) {
        Demo d = twitterHandler.GetComponent<Demo> ();
        StartCoroutine(Twitter.API.PostTweet(text, d.CONSUMER_KEY, d.CONSUMER_SECRET, m_AccessTokenResponse,
            new Twitter.PostTweetCallback(this.OnPostTweet)));
    }


    void OnPostTweet(bool success)
    {
        print("OnPostTweet - " + (success ? "succedded." : "failed."));
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////

    public void StartGetTimeline() {
        Demo d = twitterHandler.GetComponent<Demo> ();
        StartCoroutine (Twitter.API.GetTimeline(12, d.CONSUMER_KEY, d.CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.GetTimelineCallback(this.OnGetTimeline)));
    }

    void OnGetTimeline(bool success, string results)
    {
        print("OnGetTimeline - " + (success ? "succedded." : "failed."));
        var tweets = JSON.Parse(results);

//        for (int i = 0; i < tweets.Count; i++) {
//            Debug.Log (tweets [i]["text"]);
//        }

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
		Demo d = twitterHandler.GetComponent<Demo> ();
		StartCoroutine (Twitter.API.PostRetweet(id, d.CONSUMER_KEY, d.CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.PostRetweetCallback(this.OnPostRetweet)));
	}

	void OnPostRetweet(bool success) {
		print("OnPostRetweet - " + (success ? "succedded." : "failed."));
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartPostFavorite(string id) {
		Demo d = twitterHandler.GetComponent<Demo> ();
		StartCoroutine (Twitter.API.PostFavorite(id, d.CONSUMER_KEY, d.CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.PostFavoriteCallback(this.OnPostFavorite)));
	}

	void OnPostFavorite(bool success) {
		print("OnPostFavorite - " + (success ? "succedded." : "failed."));
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

}
