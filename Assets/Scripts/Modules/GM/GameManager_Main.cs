using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Main : EntitySingleton<GameManager_Main>
{
    // �Ƿ�ת
    public bool isReverse;

    // ���������
    public Transform playerRebirthPlace;

    protected override void Awake()
    {
        base.Awake();
        //����60֡
        Application.targetFrameRate = 60;
    }
}
