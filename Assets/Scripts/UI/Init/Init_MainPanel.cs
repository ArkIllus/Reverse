using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        //clear关卡记录
        GameManager_global.GetInstance().gameData_SO.ClearLevelRecords();

        //TODO 场景转换过渡
        //进入第一章
        SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.Levels[0]);
    }
    public void ClickContinueGame()
    {
        Debug.Log("ClickContinueGame");

        //clear关卡记录
        GameManager_global.GetInstance().gameData_SO.ClearLevelRecords();

        //TODO 继续游戏 切换场景
    }
    public void ClickSelectLevel()
    {
        Debug.Log("ClickSelectLevel");

        //clear关卡记录
        GameManager_global.GetInstance().gameData_SO.ClearLevelRecords();

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

        if (SceneManager.GetActiveScene().name == GameData_SO.InitScene)
        {
            //直接隐藏此panel
            HideMe();

            UIManager.GetInstance().ShowPanel<Init_1stPanel>("Init_1stPanel", E_UI_Layer.Mid);

            //显示标题
            UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
        }
        else if(SceneManager.GetActiveScene().name == GameData_SO.AfterLoginScene)
        {
            SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.InitScene);
        }
    }
}
