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

        //ֱ�����ش�panel
        HideMe();

        //��ʾInit_LoginPanel
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
        //������ʾ
        HideTip();

        usernameInput = str;
        GameManager_global.GetInstance().gameData_SO.username_memo = str;
    }
    public void EndEditPassword(string str)
    {
        //������ʾ
        HideTip();

        passwordInput = str;
        GameManager_global.GetInstance().gameData_SO.password_memo = str;
    }
    public void EndEditEnsurePassword(string str)
    {
        //������ʾ
        textTip.gameObject.SetActive(false);

        passwordInput_Ensure = str;
    }

    public void ShowTip_exist()
    {
        textTip.text = "�û����Ѵ���";
        textTip.gameObject.SetActive(true);
    }
    public void ShowTip_format()
    {
        textTip.text = "�����ʽ����";
        textTip.gameObject.SetActive(true);
    }
    public void ShowTip_diff()
    {
        textTip.text = "�������벻һ��";
        textTip.gameObject.SetActive(true);
    }
    public void HideTip()
    {
        textTip.gameObject.SetActive(false);
        textTip.text = string.Empty;
    }
}
