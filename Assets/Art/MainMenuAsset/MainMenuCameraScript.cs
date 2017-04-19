using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        timer = 0.0f;
        bWait = false;
        CameraAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("t"))
        {
            bWait = true;

            CameraAnimator.SetBool("ShouldZoom", !CameraAnimator.GetBool("ShouldZoom"));
        }

        if (bWait)
        {
            timer += Time.deltaTime;
        }

        if (timer>0.5f)
        {
            CameraAnimator.SetBool("IsZoomed", !CameraAnimator.GetBool("IsZoomed"));
            bWait = false;
            timer = 0.0f;
        }

	}

    private float timer;
    private bool bWait;
    private Animator CameraAnimator;
}
