using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    private Room[] rooms;

    private float blendTime;
    private bool isFirst = true; //��һ��С����

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

    //TODO:�Ż�Э��
    IEnumerator StopMove()
    {
        //TODO:��Ч����ͣ
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(blendTime);
        Time.timeScale = 1;
    }
    //TODO:�Ż�Э��
    IEnumerator SlowMove()
    {
        //TODO:��Ч������
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(blendTime);
        Time.timeScale = 1;
    }
}
