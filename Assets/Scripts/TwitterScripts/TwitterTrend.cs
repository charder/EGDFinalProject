using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterTrend : MonoBehaviour {
	/// <summary>
	/// Class used as data storage for trending topics
	/// </summary>

	public string trendStr; //trend string (text above car)
	public int trendVolume; //volume of trend (used for spawn frequency)

	//the following arrays store data for the important aspects of a tweet to be displayed
	public string[] trendUsers;
	public string[] trendHandles; 
	public string[] trendPictureURL;
	public string[] trendBodies;
	public int[] trendLikes;
	public int[] trendRetweets;
    public Texture2D[] trendProfPics;
	public string[] trendIDs;

	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PrintShit() {
		print ("Trend Name: " + trendStr);
		print ("Trend Volume: " + trendVolume);
		for (int i = 0; i < 50; i++) {
			print ("Trend User: " + trendRetweets[i]);
		}
	}

	public TwitterTrend() {
		trendUsers = new string[50];
		trendHandles = new string[50];
		trendPictureURL = new string[50];
		trendBodies = new string[50];
		trendLikes = new int[50];
		trendRetweets = new int[50];
        trendProfPics = new Texture2D[50];
		trendIDs = new string[50];
	}

    public void LoadProfPic(int index) {
        StartCoroutine( GetImage(index));
    }

    IEnumerator GetImage(int index)
    {
        Texture2D tex;
        WWW www = new WWW(trendPictureURL[index]);

        //Wait for the Download
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
            Debug.Log("WWW Error: " + www.error);
        else
        {
            Debug.Log ("GREATEST SUCCESS");
            trendProfPics[index] = www.texture;
        }
    }
}
