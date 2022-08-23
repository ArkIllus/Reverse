using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using UnityEngine.Rendering;

public class GameManager : EntitySingleton<GameManager>
{
    // 是否翻转
    public bool isReverse;

    // 玩家重生点
    //public Transform playerRebirthPlace;

    //后处理组件
    public Volume volume;

    [HideInInspector]
    public PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        //锁定120帧
        //Application.targetFrameRate = 120;
        //锁定300帧
        //Application.targetFrameRate = 300;
        //最高帧数
        //Application.targetFrameRate = -1;
        Time.timeScale = 1f;
        if (player == null)
            player = FindObjectOfType<PlayerController>();
        if (volume == null)
            volume = FindObjectOfType<Volume>();
    }
}
