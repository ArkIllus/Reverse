using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Init_1stPanel : BasePanel
{
    public Button btnStart;
    public Image imageBtnStart;
    public Button btnEsc;
    public const string str_Init_LoginPanel = "Init_LoginPanel";

    public float fadeInTime = 3.0f;
    public float fadeOutTime = 0.5f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

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

    #region 动效
    public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, fadeInTime);
    }
    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        Tweener tmp = canvasGroup.DOFade(0, fadeOutTime);

        //并且SetActive(false)
        tmp.onComplete += () => { this.gameObject.SetActive(false); };
    }
    #endregion
    #region UI部分
    public override void ShowMe()
    {
        this.gameObject.SetActive(true); 
        PanelFadeIn();
    }

    public override void HideMe()
    {
        //this.gameObject.SetActive(false);
        PanelFadeOut();
    }
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
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
