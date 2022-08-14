using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Init_SelectLevelPanel : BasePanel
{
    //[SerializeField] private Button button1;
    //[SerializeField] private Button button2;
    //[SerializeField] private Button button3;

    public float fadeTime = 0.3f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform1;
    public RectTransform rectTransform2;
    public RectTransform rectTransform3;
    public RectTransform rectTransformBack;
    Vector3 tmpScaleBack;

    public BezierController bezierController;

    protected override void Awake()
    {
        base.Awake();
        tmpScaleBack = rectTransformBack.localScale;
    }

    #region 动效
    public void PanelFadeIn()
    {
        //重新评估每个button的大小
        bezierController.InitItem();

        canvasGroup.alpha = 0f;
        Vector3 tmp1 = rectTransform1.localScale;
        Vector3 tmp2 = rectTransform2.localScale;
        Vector3 tmp3 = rectTransform3.localScale;
        rectTransform1.localScale = Vector3.zero;
        rectTransform2.localScale = Vector3.zero;
        rectTransform3.localScale = Vector3.zero;
        rectTransformBack.localScale = Vector3.zero;
        rectTransform1.DOScale(tmp1, fadeTime).SetEase(Ease.InExpo);
        rectTransform2.DOScale(tmp2, fadeTime).SetEase(Ease.InExpo);
        rectTransform3.DOScale(tmp3, fadeTime).SetEase(Ease.InExpo);
        rectTransformBack.DOScale(tmpScaleBack, fadeTime).SetEase(Ease.InExpo);
        canvasGroup.DOFade(1, fadeTime);
    }
    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        //rectTransform1.localScale = Vector3.one;
        //rectTransform2.localScale = Vector3.one;
        //rectTransform3.localScale = Vector3.one;
        rectTransform1.DOScale(Vector3.zero, fadeTime);
        rectTransform2.DOScale(Vector3.zero, fadeTime);
        rectTransform3.DOScale(Vector3.zero, fadeTime);
        rectTransformBack.DOScale(Vector3.zero, fadeTime);
        Tweener tmp = canvasGroup.DOFade(0, fadeTime);
        
        //并且SetActive(false)
        tmp.onComplete += () => {
            this.gameObject.SetActive(false);
        };
    }
    public void PanelFadeOut_AndShowInitMainPanel()
    {
        canvasGroup.alpha = 1f;
        rectTransform1.localScale = Vector3.one;
        rectTransform2.localScale = Vector3.one;
        rectTransform3.localScale = Vector3.one;
        rectTransform1.DOScale(Vector3.zero, fadeTime);
        rectTransform2.DOScale(Vector3.zero, fadeTime);
        rectTransform3.DOScale(Vector3.zero, fadeTime);
        rectTransformBack.DOScale(Vector3.zero, fadeTime);
        Tweener tmp = canvasGroup.DOFade(0, fadeTime);

        //并且SetActive(false)
        tmp.onComplete += () => {
            this.gameObject.SetActive(false);
            //FadeOut结束后再显示Init_MainPanel
            UIManager.GetInstance().GetPanel<Init_MainPanel>("Init_MainPanel").ShowMe();
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
    }
    public void HideMe_AndShowInitMainPanel()
    {
        PanelFadeOut_AndShowInitMainPanel();
    }
    public override void ShowMe_noEffect()
    {
        this.gameObject.SetActive(true);
    }

    public override void HideMe_noEffect()
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

        //HideMe();
        HideMe_AndShowInitMainPanel();
        //[注] 放到FadeOut结束后再做
        //UIManager.GetInstance().GetPanel<Init_MainPanel>("Init_MainPanel").ShowMe();
    }
}
