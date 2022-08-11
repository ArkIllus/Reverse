using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public const string gameExternalMainPanel = "GameExternalMainPanel";

    void Start()
    {
        UIManager.GetInstance().ShowPanel<GameExternalMainPanel>(gameExternalMainPanel, E_UI_Layer.Mid, ShowPanelOver);
    }

    private void ShowPanelOver(GameExternalMainPanel panel)
    {
        panel.InitInfo();
        //Invoke(nameof(Hide), 3);
    }

    public void Hide()
    {
        UIManager.GetInstance().HideAndDestroyPanel(gameExternalMainPanel);
    }
}
