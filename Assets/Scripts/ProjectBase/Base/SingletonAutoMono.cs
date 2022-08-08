using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 继承Monobehaviour
// 想用时直接GetInstance，自动创建GameObject
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            //设置对象的名字为脚本名
            GameObject go = new GameObject();
            go.name = typeof(T).ToString();
            instance = go.AddComponent<T>();
            //可选：让单例模式对象 过场景 不移除
            DontDestroyOnLoad(go);
        }
        return instance;
    }
}
