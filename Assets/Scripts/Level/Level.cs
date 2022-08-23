using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : Singleton<Level>
{
    public CinemachineBrain cinemachineBrain;
    private Room[] rooms;
    public bool isLastRoom;
    private float blendTime;
    private bool isFirst = true; //第一个小房间

    private float timer;

    private void Awake()
    {
        base.Awake();
        rooms = GetComponentsInChildren<Room>();
        foreach(Room room in rooms)
        {
            room.OnTriggerEnterEvent.AddListener(Blend);
        }

        blendTime = cinemachineBrain.m_DefaultBlend.BlendTime;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 30)
        {
            timer -= 30;
            GameManager_global.GetInstance().gameData_SO.UpdateLevelRecords();
        }
    }

    public void Blend()
    {
        if (!isFirst)
        {
            StartCoroutine(SlowMove());
        }
        else
        {
            isFirst = false;
        }
    }

    //TODO:优化协程
    IEnumerator StopMove()
    {
        //TODO:特效不暂停
        Time.timeScale = 0;
        GameManager.Instance.player.isPaused = true;
        yield return new WaitForSecondsRealtime(blendTime);
        Time.timeScale = 1;
        GameManager.Instance.player.isPaused = false;
    }
    //TODO:优化协程
    IEnumerator SlowMove()
    {
        //TODO:特效不减速
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(blendTime);
        Time.timeScale = 1;
    }
}
