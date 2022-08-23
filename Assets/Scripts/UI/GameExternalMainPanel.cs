using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameExternalMainPanel : BasePanel
{
    public const string buttonNewGame = "ButtonNewGame";
    public const string buttonContinueGame = "ButtonContinueGame";
    public const string buttonSelectLevel = "ButtonSelectLevel";
    public const string buttonUserInfo = "ButtonUserInfo";

    protected override void Awake()
    {
        base.Awake();
        //...
    }

    //现在无需再为按钮注册事件了
    //void Start()
    //{
    //    GetControl<Button>("Button Start").onClick.AddListener(ClickStart);
    //    GetControl<Button>("Button Quit").onClick.AddListener(ClickQuit);
    //}

    //用于自动注册按钮onClick事件
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName) 
        {
            case buttonNewGame:
                Debug.Log(buttonNewGame + "被点击");
                break;
            case buttonContinueGame:
                Debug.Log(buttonContinueGame + "被点击");
                break;
            case buttonSelectLevel:
                Debug.Log(buttonSelectLevel + "被点击");
                break;
            case buttonUserInfo:
                Debug.Log(buttonUserInfo + "被点击");
                break;
        }
    }

    //protected override void OnValueChanged_toggle(string btnName, bool value)
    //{
    //    //...同上
    //}

    public void InitInfo()
    {
        Debug.Log(this.name + "InitInfo");
    }
}
