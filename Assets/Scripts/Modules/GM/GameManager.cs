using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : EntitySingleton<GameManager>
{
    // ���������
    public Transform playerRebirthPlace;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
}
