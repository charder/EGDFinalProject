using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CameraAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("t"))
        {
            CameraAnimator.SetBool("ShouldZoom", !CameraAnimator.GetBool("ShouldZoom"));
        }

        if (CameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("MainMenuCameraAnimation"))
        {
            if (CameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !CameraAnimator.IsInTransition(0))
            {
                CameraAnimator.SetBool("IsZoomed", true);
            }
        }

        if (CameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("MainMenuCameraAnimationReverse"))
        {
            if (CameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !CameraAnimator.IsInTransition(0))
            {
                CameraAnimator.SetBool("IsZoomed", false);
            }
        }
    }

    private Animator CameraAnimator;
}
