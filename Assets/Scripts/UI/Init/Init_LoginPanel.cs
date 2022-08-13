using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Init_LoginPanel : BasePanel
{
    public const string str_Init1stPanel = "Init_1stPanel";
    public const string str_Init_RegisterPanel = "Init_RegisterPanel";

    [SerializeField] private string usernameInput;
    [SerializeField] private string passwordInput;
    [SerializeField] private Text textTip;

    public float fadeTime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    #region 动效
    public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
    }
    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        Tweener tmp = canvasGroup.DOFade(0, fadeTime);

        //并且SetActive(false)
        tmp.onComplete += () => { 
            this.gameObject.SetActive(false);
        };
    }
    public void PanelFadeOut_AndShowInit1stPanel()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        Tweener tmp = canvasGroup.DOFade(0, fadeTime);

        //并且SetActive(false)
        tmp.onComplete += () => {
            this.gameObject.SetActive(false);
            //FadeOut结束后再显示Init_1stPanel
            UIManager.GetInstance().ShowPanel<Init_1stPanel>(str_Init1stPanel, E_UI_Layer.Mid);
        };
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
    public void HideMe_AndShowInit1stPanel()
    {
        PanelFadeOut_AndShowInit1stPanel();
    }
    public override void ShowMe_noEffect()
    {
        this.gameObject.SetActive(true);
    }

    public override void HideMe_noEffect()
    {
        this.gameObject.SetActive(false);
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ButtonLogin":
                ClickLogin();
                break;
            case "ButtonBack":
                ClickBack();
                break;
            case "ButtonRegister":
                ClickRegister();
                break;
        }
    }

    public void ClickLogin()
    {
        Debug.Log("ClickLogin");

        if (passwordInput.Length < 6 || passwordInput.Length > 20)
        {
            ShowTip_format();
            return;
        }
        StartCoroutine(HttpClient.Login(HttpClient.url,
            GameManager_global.GetInstance().gameData_SO.username_memo,
            GameManager_global.GetInstance().gameData_SO.password_memo));
    }
    public void ClickBack()
    {
        Debug.Log("ClickBack");

        //直接隐藏此panel
        //HideMe();
        HideMe_AndShowInit1stPanel();
        //显示标题
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
        //[注] 放到FadeOut结束后再做
        ////显示Init_1stPanel
        //UIManager.GetInstance().ShowPanel<Init_1stPanel>(str_Init1stPanel, E_UI_Layer.Mid);
    }
    public void ClickRegister()
    {
        //直接隐藏此panel 不包含动效
        HideMe_noEffect(); 

        //显示Init_RegisterPanel
        UIManager.GetInstance().ShowPanel<Init_RegisterPanel>(str_Init_RegisterPanel, E_UI_Layer.Mid, dynamicEffect: false);
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
    
    public void ShowTip_format()
    {
        textTip.text = "Password format error";
        textTip.gameObject.SetActive(true);
    }
    public void ShowTip_error()
    {
        textTip.text = "Username or password error";
        textTip.gameObject.SetActive(true);
    }
    public void HideTip()
    {
        textTip.gameObject.SetActive(false);
        textTip.text = string.Empty;
    }
}
