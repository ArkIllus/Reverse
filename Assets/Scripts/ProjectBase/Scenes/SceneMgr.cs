using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 切换场景
/// </summary>
public class SceneMgr : BaseManager<SceneMgr>
{
    public void LoadScene(string name, UnityAction fun)
    {
        // 场景同步加载
        SceneManager.LoadScene(name);
        //加载完成过后 才会执行fun
        fun();
    }

    //提供给外部的 异步加载的接口方法
    public void LoadSceneAsync(string name, UnityAction fun = null)
    {
        // 场景异步加载
        // 为何不直接SceneManager.LoadSceneAsync(name);???
        MonoManager.GetInstance().StartCoroutine(ReallyLoadSceneAsync(name, fun));
    }

    //协程异步加载场景
    private IEnumerator ReallyLoadSceneAsync(string name, UnityAction fun = null)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            //可以在这里更新进度条
            //使用事件中心
            //EventCenter.GetInstance().EventTrigger("loading", ao.progress);
            yield return ao.progress;
        }
        yield return ao;

        //加载完成过后 才会执行fun
        if (fun != null)
            fun();
    }
}
