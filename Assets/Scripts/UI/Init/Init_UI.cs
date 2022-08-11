using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_UI : PanelsGroup
{
    public const string str_InitBgPicPanel = "Init_BgPicPanel";
    public const string str_Init1stPanel = "Init_1stPanel";
    public const string str_Init_LoginPanel = "Init_LoginPanel";
    public const string str_Init_RegisterPanel = "Init_Init_RegisterPanel";

    private void OnEnable()
    {
        UIManager.GetInstance().ShowPanel<Init_BgPicPanel>(str_InitBgPicPanel, E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<Init_1stPanel>(str_Init1stPanel, E_UI_Layer.Mid);
    }

    public static void HideAndDestroyAllPanels()
    {
        UIManager.GetInstance().HideAndDestroyPanel(str_InitBgPicPanel);
        UIManager.GetInstance().HideAndDestroyPanel(str_Init1stPanel);
        UIManager.GetInstance().HideAndDestroyPanel(str_Init_LoginPanel);
        UIManager.GetInstance().HideAndDestroyPanel(str_Init_RegisterPanel);
    }
    public static void HidePanelsAfterLogin()
    {
        UIManager.GetInstance().HideAndDestroyPanel(str_Init1stPanel);
        UIManager.GetInstance().HideAndDestroyPanel(str_Init_LoginPanel);
        UIManager.GetInstance().HideAndDestroyPanel(str_Init_RegisterPanel);
    }
}
