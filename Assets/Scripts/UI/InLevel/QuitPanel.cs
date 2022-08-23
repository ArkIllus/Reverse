using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuitPanel : BasePanel
{
    public override void ShowMe()
    {
        this.gameObject.SetActive(true);
        //暂停
        Time.timeScale = 0f;
        GameManager.Instance.player.isPaused = true;
    }

    public override void HideMe()
    {
        this.gameObject.SetActive(false);
        //取消暂停
        Time.timeScale = 1f;
        GameManager.Instance.player.isPaused = false;
    }

    protected override void OnClick(string btnName)
    {
        Debug.Log("OnClick");
        base.OnClick(btnName);
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
        //取消暂停
        Time.timeScale = 1f;
        MusicManager.GetInstance().StopBGM();
        //GameManager.Instance.player.isPaused = false;
        //SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.InitScene);
        SceneMgr.GetInstance().LoadSceneAsync(GameData_SO.AfterLoginScene);
    }

    public void ClickBack()
    {
        HideMe();
        UIManager.GetInstance().GetPanel<PausePanel>("PausePanel").ShowMe();
    }
}
