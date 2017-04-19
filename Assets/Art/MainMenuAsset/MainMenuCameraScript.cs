using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuCameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CameraAnimator = GetComponent<Animator>();
        UpdatingAlpha = 0.0f;
        bFadeOut = false;
    }
	
	// Update is called once per frame
	void Update () {

        // The fade in fade out for "press start"
        if (UpdatingAlpha < 1.0f && !bFadeOut)
        {
            UpdatingAlpha += Time.deltaTime;
        }
        if (UpdatingAlpha > 0.0f && bFadeOut)
        {
            UpdatingAlpha -= Time.deltaTime;
        }
        if (!bEnteringNewScene)
        {
            if (UpdatingAlpha >= 1.0f)
            {
                bFadeOut = true;
            }
            if (UpdatingAlpha <= 0.0f)
            {
                bFadeOut = false;
            }
        }

        if(bEnteringNewScene)
        {
            FadeOutImage.color = new Color(0.0f, 0.0f, 0.0f, UpdatingAlpha);
        }

        // Camera animations
        if(CameraAnimator.GetBool("IsZoomed"))
        {
            StartText.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
        }
        else
        {
            if (CameraAnimator.GetBool("ShouldZoom"))
            {
                StartText.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            }
            else
            {
                StartText.color = new Color(255.0f, 255.0f, 255.0f, UpdatingAlpha);
            }
        }

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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdatingAlpha = 0.01f;
            FadeOutImage.color = new Color(0.0f, 0.0f, 0.0f, UpdatingAlpha);
            bEnteringNewScene = true;
            bFadeOut = false;
        }
    }

    public Text StartText;
    public Image FadeOutImage;

    private bool bEnteringNewScene;
    private bool bFadeOut;
    private float UpdatingAlpha;

    private Animator CameraAnimator;
}
