using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class External_AchievementsPanel : BasePanel
{
    public List<Image> images;

    public float fadeInTime = 0.3f;
    public float fadeOutTime = 0.2f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    private Vector3 originScale;

    protected override void Awake()
    {
        base.Awake();
        originScale = rectTransform.localScale;

        //更新成就解锁进度
        GameData_SO gameData_SO = GameManager_global.GetInstance().gameData_SO;
        ShowAch(gameData_SO.ach_1_Firstmeet, 0);
        ShowAch(gameData_SO.ach_2_Overload, 1);
        ShowAch(gameData_SO.ach_3_Pass, 3);
        ShowAch(gameData_SO.ach_4_Firstcake, 2);
        ShowAch(gameData_SO.ach_5_Allcake, 5);
        ShowAch(gameData_SO.ach_6_FirstEnchant, 4);
    }
    public void ShowAch(Achievement_SO achievement_SO, int index)
    {
        if (achievement_SO.isComplete)
        {
            images[index].color = Color.white;
        }
        else
        {
            images[index].color = Color.gray;
        }
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
        UIManager.GetInstance().GetPanel<External_VisitingCardPanel>("External_VisitingCardPanel").HideMe();

        this.gameObject.SetActive(true);
        PanelFadeIn();
    }

    public override void HideMe()
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        PanelFadeOut();
        //this.gameObject.SetActive(false);

        //显示标题
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
    }
}
