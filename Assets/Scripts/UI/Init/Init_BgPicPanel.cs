using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_BgPicPanel : BasePanel
{
    [SerializeField] private Image imageGameTitle;
    [SerializeField] private Image imageBackground;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material effectMat;

    public void HideTitle()
    {
        imageBackground.material = normalMat;
        imageGameTitle.gameObject.SetActive(false);
    }
    public void ShowTitle()
    {
        imageBackground.material = effectMat;
        imageGameTitle.gameObject.SetActive(true);
    }
}
