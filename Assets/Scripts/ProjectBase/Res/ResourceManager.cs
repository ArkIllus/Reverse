using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// TODO:AssetBundle
/// </summary>
public class ResourceManager : BaseManager<ResourceManager>
{
    //同步加载资源
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是GameObject类型，将其实例化后再返回出去
#if UNITY_EDITOR
        if (res == null)
        {
            Debug.LogError(name + " not found!");
        }
#endif
        if (res is GameObject)
        {
            return GameObject.Instantiate(res) as T;
        }
        else
        {
            return res;
        }
    }

    //异步加载资源
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T: Object
    {
        //开启异步加载的协程
        //Resources.LoadAsync(name); //错
        MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync(name, callback));
    }

    //真正的协程函数 用于开启加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;
#if UNITY_EDITOR
        if (r.asset == null)
        {
            Debug.LogError(name + " not found!");
        }
#endif

        if (r.asset is GameObject)
        {
            //如果对象是GameObject类型，将其实例化后再返回出去
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }
}
