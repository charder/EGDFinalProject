﻿using System.Collections;
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
        StartText.text = EnterText;

        SetInputField();
        LoginButton.onClick.AddListener(LoginButtonClicked);
        EnterPINButton.onClick.AddListener(EnterPINButtonClicked);
        RequestButton.onClick.AddListener(RequestAgainButtonClicked);
        AccountInputField.onEndEdit.AddListener(AccountEdited);
        PasswordInputField.onEndEdit.AddListener(PasswordEdited);
        AccountInputField.onValueChanged.AddListener(OnAccountInfoChanging);
        PasswordInputField.onValueChanged.AddListener(OnAccountInfoChanging);
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
                StartText.text = EnterText;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (bIsPinned)
            {
                PINInputField.ActivateInputField();
            }

            if (AccountInputField.isFocused)
            {
                AccountInputField.DeactivateInputField();
                PasswordInputField.ActivateInputField();
            }
            else
            {
                AccountInputField.ActivateInputField();
                PasswordInputField.DeactivateInputField();
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
                EnterPINButtonClicked();
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

        if (TwitterHandler.urlText.text.Length > 1)
        {
            Application.OpenURL(TwitterHandler.urlText.text);
        }
    }

    void RequestAgainButtonClicked()
    {
        bIsPinned = true;
        TwitterHandler.ClearUserInfo();
        
        if(TwitterHandler.bIsReadyToStart)
        {
            bIsPinned = false;
            RequestButton.GetComponent<Text>().text = "Request Again";
        }
    }

    void EnterPINButtonClicked()
    {
        if (TwitterHandler.urlText.text.Length > 1)
        {
            TwitterHandler.EnterPIN();
        }
    }

    void AccountEdited(string value)
    {
        if (value != Account)
        {
            TwitterHandler.ClearUserInfo();
            Account = value;
        }
    }

    void PasswordEdited(string value)
    {
        if (value != Password)
        {
            TwitterHandler.ClearUserInfo();
            Password = value;
        }
    }

    void OnAccountInfoChanging(string value)
    {
        TwitterHandler.ClearUserInfo();
    }

    public bool GetPinned()
    {
        return bIsPinned;
    }

    public Text StartText;
    public Image FadeOutImage;
    public GameObject TwitterPanel;
    public GameObject PINPanel;

    public Button LoginButton;
    public Button EnterPINButton;
    public Button RequestButton;
    public InputField AccountInputField;
    public InputField PasswordInputField;
    public InputField PINInputField;
    public TwitterHandler TwitterHandler;

    public string Account { get; set; }
    public string Password { get; set; }
    private bool bEnteringNewScene;
    private bool bFadeOut;
    private float UpdatingAlpha;
    private bool bIsPinned;

    private static string EnterText = @"Press ""Enter"" to Start 
Press ""T"" to Switch Account";
    private static string EscapeText = @"Press ""Escape"" to back";

    private Animator CameraAnimator;
}
