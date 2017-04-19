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
        TwitterPanel.SetActive(false);
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

        // Press Start text
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

        // Camera Animations
        if (Input.GetKeyDown("t"))
        {
            bool CurrentState = CameraAnimator.GetBool("ShouldZoom");

            if (CurrentState)
            {
                TwitterPanel.SetActive(false);
            }

            CameraAnimator.SetBool("ShouldZoom", !CurrentState);
        }

        if (CameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("MainMenuCameraAnimation") && !CameraAnimator.GetBool("IsZoomed"))
        {
            if (CameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !CameraAnimator.IsInTransition(0))
            {
                CameraAnimator.SetBool("IsZoomed", true);
                TwitterPanel.SetActive(true);
            }
        }

        if (CameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("MainMenuCameraAnimationReverse") && CameraAnimator.GetBool("IsZoomed"))
        {
            if (CameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !CameraAnimator.IsInTransition(0))
            {
                CameraAnimator.SetBool("IsZoomed", false);
                TwitterPanel.SetActive(false);
            }
        }

        if (!CameraAnimator.GetBool("IsZoomed") && !bEnteringNewScene)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UpdatingAlpha = 0.01f;
                FadeOutImage.color = new Color(0.0f, 0.0f, 0.0f, UpdatingAlpha);
                bEnteringNewScene = true;
                bFadeOut = false;
            }
        }
    }

    public Text StartText;
    public Image FadeOutImage;
    public GameObject TwitterPanel;

    private bool bEnteringNewScene;
    private bool bFadeOut;
    private float UpdatingAlpha;

    private Animator CameraAnimator;
}
