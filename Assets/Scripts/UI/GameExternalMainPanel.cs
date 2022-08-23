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

    //����������Ϊ��ťע���¼���
    //void Start()
    //{
    //    GetControl<Button>("Button Start").onClick.AddListener(ClickStart);
    //    GetControl<Button>("Button Quit").onClick.AddListener(ClickQuit);
    //}

    //�����Զ�ע�ᰴťonClick�¼�
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName) 
        {
            case buttonNewGame:
                Debug.Log(buttonNewGame + "�����");
                break;
            case buttonContinueGame:
                Debug.Log(buttonContinueGame + "�����");
                break;
            case buttonSelectLevel:
                Debug.Log(buttonSelectLevel + "�����");
                break;
            case buttonUserInfo:
                Debug.Log(buttonUserInfo + "�����");
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
