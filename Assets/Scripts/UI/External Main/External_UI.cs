using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class External_UI : PanelsGroup
{
    public const string str_External_BgPicPanel = "External_BgPicPanel";
    public const string str_External_MainPanel = "External_MainPanel";

    public static void ShowPanelsAtStart()
    {
        UIManager.GetInstance().ShowPanel<External_BgPicPanel>(str_External_BgPicPanel, E_UI_Layer.Bot);
        UIManager.GetInstance().ShowPanel<External_MainPanel>(str_External_MainPanel, E_UI_Layer.Mid);
    }
}
