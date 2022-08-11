using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class External_MainPanel : BasePanel
{
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
            case "ButtonNewGame":
                ClickNewGame();
                break;
            case "ButtonContinueGame":
                ClickContinueGame();
                break;
            case "ButtonSelectLevel":
                ClickSelectLevel();
                break;
            case "ButtonUserInfo":
                ClickUserInfo();
                break;
            case "ButtonQuit":
                ClickQuit();
                break;
        }
    }

    public void ClickNewGame()
    {
        Debug.Log("ClickNewGame");

        //TODO �����µ���Ϸ �л�����
    }
    public void ClickContinueGame()
    {
        Debug.Log("ClickContinueGame");

        //TODO ������Ϸ �л�����
    }
    public void ClickSelectLevel()
    {
        Debug.Log("ClickSelectLevel");

        //ֱ�����ش�panel
        HideMe();

        //TODO ����ѡ��ؿ�����
    }

    public void ClickUserInfo()
    {
        Debug.Log("ClickUserInfo");

        //ֱ�����ش�panel
        HideMe();

        //������������
        UIManager.GetInstance().ShowPanel<External_BgPicPanel>("External_BgPicPanel", E_UI_Layer.Bot);
        //����������Ϣ����
        UIManager.GetInstance().ShowPanel<External_UserInfoPanel>("External_UserInfoPanel", E_UI_Layer.Mid);
    }
    public void ClickQuit()
    {
        Debug.Log("ClickQuit");

        //ֱ�����ش�panel
        HideMe();

        UIManager.GetInstance().ShowPanel<Init_1stPanel>("Init_1stPanel", E_UI_Layer.Mid);
    }
}
