using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : EntitySingleton<GameManager>
{
    // 玩家重生点
    public Transform playerRebirthPlace;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
}
