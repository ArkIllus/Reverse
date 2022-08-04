using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPause : EntityComponent
{
    // ����Ƿ���ͣ
    public bool isPaused;

    // ����Ƿ�����ͣ�Ļ����Ͻ���������
    // �����������صģ���ô��ȻҲ����ͣ��
    public bool isHidden { get; private set; }

    // ����Player(Sprite)
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

    // ��ʾPlayer(Sprite)
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

    // ��ʾ������Player(Sprite)
    public void HideOrShow(bool hide)
    {
        if (hide) Hide();
        else Show();
    }
}
