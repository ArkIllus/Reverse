using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class External_UserInfoPanel : BasePanel
{
    public float fadeInTime = 0.3f;
    public float fadeOutTime = 0.2f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    private Vector3 originScale;

    protected override void Awake()
    {
        base.Awake();
        originScale = rectTransform.localScale;
    }

    #region 动效
    public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(originScale, fadeInTime);
        canvasGroup.DOFade(1, fadeInTime);
    }
    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.DOScale(Vector3.zero, fadeInTime);
        Tweener tmp = canvasGroup.DOFade(0, fadeOutTime);

        //并且SetActive(false)
        tmp.onComplete += () => {
            this.gameObject.SetActive(false);
        };
    }
    #endregion

    public override void ShowMe()
    {
        this.gameObject.SetActive(true); 
        PanelFadeIn();
    }

    public override void HideMe()
    {
        PanelFadeOut();
        //this.gameObject.SetActive(false);

        //显示标题
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
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
        UIManager.GetInstance().ShowPanel<External_AchievementsPanel>("External_AchievementsPanel", E_UI_Layer.Mid);
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
        ////隐藏标题
        //UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").HideTitle();

        UIManager.GetInstance().ShowPanel<External_VisitingCardPanel>("External_VisitingCardPanel", E_UI_Layer.Mid);
    }

    private void ClickBack()
    {
        Debug.Log("ClickBack");

        //
        HideMe();
        UIManager.GetInstance().GetPanel<External_BgPicPanel>("External_BgPicPanel").HideMe();
        UIManager.GetInstance().GetPanel<External_VisitingCardPanel>("External_VisitingCardPanel").HideMe();
        UIManager.GetInstance().GetPanel<External_AchievementsPanel>("External_AchievementsPanel").HideMe();

        UIManager.GetInstance().GetPanel<Init_MainPanel>("Init_MainPanel").ShowMe();
    }
}
