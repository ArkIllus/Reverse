using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class External_UI : PanelsGroup
{
    public static void ShowPanelsAtStart()
    {
        //UIManager.GetInstance().ShowPanel<External_BgPicPanel>("External_BgPicPanel", E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<Init_MainPanel>("Init_MainPanel", E_UI_Layer.Mid);
    }
}
