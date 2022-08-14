using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Init_RegisterPanel : BasePanel
{
    public const string str_Init_LoginPanel = "Init_LoginPanel";

    [SerializeField] private string usernameInput;
    [SerializeField] private string passwordInput;
    [SerializeField] private string passwordInput_Ensure;
    [SerializeField] private Text textTip;

    public float fadeInTime = 0.8f;
    public float fadeOutTime = 0.5f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    #region 动效
    public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeInTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeInTime);
    }
    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeOutTime, false).SetEase(Ease.InOutQuint);
        Tweener tmp = canvasGroup.DOFade(0, fadeOutTime);

        //并且SetActive(false)
        tmp.onComplete += () => { this.gameObject.SetActive(false); };
    }
    #endregion

    private void OnEnable()
    {
        HideTip();
    }

    public override void ShowMe()
    {
        this.gameObject.SetActive(true);
        PanelFadeIn();
    }

    public override void HideMe()
    {
        PanelFadeOut();
    }
    public override void ShowMe_noEffect()
    {
        Debug.Log("Init_RegisterPanel   ShowMe_noEffect");
        this.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public override void HideMe_noEffect()
    {
        this.gameObject.SetActive(false);
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ButtonRegisterAndLogin":
                ClickRegisterAndLogin();
                break;
            case "ButtonBack":
                ClickBack();
                break;
            case "ButtonCancel":
                ClickCancel();
                break;
        }
    }

    public void ClickBack()
    {
        Debug.Log("ClickBack");

        //直接隐藏此panel 不包含动效
        HideMe_noEffect();

        //显示Init_LoginPanel
        UIManager.GetInstance().ShowPanel<Init_LoginPanel>(str_Init_LoginPanel, E_UI_Layer.Mid, dynamicEffect: false);
    }
    public void ClickCancel()
    {
        //TODO:...
        ClickBack();
    }
    public void ClickRegisterAndLogin()
    {
        Debug.Log("ClickRegisterAndLogin");

        if (passwordInput != passwordInput_Ensure)
        {
            ShowTip_diff();
            return;
        }
        else if (passwordInput.Length < 6 || passwordInput.Length > 20)
        {
            ShowTip_format();
            return;
        }
        StartCoroutine(HttpClient.Register(HttpClient.url,
            GameManager_global.GetInstance().gameData_SO.username_memo,
            GameManager_global.GetInstance().gameData_SO.password_memo));
    }


    protected override void OnEndEdit_Inputfield(string inputfieldName, string str)
    {
        switch (inputfieldName)
        {
            case "InputField_Username":
                EndEditUsername(str);
                break;
            case "InputFieldPassword":
                EndEditPassword(str);
                break;
            case "InputFieldEnsurePassword":
                EndEditEnsurePassword(str);
                break;
        }
    }
    public void EndEditUsername(string str)
    {
        //隐藏提示
        HideTip();

        usernameInput = str;
        GameManager_global.GetInstance().gameData_SO.username_memo = str;
    }
    public void EndEditPassword(string str)
    {
        //隐藏提示
        HideTip();

        passwordInput = str;
        GameManager_global.GetInstance().gameData_SO.password_memo = str;
    }
    public void EndEditEnsurePassword(string str)
    {
        //隐藏提示
        textTip.gameObject.SetActive(false);

        passwordInput_Ensure = str;
    }

    public void ShowTip_exist()
    {
        textTip.text = "Username already exists";
        textTip.gameObject.SetActive(true);
    }
    public void ShowTip_format()
    {
        textTip.text = "Password format error";
        textTip.gameObject.SetActive(true);
    }
    public void ShowTip_diff()
    {
        textTip.text = "Passwords inconsistent";
        textTip.gameObject.SetActive(true);
    }
    public void HideTip()
    {
        textTip.gameObject.SetActive(false);
        textTip.text = string.Empty;
    }
}
