using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Init_BgPicPanel : BasePanel
{
    [SerializeField] private Image imageGameTitle;
    [SerializeField] private Image imageBackground;
    [SerializeField] private Image imageBackground2;

    [SerializeField] private Material normalMat;
    [SerializeField] private Material effectMat;

    [SerializeField] private Button btnStart;
    [SerializeField] private Image imageBtnStart;

    //[注]Blur帧数超低 不采用，使用2张图片的trick
    //public BlurPanelController panelController;

    public float fadeTime = 0.8f;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == GameData_SO.InitScene)
        {
            //SetImage_normalMat();
            imageBackground.color = new Color(imageBackground.color.r, imageBackground.color.g, imageBackground.color.b, 0);
            imageBackground2.color = new Color(imageBackground2.color.r, imageBackground2.color.g, imageBackground2.color.b, 1);
            imageGameTitle.color = new Color(imageGameTitle.color.r, imageGameTitle.color.g, imageGameTitle.color.b, 0);
            imageGameTitle.gameObject.SetActive(false);

            //2秒浮现标题和按钮完成后 再激活开始按钮
            btnStart = UIManager.GetInstance().GetPanel<Init_1stPanel>("Init_1stPanel").btnStart;
            btnStart.enabled = false;
            imageBtnStart = UIManager.GetInstance().GetPanel<Init_1stPanel>("Init_1stPanel").imageBtnStart;
            imageBtnStart.color = new Color(imageBtnStart.color.r, imageBtnStart.color.g, imageBtnStart.color.b, 0);

            //ShowTitle();
            Debug.Log("2秒浮现标题");
            //2秒浮现标题
            SetImage_effectMat();
            imageGameTitle.gameObject.SetActive(true);
            imageBackground.DOFade(1, 2);
            imageGameTitle.DOFade(1, 2);
            imageBackground2.DOFade(0, 2);
            Debug.Log("2秒浮现标题......");
            imageBtnStart.DOFade(1, 2).onComplete += () =>
            {
                btnStart.enabled = true;
                Debug.Log("2秒浮现完成");
            };
        }
        else if (SceneManager.GetActiveScene().name == GameData_SO.AfterLoginScene)
        {
            UIManager.GetInstance().ShowPanel<Init_MainPanel>("Init_MainPanel", E_UI_Layer.Mid);
            //TODO 2秒浮现标题
        }
    }

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
        imageBackground.color = new Color(imageBackground.color.r, imageBackground.color.g, imageBackground.color.b, 0);
        imageGameTitle.color = new Color(imageGameTitle.color.r, imageGameTitle.color.g, imageGameTitle.color.b, 0);
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
