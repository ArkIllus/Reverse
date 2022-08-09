using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoPanel : BasePanel
{
    public const string buttonVisitingCard = "ButtonVisitingCard";
    public const string buttonFriendsList = "ButtonFriendsList";
    public const string buttonAddFriends = "ButtonAddFriends";
    public const string buttonAchievements = "ButtonAchievements";
    public const string buttonBack = "ButtonBack";

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
        switch (btnName)
        {
            case buttonBack:
                Debug.Log(buttonBack + "被点击");
                break;
            case buttonVisitingCard:
                Debug.Log(buttonVisitingCard + "被点击");
                break;
            case buttonFriendsList:
                Debug.Log(buttonFriendsList + "被点击");
                break;
            case buttonAddFriends:
                Debug.Log(buttonAddFriends + "被点击");
                break;
            case buttonAchievements:
                Debug.Log(buttonAchievements + "被点击");
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
