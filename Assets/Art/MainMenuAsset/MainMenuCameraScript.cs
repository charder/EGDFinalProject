using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CameraAnimator = GetComponent<Animator>();
        UpdatingAlpha = 0.0f;
        bFadeOut = false;
        TwitterPanel.SetActive(false);
        PINPanel.SetActive(false);
        bIsPinned = false;
        StartText.text = @"Press ""Enter"" to Start";

        SetInputField();
        LoginButton.onClick.AddListener(LoginButtonClicked);
        LogOutButton.onClick.AddListener(LogOutButtonClicked);
        RequestButton.onClick.AddListener(RequestAgainButtonClicked);
    }
	
	// Update is called once per frame
	void Update () {
        if (TwitterHandler.bIsReadyToStart && StartText.text.Length < 25 && !CameraAnimator.GetBool("ShouldZoom"))
        {
            StartText.text = EnterText;
        }

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
            if (UpdatingAlpha >= 0.99)
            {
                // Enter new Scene
                SceneManager.LoadScene("Scenes/ShowcaseScene");
            }
        }

        // Press Start text
        StartText.color = new Color(255.0f, 255.0f, 255.0f, UpdatingAlpha);

        // Camera Animations
        if (Input.GetKeyDown("t") && !CameraAnimator.GetBool("IsZoomed"))
        {
            CameraAnimator.SetBool("ShouldZoom", true);
            StartText.text = EscapeText;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CameraAnimator.SetBool("ShouldZoom", false);
            TwitterPanel.SetActive(false);
            PINPanel.SetActive(false);
            bIsPinned = false;
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
                if (TwitterHandler.bIsReadyToStart)
                {
                    StartText.text = EnterText;
                }
                else
                {
                    StartText.text = @"Press ""Enter"" to Start";
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!TwitterHandler.bIsReadyToStart)
            {
                CameraAnimator.SetBool("ShouldZoom", true);
                StartText.text = EscapeText;
            }
            else if (!CameraAnimator.GetBool("IsZoomed") && !bEnteringNewScene && TwitterHandler.bIsReadyToStart)
            {
                UpdatingAlpha = 0.01f;
                FadeOutImage.color = new Color(0.0f, 0.0f, 0.0f, UpdatingAlpha);
                bEnteringNewScene = true;
                bFadeOut = false;
            }
            if (CameraAnimator.GetBool("IsZoomed") && !bIsPinned)
            {
                LoginButtonClicked();
            }
            else if (CameraAnimator.GetBool("IsZoomed") && bIsPinned)
            {
                TwitterHandler.EnterPIN();
            }
        }
    }

    void SetInputField()
    {
        AccountInputField.text = PlayerPrefs.GetString(TwitterHandler.PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
        if (AccountInputField.text.Length > 1)
        {
            LoginText.text = "Switch";
            LogOutButtonObject.SetActive(true);
        }
        else
        {
            AccountInputField.text = "No User Signed In";
            LoginText.text = "Sign In";
            LogOutButtonObject.SetActive(false);
        }
    }

    void LoginButtonClicked()
    {
        TwitterPanel.SetActive(false);
        PINPanel.SetActive(true);

        bIsPinned = true;

        if (TwitterHandler.bIsReadyToStart)
        {
            RequestAgainButtonClicked();
            bIsPinned = true;
        }

        if (TwitterHandler.urlText.text.Length > 1)
        {
            Application.OpenURL(TwitterHandler.urlText.text);
        }
    }

    void LogOutButtonClicked()
    {
        TwitterHandler.ClearUserInfo();
        LogOutButtonObject.SetActive(false);

        AccountInputField.text = "No User Signed In";

        RequestText.text = "Request Again";
    }

    void RequestAgainButtonClicked()
    {
        bIsPinned = true;
        TwitterHandler.ClearUserInfo();

        LogOutButtonObject.SetActive(false);

        if (!TwitterHandler.bIsReadyToStart)
        {
            AccountInputField.text = "No User Signed In";

            if (RequestText.text.Length < 10)
            {
                bIsPinned = false;
                RequestText.text = "Request Again";
            }
        }
    }

    public bool GetPinned()
    {
        return bIsPinned;
    }

    public Text StartText;
    public Image FadeOutImage;
    public GameObject TwitterPanel;
    public GameObject PINPanel;
    public GameObject LogOutButtonObject;

    public Button LoginButton;
    public Text LoginText;
    public Button LogOutButton;
    public Button RequestButton;
    public Text RequestText;
    public InputField AccountInputField;
    public InputField PINInputField;
    public TwitterHandler TwitterHandler;
    
    private bool bEnteringNewScene;
    private bool bFadeOut;
    private float UpdatingAlpha;
    private bool bIsPinned;

    private static string EnterText = @"Press ""Enter"" to Start 
Press ""T"" to Switch Account";
    private static string EscapeText = @"Press ""Escape"" to back";

    private Animator CameraAnimator;
}
