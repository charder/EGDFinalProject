using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class DemoGameObject : MonoBehaviour {


    GameObject twitterHandler = null;
    Twitter.AccessTokenResponse m_AccessTokenResponse = new Twitter.AccessTokenResponse();

	// Use this for initialization
	void Start () {
        twitterHandler = GameObject.Find ("TwitterHandler");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown (KeyCode.Q)) {
            Debug.Log ("Getting tweets...");
            StartGetHashtag ("#FakeNews", 5);
        }

        if (Input.GetKeyDown (KeyCode.W)) {
            Debug.Log ("Getting trends...");
            StartGetTrends ();
        }

        if (Input.GetKeyDown (KeyCode.E)) {
            Debug.Log ("Posting something to twitter...");
            StartPostTweet ("#Test");
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
        Dictionary<string, int> trends = new Dictionary<string, int> ();

        //Debug.Log("# of Trends: " + tweets[0]["trends"].Count );

        for (int i=0; i< tweets[0]["trends"].Count; i++)
        {
            //Debug.Log("Trend #" + i + ": " + tweets[0]["trends"][i]["name"] + ", Volume: " + tweets[0]["trends"][i]["tweet_volume"]);
            trends.Add (tweets [0] ["trends"] [i] ["name"], tweets [0] ["trends"] [i] ["tweet_volume"].AsInt );
        }

        // trends is a dictionary of string int pairs, where the string is the trend and the int is thge volume
        // WARNING volume may be null
        // Do stuff with it here.
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

}
