using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Main : EntitySingleton<GameManager_Main>
{
    // 是否翻转
    public bool isReverse;

    // 玩家重生点
    public Transform playerRebirthPlace;

    protected override void Awake()
    {
        base.Awake();
        //锁定60帧
        Application.targetFrameRate = 60;
    }
}
