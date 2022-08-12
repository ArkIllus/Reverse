using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_1stPanel : BasePanel
{
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnEsc;
    public const string str_Init_LoginPanel = "Init_LoginPanel";

    #region 注册Esc输入的事件，仅在PC上有效
    private void OnEnable()
    {
        InputManager.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<KeyCode>("keyDown", CheckInputDown);

        //隐藏ButtonEsc
        btnEsc.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        InputManager.GetInstance().StartOrEndCheck(false);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("keyDown", CheckInputDown);
    }
    private void CheckInputDown(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Escape:
                btnEsc.gameObject.SetActive(!btnEsc.gameObject.activeInHierarchy);
                btnStart.gameObject.SetActive(!btnStart.gameObject.activeInHierarchy);
                break;
        }
    }
    #endregion


    #region UI部分
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
            case "ButtonStart":
                ClickStart();
                break;
            case "ButtonEsc":
                ClickEsc();
                break;
        }
    }

    public void ClickStart()
    {
        //初次开始游戏，进入登录界面
        Debug.Log("ClickStart");

        ////隐藏ButtonStart & ButtonEsc
        //btnStart.gameObject.SetActive(false);
        //btnEsc.gameObject.SetActive(false);
        //直接隐藏此panel
        HideMe();

        //隐藏标题
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").HideTitle();

        UIManager.GetInstance().ShowPanel<Init_LoginPanel>(str_Init_LoginPanel, E_UI_Layer.Mid);

        //TODO：二次开始游戏，已记住密码，进入游戏外主界面
    }

    public void ClickEsc()
    {
        Debug.Log("ClickEsc");
        //直接退出，无需上传数据
        Application.Quit();
    }
    #endregion
}
