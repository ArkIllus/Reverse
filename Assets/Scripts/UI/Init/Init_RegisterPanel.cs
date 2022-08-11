using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_RegisterPanel : BasePanel
{
    public const string str_Init_LoginPanel = "Init_LoginPanel";

    [SerializeField] private string usernameInput;
    [SerializeField] private string passwordInput;
    [SerializeField] private string passwordInput_Ensure;
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
            case "ButtonRegisterAndLogin":
                ClickRegisterAndLogin();
                break;
            case "ButtonBack":
                ClickBack();
                break;
        }
    }

    public void ClickBack()
    {
        Debug.Log("ClickBack");

        //直接隐藏此panel
        HideMe();

        //显示Init_LoginPanel
        UIManager.GetInstance().ShowPanel<Init_LoginPanel>(str_Init_LoginPanel, E_UI_Layer.Mid);
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
        textTip.text = "用户名已存在";
        textTip.gameObject.SetActive(true);
    }
    public void ShowTip_format()
    {
        textTip.text = "密码格式错误";
        textTip.gameObject.SetActive(true);
    }
    public void ShowTip_diff()
    {
        textTip.text = "两次密码不一致";
        textTip.gameObject.SetActive(true);
    }
    public void HideTip()
    {
        textTip.gameObject.SetActive(false);
        textTip.text = string.Empty;
    }
}
