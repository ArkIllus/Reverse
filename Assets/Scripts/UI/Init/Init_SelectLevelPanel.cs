using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_SelectLevelPanel : BasePanel
{
    public override void ShowMe()
    {
        this.gameObject.SetActive(true);
    }

    public override void HideMe()
    {
        this.gameObject.SetActive(false);
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Button1":
                ClickLevel(1);
                break;
            case "Button2":
                ClickLevel(2);
                break;
            case "Button3":
                ClickLevel(3);
                break;
            case "ButtonBack":
                ClickBack();
                break;
        }
    }
    public void ClickLevel(int level)
    {
        Debug.Log("ClickLevel " + level);
        SceneMgr.GetInstance().LoadSceneAsync("level" + level + "_1");
    }
    public void ClickBack()
    {
        Debug.Log("ClickBack");

        HideMe();
        UIManager.GetInstance().GetPanel<Init_MainPanel>("Init_MainPanel").ShowMe();
    }
}
