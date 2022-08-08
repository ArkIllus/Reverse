using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 双锁double lock
// 不继承MonoBehaviour
public class BaseManager<T> where T : new()
{
    private static T instance;

    //保险起见：私有化(protected)构造函数
    protected BaseManager()
    {

    }

    public static T GetInstance()
    {
        if(instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}
