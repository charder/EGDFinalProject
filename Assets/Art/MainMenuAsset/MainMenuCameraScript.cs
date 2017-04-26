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

        SetInputField();
        LoginButton.onClick.AddListener(LoginButtonClicked);
        EnterPINButton.onClick.AddListener(TwitterHandler.EnterPIN);
        AccountInputField.onEndEdit.AddListener(AccountEdited);
        PasswordInputField.onEndEdit.AddListener(PasswordEdited);
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
            if (UpdatingAlpha >= 0.9)
            {
                // Enter new Scene
                SceneManager.LoadScene("Scenes/ShowcaseScene");
            }
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
        if (Input.GetKeyDown("t") && !CameraAnimator.GetBool("IsZoomed"))
        {
            CameraAnimator.SetBool("ShouldZoom", true);
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
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!CameraAnimator.GetBool("IsZoomed") && !bEnteringNewScene)
            {
                UpdatingAlpha = 0.01f;
                FadeOutImage.color = new Color(0.0f, 0.0f, 0.0f, UpdatingAlpha);
                bEnteringNewScene = true;
                bFadeOut = false;
            }
            else if (CameraAnimator.GetBool("IsZoomed") && !bIsPinned)
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
        Account = "egdgroup@yahoo.com";
        Password = "username123";

        AccountInputField.text = Account;
        PasswordInputField.text = "***********";
    }

    void LoginButtonClicked()
    {
        TwitterPanel.SetActive(false);
        PINPanel.SetActive(true);

        bIsPinned = true;
        Application.OpenURL(TwitterHandler.urlText.text);
    }

    void AccountEdited(string value)
    {
        Account = value;
    }

    void PasswordEdited(string value)
    {
        Password = value;
    }

    public Text StartText;
    public Image FadeOutImage;
    public GameObject TwitterPanel;
    public GameObject PINPanel;

    public Button LoginButton;
    public Button EnterPINButton;
    public InputField AccountInputField;
    public InputField PasswordInputField;
    public TwitterHandler TwitterHandler;

    public string Account { get; set; }
    public string Password { get; set; }
    private bool bEnteringNewScene;
    private bool bFadeOut;
    private float UpdatingAlpha;
    private bool bIsPinned;

    private Animator CameraAnimator;
}
