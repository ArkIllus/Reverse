using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : BasePanel
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
            case "ButtonPause":
                ClickPause();
                break;
        }
    }

    public void ClickPause()
    {
        Debug.Log("ClickPause");
        HideMe();
        UIManager.GetInstance().GetPanel<QuitPanel>("QuitPanel").ShowMe();
    }
}
