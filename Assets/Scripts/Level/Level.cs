using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    private Room[] rooms;

    private float blendTime;
    private bool isFirst = true; //第一个小房间

    private void Awake()
    {
        rooms = GetComponentsInChildren<Room>();
        foreach(Room room in rooms)
        {
            room.OnTriggerEnterEvent.AddListener(Blend);
        }

        blendTime = cinemachineBrain.m_DefaultBlend.BlendTime;
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
        yield return new WaitForSecondsRealtime(blendTime);
        Time.timeScale = 1;
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
