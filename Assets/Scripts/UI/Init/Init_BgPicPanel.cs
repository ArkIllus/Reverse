using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_BgPicPanel : BasePanel
{
    [SerializeField] private Image imageGameTitle;

    public void HideTitle()
    {
        imageGameTitle.gameObject.SetActive(false);
    }
    public void ShowTitle()
    {
        imageGameTitle.gameObject.SetActive(true);
    }
}
