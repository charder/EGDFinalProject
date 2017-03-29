using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScr : MonoBehaviour {

    public string gameLevelName = "";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame() {
        SceneManager.LoadScene(gameLevelName);
    }
}
