using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class GameManager : EntitySingleton<GameManager>
{
    // 是否翻转
    public bool isReverse;

    // 玩家重生点
    public Transform playerRebirthPlace;

    [HideInInspector]
    public PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        //锁定120帧
        Application.targetFrameRate = 120;
        Time.timeScale = 1f;

        player = FindObjectOfType<PlayerController>();
    }
}
