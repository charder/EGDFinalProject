using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {
	string activeSceneName;
	bool created = false;
	public AudioSource menuMusic;
	public AudioSource gameMusic;
	bool twitterData = false;
	bool menuMusicPlaying = false; //whether or not the menuMusic is playing currently

	// Use this for initialization
	void Awake () {
		activeSceneName = SceneManager.GetActiveScene ().name;
		//Handle all start instances
		if (!created && FindObjectsOfType<SceneHandler> ().Length > 1) {
			Destroy (this.gameObject);
		} else if (!created) {
			DontDestroyOnLoad (this.gameObject);
			created = true;
		}

		activeSceneName = SceneManager.GetActiveScene ().name;
		newScene ();
	}

	void OnSceneLoad() {
		
	}
	// Update is called once per frame
	void Update () {
		if (activeSceneName != SceneManager.GetActiveScene ().name) {
			activeSceneName = SceneManager.GetActiveScene ().name;
			newScene ();
		}

		//NON-FUNCTIONING CONTROLS <<<<<

		/*
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			if (activeSceneName == "ModifiedCity") {
				SceneManager.LoadScene ("ShowcaseScene");
			}
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			if (activeSceneName != "MainMenu") {
				SceneManager.LoadScene ("MainMenu");
			}
		}
		*/
	}

	void newScene() {
		if (activeSceneName == "ShowcaseScene") {
			GetComponent<StoreTwitterData> ().loadTimeline ();
			//load trends ONCE, while timeline is refreshed every time
			if (!twitterData) {
				GetComponent<StoreTwitterData> ().loadTrendData ();
				twitterData = true;
			}
		}

		if (menuMusicPlaying && activeSceneName == "ModifiedCity") {
			print ("Playing Game");
			menuMusic.Stop ();
			gameMusic.Play ();
			menuMusicPlaying = false;
		} else if (!menuMusicPlaying) {
			menuMusicPlaying = true;
			menuMusic.Play ();
			gameMusic.Stop ();
		}
	}
}
