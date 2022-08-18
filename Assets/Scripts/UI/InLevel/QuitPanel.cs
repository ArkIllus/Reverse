using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuitPanel : BasePanel
{
    public override void ShowMe()
    {
        this.gameObject.SetActive(true);
        //��ͣ
        Time.timeScale = 0f;
    }

    public override void HideMe()
    {
        this.gameObject.SetActive(false);
        //ȡ����ͣ
        Time.timeScale = 1f;
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ButtonQuit":
                ClickQuit();
                break;
            case "ButtonBack":
                ClickBack();
                break;
        }
    }

    public void ClickQuit()
    {
        Debug.Log("ClickQuit");
        //ȡ����ͣ
        Time.timeScale = 1f;
        //SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.InitScene);
        SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.AfterLoginScene);
    }

    public void ClickBack()
    {
        HideMe();
        UIManager.GetInstance().GetPanel<PausePanel>("PausePanel").ShowMe();
    }
}
