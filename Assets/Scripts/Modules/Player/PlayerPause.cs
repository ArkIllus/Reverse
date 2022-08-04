using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPause : EntityComponent
{
    // 玩家是否暂停
    public bool isPaused;

    // 玩家是否在暂停的基础上进行了隐藏
    // 如果玩家是隐藏的，那么必然也是暂停的
    public bool isHidden { get; private set; }

    // 隐藏Player(Sprite)
    public void Hide()
    {
        if (isHidden) return;
        var srList = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in srList)
        {
            sr.enabled = false;
        }

        isPaused = true;
        isHidden = true;
    }

    // 显示Player(Sprite)
    public void Show()
    {
        if (!isHidden) return;
        var srList = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in srList)
        {
            sr.enabled = true;
        }

        isPaused = false;
        isHidden = false;
    }

    // 显示或隐藏Player(Sprite)
    public void HideOrShow(bool hide)
    {
        if (hide) Hide();
        else Show();
    }
}
