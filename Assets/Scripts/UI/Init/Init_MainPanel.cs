using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Init_MainPanel : BasePanel
{
    public Button btnContinue;
    public VerticalLayoutGroup vlo;

    public override void ShowMe()
    {
        //base.ShowMe();
        this.gameObject.SetActive(true);

        //读取上一次游玩的关卡记录，如果没有记录则不显示Continue按钮
        if (GameManager_global.GetInstance().gameData_SO.lastLevel == -1)
        {
            btnContinue.gameObject.SetActive(false);
            vlo.spacing = 60;
        }
        else
        {
            btnContinue.gameObject.SetActive(true);
            vlo.spacing = 50;
        }
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
        GameManager_global.GetInstance().gameData_SO.lastLevel = 0;
        SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.Levels[0]);
    }
    public void ClickContinueGame()
    {
        Debug.Log("ClickContinueGame");

        //不clear关卡记录

        //继续上次关卡
        SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.Levels[GameManager_global.GetInstance().gameData_SO.lastLevel]);
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
