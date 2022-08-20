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

        //��ȡ��һ������Ĺؿ���¼�����û�м�¼����ʾContinue��ť
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

        //clear�ؿ���¼
        GameManager_global.GetInstance().gameData_SO.ClearLevelRecords();

        //TODO ����ת������
        //�����һ��
        GameManager_global.GetInstance().gameData_SO.lastLevel = 0;
        SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.Levels[0]);
    }
    public void ClickContinueGame()
    {
        Debug.Log("ClickContinueGame");

        //��clear�ؿ���¼

        //�����ϴιؿ�
        SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.Levels[GameManager_global.GetInstance().gameData_SO.lastLevel]);
    }
    public void ClickSelectLevel()
    {
        Debug.Log("ClickSelectLevel");

        //clear�ؿ���¼
        GameManager_global.GetInstance().gameData_SO.ClearLevelRecords();

        //ֱ�����ش�panel
        HideMe();

        //��ʾ����
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();

        //����ѡ��ؿ�����
        UIManager.GetInstance().ShowPanel<Init_SelectLevelPanel>("Init_SelectLevelPanel", E_UI_Layer.Mid);
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

        if (SceneManager.GetActiveScene().name == GameData_SO.InitScene)
        {
            //ֱ�����ش�panel
            HideMe();

            UIManager.GetInstance().ShowPanel<Init_1stPanel>("Init_1stPanel", E_UI_Layer.Mid);

            //��ʾ����
            UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
        }
        else if(SceneManager.GetActiveScene().name == GameData_SO.AfterLoginScene)
        {
            SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.InitScene);
        }
    }
}
