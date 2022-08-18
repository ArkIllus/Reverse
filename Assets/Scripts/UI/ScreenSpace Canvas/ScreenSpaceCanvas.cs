using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceCanvas : Singleton<ScreenSpaceCanvas>
{
    private bool isShowQuitPanel;
    public QuitPanel quitPanel;
    public Button btnQuit;

    //protected override void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this.gameObject); //optional
    //    }
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isShowQuitPanel = !isShowQuitPanel;
            if (isShowQuitPanel)
            {
                quitPanel.ShowMe();
            }
            else
            {
                quitPanel.HideMe();
            }
        }
    }
}
