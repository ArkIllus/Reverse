using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_1stPanel : BasePanel
{
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnEsc;
    public const string str_Init_LoginPanel = "Init_LoginPanel";

    #region ע��Esc������¼�������PC����Ч
    private void OnEnable()
    {
        InputManager.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<KeyCode>("keyDown", CheckInputDown);

        //����ButtonEsc
        btnEsc.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        InputManager.GetInstance().StartOrEndCheck(false);
        EventCenter.GetInstance().RemoveEventListener<KeyCode>("keyDown", CheckInputDown);
    }
    private void CheckInputDown(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Escape:
                btnEsc.gameObject.SetActive(!btnEsc.gameObject.activeInHierarchy);
                btnStart.gameObject.SetActive(!btnStart.gameObject.activeInHierarchy);
                break;
        }
    }
    #endregion


    #region UI����
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
            case "ButtonStart":
                ClickStart();
                break;
            case "ButtonEsc":
                ClickEsc();
                break;
        }
    }

    public void ClickStart()
    {
        //���ο�ʼ��Ϸ�������¼����
        Debug.Log("ClickStart");

        ////����ButtonStart & ButtonEsc
        //btnStart.gameObject.SetActive(false);
        //btnEsc.gameObject.SetActive(false);
        //ֱ�����ش�panel
        HideMe();

        //���ر���
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").HideTitle();

        UIManager.GetInstance().ShowPanel<Init_LoginPanel>(str_Init_LoginPanel, E_UI_Layer.Mid);

        //TODO�����ο�ʼ��Ϸ���Ѽ�ס���룬������Ϸ��������
    }

    public void ClickEsc()
    {
        Debug.Log("ClickEsc");
        //ֱ���˳��������ϴ�����
        Application.Quit();
    }
    #endregion
}
