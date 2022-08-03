using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySingleton<T> : Entity where T : EntitySingleton<T>
{
    protected static T instance;
    public static T Instance { get { return instance; } }
    public static bool IsInitialized { get { return instance != null; } }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Singleton instantiate failed.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this as T;
            //DontDestroyOnLoad(this.gameObject); //optional
        }
    }

    private void OnDestory()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}