using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{
    
}

public class EventInfo<T>: IEventInfo
{
    public UnityAction<T> actions;

    //constructor
    public EventInfo(UnityAction<T> action)
    {
        this.actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    //constructor
    public EventInfo(UnityAction action)
    {
        this.actions += action;
    }
}

/// <summary>
/// 事件中心
/// TODO: UnityAction? Action?性能对比
/// </summary>
public class EventCenter : BaseManager<EventCenter> //EventCenter<T>就不行了，泛型类和单例冲突
{
    //key 事件的名字
    //value 监听该事件的对应委托函数
    //<object>传递任何类型的对象参数，缺点是值类型会装箱
    //现在<IEventInfo>避免了装箱拆箱
    //private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    //添加事件监听
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (!eventDic.ContainsKey(name))
        {
            //eventDic.Add(name, action);
            eventDic.Add(name, new EventInfo<T>(action));
        }
        else
        {
            (eventDic[name] as EventInfo<T>).actions += action;
            //eventDic[name] += action;
        }
    }
    //添加事件监听(重载 不需要参数的事件)
    public void AddEventListener(string name, UnityAction action)
    {
        if (!eventDic.ContainsKey(name))
        {
            //eventDic.Add(name, action);
            eventDic.Add(name, new EventInfo(action));
        }
        else
        {
            (eventDic[name] as EventInfo).actions += action;
            //eventDic[name] += action;
        }
    }

    //移除事件监听
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name] -= action;
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }
    //移除事件监听(重载 不需要参数的事件)
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name] -= action;
            (eventDic[name] as EventInfo).actions -= action;
        }
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="info"></param>
    public void EventTrigger<T>(string name, T info)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name](info);
            //eventDic[name].Invoke(info);
            (eventDic[name] as EventInfo<T>).actions?.Invoke(info);
        }
    }

    /// <summary>
    /// 事件触发(重载 不需要参数的事件)
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="info"></param>
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name](info);
            //eventDic[name].Invoke(info);
            (eventDic[name] as EventInfo).actions?.Invoke();
        }
    }

    //清空事件中心
    //主要用在场景切换时
    public void Clear()
    {
        eventDic.Clear();
    }
}
