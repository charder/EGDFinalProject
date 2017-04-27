using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class TwitterHandler: MonoBehaviour {

    public GameObject displayText = null;
    public Text urlText = null;
    public GameObject pinInput = null;
    public GameObject pinText = null;
    public GameObject EnterPINButton = null;

    public GameObject tweetTest = null;
    public MainMenuCameraScript MainCamera;

    public bool bIsReadyToStart = false;

    GameObject twitterHandler = null;
    static Twitter.RequestTokenResponse m_RequestTokenResponse;
    static Twitter.AccessTokenResponse m_AccessTokenResponse;
	public Dictionary<string, int> trends;
	public List<string> trendKeys = new List<string> ();

    // You need to save access token and secret for later use.
    // You can keep using them whenever you need to access the user's Twitter account. 
    // They will be always valid until the user revokes the access to your application.
    public const string PLAYER_PREFS_TWITTER_USER_ID           = "TwitterUserID";
    public const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME  = "TwitterUserScreenName";
    public const string PLAYER_PREFS_TWITTER_USER_TOKEN        = "TwitterUserToken";
    public const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

    private const string CONSUMER_KEY = "bxF6XgC7nXVhEnGJPlLUkoA8c";
    private const string CONSUMER_SECRET = "jfM8RmslKt1Iq7MxZf7xuJJiuoz3LxkLltN6uPs77aUOWCYNnH";

    private static readonly string RequestTokenURL = "https://api.twitter.com/oauth/request_token";
    private static readonly string AuthorizationURL = "https://api.twitter.com/oauth/authenticate?oauth_token={0}";
    private static readonly string AccessTokenURL = "https://api.twitter.com/oauth/access_token";

    public delegate void TinyURLCallback(bool success, string result, string longurl);

	// Use this for initialization
	void Start () {
        LoadTwitterUserInfo ();
        twitterHandler = this.gameObject;
		StartGetTrends ();

        urlText.GetComponent<Text> ().text = "";
        pinInput.SetActive (false);
        EnterPINButton.SetActive(false);

        if (!(displayText == null || urlText == null || pinInput == null || pinText == null || EnterPINButton == null)) {
            handleLogin ();
        }
	}
	
	// Update is called once per frame
	void Update () {
//        if (Input.GetKeyDown (KeyCode.Escape)) {
//            ClearUserInfo ();
//        }

        /*if (Input.GetKeyDown (KeyCode.Tab)) {
            Debug.Log ("HERE WE GO");
            string url = "https://pbs.twimg.com/profile_images/848712566834593793/uRee6rwf.jpg";
            StartCoroutine (GetImage (url));
        }*/
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

    //This is for testing plz no bully
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
            tweetTest.GetComponent<RawImage> ().texture = www.texture;
            Debug.Log ("Great Success!");
        }
    }

    void handleLogin() {
        urlText.GetComponent<Text> ().text = "";
        pinInput.SetActive (false);

        if (string.IsNullOrEmpty (CONSUMER_KEY) || string.IsNullOrEmpty (CONSUMER_SECRET)) {
            // Game had no associated twitter app
            displayText.GetComponent<Text>().text = "This apps permissions are not set, contact a developer.";
        }
        else if (string.IsNullOrEmpty (m_AccessTokenResponse.ScreenName)) {
            // Not logged in, need to log in

            displayText.GetComponent<Text>().text = "No user is logged in. Please click the link below or open it on your mobile device.";
            displayText.GetComponent<Text>().text = "Please enter PIN below:";
            StartCoroutine(Twitter.API.GetRequestToken(CONSUMER_KEY, CONSUMER_SECRET,
                new Twitter.RequestTokenCallback(this.OnRequestTokenCallback)));
        }
        else {
            // have app stuff and we are theoretically logged in, check
            displayText.GetComponent<Text>().text = "verifying...";
            EnterPINButton.SetActive(false);
            StartGetVerify();
        }
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

            StartCoroutine(GetTinyUrl(string.Format (AuthorizationURL, response.Token), this.OnTinyURLCallback));

        }
        else
        {
            print("OnRequestTokenCallback - failed.");
            urlText.GetComponent<Text> ().text = "";
            pinInput.SetActive (false);
            EnterPINButton.SetActive(false);
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

            MainCamera.AccountInputField.text = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
        }
        else
        {
            print("OnAccessTokenCallback - failed.");
        }

        handleLogin ();
    }
        
    public void EnterPIN() {
        StartCoroutine(Twitter.API.GetAccessToken(CONSUMER_KEY, CONSUMER_SECRET, m_RequestTokenResponse.Token, pinText.GetComponent<Text>().text,
            new Twitter.AccessTokenCallback(this.OnAccessTokenCallback)));
    }

    void StartGetVerify() {
        StartCoroutine(Twitter.API.GetVerify(CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
            new Twitter.GetVerifyCallback(this.OnGetVerify)));
    }

    void OnGetVerify(bool success, string results) {
        print("OnGetVerify - " + (success ? "succeeded." : "failed."));

        if (success) {
            var r = JSON.Parse (results);
            displayText.GetComponent<Text>().text = "@" + r ["screen_name"] + " is signed in!";
            bIsReadyToStart = true;
            EnterPINButton.SetActive(false);
            MainCamera.RequestText.text = "Sign Out";
            MainCamera.LogOutButtonObject.SetActive(true);
            MainCamera.LoginText.text = "Switch";
        }
        else {
            displayText.GetComponent<Text>().text = "User could not be verified, please sign out.";
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
        List<string> statuses = new List<string> ();

        //Debug.Log("# of Tweets: " + tweets["statuses"].Count);
        for (int i=0; i< tweets["statuses"].Count; i++)
        {
            //Debug.Log("Tweet #" + i + tweets["statuses"][i]["text"]);
            statuses.Add (tweets ["statuses"] [i] ["text"]);
        }

        // Statuses is a list of tweets, do stuff with it here

    }

    /// //////////////////////////////////////////////////////////////////////////////////////////

    public void StartPostTweet(string text) {
        StartCoroutine(Twitter.API.PostTweet(text, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
            new Twitter.PostTweetCallback(this.OnPostTweet)));
    }


    void OnPostTweet(bool success)
    {
        print("OnPostTweet - " + (success ? "succeeded." : "failed."));
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////

    public void StartPostReply(string text, string replyID) {
        StartCoroutine(Twitter.API.PostTweet(text, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse,
            new Twitter.PostTweetCallback(this.OnPostTweet), replyID));
    }

    void OnPostReply(bool success) {
        print("OnPostTweet - " + (success ? "succeeded." : "failed."));
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////

    public void StartGetTimeline() {
        StartCoroutine (Twitter.API.GetTimeline(12, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.GetTimelineCallback(this.OnGetTimeline)));
    }

    void OnGetTimeline(bool success, string results)
    {
        print("OnGetTimeline - " + (success ? "succeeded." : "failed."));
        var tweets = JSON.Parse(results);

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
		print("OnPostRetweet - " + (success ? "succeeded." : "failed."));
	}

	/// //////////////////////////////////////////////////////////////////////////////////////////

	public void StartPostFavorite(string id) {
		StartCoroutine (Twitter.API.PostFavorite(id, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, new Twitter.PostFavoriteCallback(this.OnPostFavorite)));
	}

	void OnPostFavorite(bool success) {
		print("OnPostFavorite - " + (success ? "succeeded." : "failed. xoxo"));
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
        bIsReadyToStart = false;

        MainCamera.LoginText.text = "Sign In";

        handleLogin ();
    }

    private const string TinyURLCreateURL = "tinyurl.com/api-create.php?url=";

    public static IEnumerator GetTinyUrl(string longurl, TinyURLCallback callback) {
		Dictionary<string, string> headers = new Dictionary<string, string>();
		WWW web = new WWW( TinyURLCreateURL + longurl, null, headers);
        yield return web;

        if (!string.IsNullOrEmpty (web.error)) {
            Debug.Log (string.Format ("TinyURL - failed. {0}\n{1}", web.error, web.text));
            callback (false, web.text, longurl);
        }
        else {
            callback (true, web.text, longurl);
        }
    }

    void OnTinyURLCallback(bool success, string result, string longurl) {
        if (success) {
            print("TinyURL - " + (success ? "succeeded." : "failed."));
            Debug.Log (longurl + " condensed to " + result);
            urlText.GetComponent<Text> ().text = result;
            pinInput.SetActive (true);
            EnterPINButton.SetActive(true);

            if (MainCamera.GetPinned() && MainCamera.RequestText.text.Length > 8 && MainCamera.GetPinned())
            {
                //Application.OpenURL(urlText.text);
            }
        }
        else {
            urlText.GetComponent<Text> ().text = "";
            pinInput.SetActive (false);
            EnterPINButton.SetActive(false);
        }
    }

    public void OpenURL() {
        Application.OpenURL (urlText.text);
    }
}
