using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class External_UserInfoPanel : BasePanel
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
            case "ButtonBack":
                ClickBack();
                break;
            case "ButtonVisitingCard":
                ClickVisitingCard();
                break;
            case "ButtonFriendsList":
                ClickFriendsList();
                break;
            case "ButtonAddFriends":
                ClickAddFriends();
                break;
            case "ButtonAchievements":
                ClickAchievements();
                break;
        }
    }

    private void ClickAchievements()
    {
        throw new NotImplementedException();
    }

    private void ClickAddFriends()
    {
        throw new NotImplementedException();
    }

    private void ClickFriendsList()
    {
        throw new NotImplementedException();
    }

    private void ClickVisitingCard()
    {
        throw new NotImplementedException();
    }

    private void ClickBack()
    {
        Debug.Log("ClickBack");

        //
        HideMe();
        UIManager.GetInstance().GetPanel<External_BgPicPanel>("External_BgPicPanel").HideMe();
        UIManager.GetInstance().GetPanel<External_MainPanel>("External_MainPanel").ShowMe();
    }
}
