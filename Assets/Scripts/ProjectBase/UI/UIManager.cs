using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI层级
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System
}

/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏等接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    //路径
    public static string path_UI = "UI/";

    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //canvas供外部使用
    public RectTransform canvas;

    //面板从下到上分为4个层级
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    //供外部使用
    public Transform Bot => bot;
    public Transform Mid => mid;
    public Transform Top => top;
    public Transform System => system;

    public UIManager()
    {
        //TODO: 只有一个Canvas不好，不方便动静分离
        //创建Canvas 让其过场景的时候不被移除
        GameObject objCanvas = ResourceManager.GetInstance().Load<GameObject>(path_UI + "Canvas");

        Canvas _canvas = objCanvas.GetComponent<Canvas>();
#if UNITY_EDITOR
        if (_canvas == null)
            Debug.LogError("no canvas");
#endif
        _canvas.worldCamera = Camera.main;

        canvas = objCanvas.transform as RectTransform;
        GameObject.DontDestroyOnLoad(objCanvas);

        //找到各层
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //创建EventSystem 让其过场景的时候不被移除
        objCanvas = ResourceManager.GetInstance().Load<GameObject>(path_UI + "EventSystem");
        GameObject.DontDestroyOnLoad(objCanvas);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">面板预制体创建成功后 你想做的事</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer, UnityAction<T> callBack = null) where T: BasePanel
    {
        //如果已经加载了这个面板，避免重复加载
        //TODO: 可能存在的问题：异步加载，这个面板正在加载中，字典里还没有，
        //另一处地方又调用了ShowPanel，就可能重复加载
        if (panelDic.ContainsKey(panelName))
        {
            //调用panel的ShowMe方法
            panelDic[panelName].ShowMe();
            //处理 创建面板完成后 需要执行的事
            if (callBack != null)
            {
                callBack(panelDic[panelName] as T);
            }
            return;
        }

        ResourceManager.GetInstance().LoadAsync<GameObject>(path_UI + panelName, (obj) =>
        {
            //设置父对象（4个层级之一）
            Transform parent = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    parent = mid;
                    break;
                case E_UI_Layer.Top:
                    parent = top;
                    break;
                case E_UI_Layer.System:
                    parent = system;
                    break;
            }
            obj.transform.SetParent(parent);
            //设置相对位置和大小
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            //TODO:真的要吗
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到预制体身上的面板脚本
            T panel = obj.GetComponent<T>();
#if UNITY_EDITOR
            if (panel == null)
            {
                Debug.LogError(obj.name + " panel not found!");
            }
#endif
            //处理 创建面板完成后 需要执行的事
            if (callBack != null)
            {
                callBack(panel);
            }

            //调用panel的ShowMe方法
            panel.ShowMe();

            //把面板存起来
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// 隐藏（destroy）面板
    /// 缺点：频繁destroy可能会卡，即使是异步加载
    /// </summary>
    /// <param name="panelName"></param>
    public void HideAndDestroyPanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            //调用panel的HideMe方法
            panelDic[panelName].HideMe();

            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// 得到一个已经显示的面板 供外部使用
    /// </summary>
    public T GetPanel<T>(string name) where T: BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
