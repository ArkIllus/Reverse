using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance;

    public static T Instance { get { return instance; } }
    public static bool IsInitialized { get { return instance != null; } }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this as T;
            //DontDestroyOnLoad(this.gameObject); //optional
        }
    }

    protected virtual void OnDestory()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
