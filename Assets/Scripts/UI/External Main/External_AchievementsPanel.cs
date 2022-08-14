using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class External_AchievementsPanel : BasePanel
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
