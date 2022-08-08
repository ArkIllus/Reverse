using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono的管理者
/// 让没有继承MonoBehaviour的对象的函数也能帧更新（Update）
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (updateEvent != null)
        {
            //updateEvent();
            updateEvent.Invoke();
        }
    }

    //给外部提供的 添加帧更新事件的函数
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun; 
    }

    //给外部提供的 移除帧更新事件的函数
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
