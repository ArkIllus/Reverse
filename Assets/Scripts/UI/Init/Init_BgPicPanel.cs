using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Init_BgPicPanel : BasePanel
{
    [SerializeField] private Image imageGameTitle;
    [SerializeField] private Image imageBackground;
    [SerializeField] private Image imageBackground2;

    [SerializeField] private Material normalMat;
    [SerializeField] private Material effectMat;

    //[注]Blur帧数超低 不采用，使用2张图片的trick
    //public BlurPanelController panelController;

    public float fadeTime = 0.8f;

    //显示标题 消除模糊
    public void HideTitle()
    {
        Debug.Log("HideTitle");
        SetImage_normalMat();
        imageBackground.DOFade(0, fadeTime);
        imageGameTitle.DOFade(0, fadeTime);
        Tweener tmp = imageBackground2.DOFade(1, fadeTime);
        tmp.onComplete += () => {
            imageGameTitle.gameObject.SetActive(false);
        };

        //StartCoroutine(panelController.BlurPanel());
    }
    //隐藏标题 开启模糊
    public void ShowTitle()
    {
        Debug.Log("ShowTitle");
        SetImage_effectMat();
        imageGameTitle.gameObject.SetActive(true);
        imageBackground.DOFade(1, fadeTime);
        imageGameTitle.DOFade(1, fadeTime);
        Tweener tmp = imageBackground2.DOFade(0, fadeTime);
        //tmp.onComplete += () => {
        //    SetImage_effectMat();
        //};

        //StartCoroutine(panelController.DisBlurPanel());
    }

    public void SetImage_normalMat()
    {
        imageBackground.material = normalMat;
    }
    public void SetImage_effectMat()
    {
        imageBackground.material = effectMat;
    }
}
