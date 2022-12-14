using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//δ?̳?BasePanel
public class OverheatPanel : Singleton<OverheatPanel>
{
    private Image image;
    private Color _Color;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
        _Color = image.color;
    }


    public void SetAlpha(float alpha)
    {
        image.color = new Color(_Color.r, _Color.g, _Color.b, alpha);
    }

    public void QuickFade()
    {
        image.DOFade(0f, 0.5f);
    }
}
