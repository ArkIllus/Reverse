using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Init_MainPanel : BasePanel
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

        //TODO 进入新的游戏 切换场景
        throw new NotImplementedException();
    }
    public void ClickContinueGame()
    {
        Debug.Log("ClickContinueGame");

        //TODO 继续游戏 切换场景
        throw new NotImplementedException();
    }
    public void ClickSelectLevel()
    {
        Debug.Log("ClickSelectLevel");

        //直接隐藏此panel
        HideMe();

        //显示标题
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();

        //跳出选择关卡界面
        UIManager.GetInstance().ShowPanel<Init_SelectLevelPanel>("Init_SelectLevelPanel", E_UI_Layer.Mid);
    }

    public void ClickUserInfo()
    {
        Debug.Log("ClickUserInfo");

        //直接隐藏此panel
        HideMe();

        //更换背景界面
        UIManager.GetInstance().ShowPanel<External_BgPicPanel>("External_BgPicPanel", E_UI_Layer.Bot);
        //跳出个人信息界面
        UIManager.GetInstance().ShowPanel<External_UserInfoPanel>("External_UserInfoPanel", E_UI_Layer.Mid);
    }
    public void ClickQuit()
    {
        Debug.Log("ClickQuit");

        //直接隐藏此panel
        HideMe();

        UIManager.GetInstance().ShowPanel<Init_1stPanel>("Init_1stPanel", E_UI_Layer.Mid);

        //显示标题
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
    }
}
