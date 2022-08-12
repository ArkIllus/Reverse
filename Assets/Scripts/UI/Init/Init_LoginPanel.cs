using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_LoginPanel : BasePanel
{
    public const string str_Init1stPanel = "Init_1stPanel";
    public const string str_Init_RegisterPanel = "Init_RegisterPanel";

    [SerializeField] private string usernameInput;
    [SerializeField] private string passwordInput;
    [SerializeField] private Text textTip;

    private void OnEnable()
    {
        HideTip();
    }

    public override void ShowMe()
    {
        //base.ShowMe();
        this.gameObject.SetActive(true);
    }

    public override void HideMe()
    {
        //base.HideMe();
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
        HideMe();
        //显示标题
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();

        //显示Init_1stPanel
        UIManager.GetInstance().ShowPanel<Init_1stPanel>(str_Init1stPanel, E_UI_Layer.Mid);
    }
    public void ClickRegister()
    {
        //直接隐藏此panel
        HideMe();

        //显示Init_RegisterPanel
        UIManager.GetInstance().ShowPanel<Init_RegisterPanel>(str_Init_RegisterPanel, E_UI_Layer.Mid);
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
