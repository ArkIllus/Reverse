using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 抽屉数据
/// （方便在Hierarchy中观察）
/// </summary>
public class PoolData
{
    //挂载List的父物体
    public GameObject parentObj;
    //对象的容器
    public List<GameObject> poolList;

    //constructor
    public PoolData(GameObject obj, GameObject poolObj)
    {
        //父物体名 = 对象obj名
        this.parentObj = new GameObject(obj.name); 
        parentObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() { };
        PushObj(obj);
    }

    public void PushObj(GameObject obj)
    {
        poolList.Add(obj);
        obj.transform.parent = parentObj.transform;
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        //显示
        obj.SetActive(true);
        //父物体设为空（方便在Hierarchy中观察）
        obj.transform.parent = null;
        return obj;
    }
}

/// <summary>
/// 缓存池模块
/// </summary>
public class PoolManager : BaseManager<PoolManager>
{
    //缓存池容器
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    /// <summary>
    /// 往外拿东西
    /// </summary>
    /// <param name="name">资源路径</param>
    /// <param name="callBack">取得资源后的回调函数</param>
    public void GetObj(string name, UnityAction<GameObject> callBack)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            callBack(poolDic[name].GetObj());
        }
        else
        {
            //异步加载
            ResourceManager.GetInstance().LoadAsync<GameObject>(name, (_obj) =>
            {
                //对象的名字改成池子的名字
                _obj.name = name;
                callBack(_obj);
            });
        }
    }

    /// <summary>
    /// 往里存东西
    /// </summary>
    /// <param name="name">该gameObject的名字</param>
    /// <param name="obj">该gameObject</param>
    public void PushObj(string name, GameObject obj)
    {
        //缓存池的根节点物体 名为Pool
        if (poolObj == null)
        {
            poolObj = new GameObject("Pool");
        }
        //设置父对象为Pool物体（方便在Hierarchy中观察）
        obj.transform.parent = poolObj.transform;

        //隐藏
        obj.SetActive(false);
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }

    /// <summary>
    /// 清空缓存池
    /// 主要用于 场景切换时
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
