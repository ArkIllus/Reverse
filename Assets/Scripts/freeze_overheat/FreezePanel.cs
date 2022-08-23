using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//Î´¼Ì³ÐBasePanel
public class FreezePanel : Singleton<FreezePanel>
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
        if (image.color.a != 0)
            image.DOFade(0f, 0.5f);
    }
}
