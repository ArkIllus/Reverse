using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_UI : PanelsGroup
{
    //private void OnEnable()
    private void Start() //在UIManager_MonoProxy的OnEnable()之后调用
    {
        UIManager.GetInstance().ShowPanel<Init_BgPicPanel>("Init_BgPicPanel", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<Init_1stPanel>("Init_1stPanel", E_UI_Layer.Mid);
    }

    public static void HidePanels()
    {
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").HideMe();
        UIManager.GetInstance().GetPanel<Init_1stPanel>("Init_1stPanel").HideMe();
        UIManager.GetInstance().GetPanel<Init_LoginPanel>("Init_LoginPanel").HideMe();
        UIManager.GetInstance().GetPanel<Init_RegisterPanel>("Init_RegisterPanel").HideMe();
    }
    public static void HidePanelsAfterLogin()
    {
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").HideMe();
        UIManager.GetInstance().GetPanel<Init_LoginPanel>("Init_LoginPanel").HideMe();
        UIManager.GetInstance().GetPanel<Init_RegisterPanel>("Init_RegisterPanel").HideMe();
    }
}
