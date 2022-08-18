using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class External_VisitingCardPanel : BasePanel
{
    public Text textUID;
    public Text textUsername;

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

    #region ��Ч
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

        //����SetActive(false)
        tmp.onComplete += () => {
            this.gameObject.SetActive(false);
        };
    }
    #endregion

    public override void ShowMe()
    {
        textUID.text = GameManager_global.GetInstance().gameData_SO.uid.ToString();
        textUsername.text = GameManager_global.GetInstance().gameData_SO.username_memo;

        UIManager.GetInstance().GetPanel<External_AchievementsPanel>("External_AchievementsPanel").HideMe();

        this.gameObject.SetActive(true);
        PanelFadeIn();
    }

    public override void HideMe()
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        PanelFadeOut();
        //this.gameObject.SetActive(false);

        //��ʾ����
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").ShowTitle();
    }
}
