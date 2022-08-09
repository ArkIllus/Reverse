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

    //����������Ϊ��ťע���¼���
    //void Start()
    //{
    //    GetControl<Button>("Button Start").onClick.AddListener(ClickStart);
    //    GetControl<Button>("Button Quit").onClick.AddListener(ClickQuit);
    //}

    //�����Զ�ע�ᰴťonClick�¼�
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case buttonBack:
                Debug.Log(buttonBack + "�����");
                break;
            case buttonVisitingCard:
                Debug.Log(buttonVisitingCard + "�����");
                break;
            case buttonFriendsList:
                Debug.Log(buttonFriendsList + "�����");
                break;
            case buttonAddFriends:
                Debug.Log(buttonAddFriends + "�����");
                break;
            case buttonAchievements:
                Debug.Log(buttonAchievements + "�����");
                break;
        }
    }

    //protected override void OnValueChanged_toggle(string btnName, bool value)
    //{
    //    //...ͬ��
    //}

    public void InitInfo()
    {
        Debug.Log(this.name + "InitInfo");
    }
}
