using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : EntitySingleton<GameManager>
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
