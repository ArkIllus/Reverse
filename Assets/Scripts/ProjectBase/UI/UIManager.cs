using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI�㼶
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System
}

/// <summary>
/// UI������
/// 1.����������ʾ�����
/// 2.�ṩ���ⲿ ��ʾ�����صȽӿ�
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    //·��
    public static string path_UI = "UI/";

    //��̬����_canvas���ⲿʹ��
    public static Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //canvas���ⲿʹ��
    public RectTransform canvas;
    //��̬����_canvas���ⲿʹ�ã��Ƿ�����Canvas��EventSystem
    public static Canvas _canvas;

    //�����µ��Ϸ�Ϊ4���㼶
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    //���ⲿʹ��
    public Transform Bot => bot;
    public Transform Mid => mid;
    public Transform Top => top;
    public Transform System => system;

    public UIManager()
    {
        //TODO:��ʱ����������ж��Ƿ�����Canvas��EventSystem
        if (_canvas != null)
        {
            _canvas.worldCamera = Camera.main;
            canvas = _canvas.transform as RectTransform;
            GameObject.DontDestroyOnLoad(canvas.gameObject);
            //�ҵ�����
            bot = canvas.Find("Bot");
            mid = canvas.Find("Mid");
            top = canvas.Find("Top");
            system = canvas.Find("System");

            GameObject.DontDestroyOnLoad(EventSystem.current.gameObject);
            return;
        }

        //TODO: ֻ��һ��Canvas���ã������㶯������
        //����Canvas �����������ʱ�򲻱��Ƴ�
        GameObject obj = ResourceManager.GetInstance().Load<GameObject>(path_UI + "Canvas");

        _canvas = obj.GetComponent<Canvas>();
#if UNITY_EDITOR
        if (_canvas == null)
            Debug.LogError("no canvas");
#endif
        _canvas.worldCamera = Camera.main;

        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        //�ҵ�����
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //����EventSystem �����������ʱ�򲻱��Ƴ�
        obj = ResourceManager.GetInstance().Load<GameObject>(path_UI + "EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">���ű�����</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="layer">��ʾ����һ��</param>
    /// <param name="callBack">���Ԥ���崴���ɹ��� ����������</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer, UnityAction<T> callBack = null, bool dynamicEffect = true) where T: BasePanel
    {
        Debug.Log("ShowPanel " + panelName);
        //����Ѿ������������壬�����ظ�����
        //TODO: ���ܴ��ڵ����⣺�첽���أ����������ڼ����У��ֵ��ﻹû�У�
        //��һ���ط��ֵ�����ShowPanel���Ϳ����ظ�����
        if (panelDic.ContainsKey(panelName))
        {
            Debug.Log("�Ѵ��� " + panelName);
            //����panel��ShowMe����
            if (dynamicEffect)
                panelDic[panelName].ShowMe();
            else
                panelDic[panelName].ShowMe_noEffect();
            //���� ���������ɺ� ��Ҫִ�е���
            if (callBack != null)
            {
                callBack(panelDic[panelName] as T);
            }
            return;
        }

        Debug.Log("������ " + panelName);
        ResourceManager.GetInstance().LoadAsync<GameObject>(path_UI + panelName, (obj) =>
        {
            //���ø�����4���㼶֮һ��
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
            //�������λ�úʹ�С
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            //TODO:���Ҫ��
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //�õ�Ԥ�������ϵ����ű�
            T panel = obj.GetComponent<T>();
#if UNITY_EDITOR
            if (panel == null)
            {
                Debug.LogError(obj.name + " panel not found!");
            }
#endif
            //���� ���������ɺ� ��Ҫִ�е���
            if (callBack != null)
            {
                callBack(panel);
            }

            //����panel��ShowMe����
            if (dynamicEffect)
                panelDic[panelName].ShowMe();
            else
                panelDic[panelName].ShowMe_noEffect();

            //����������
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// ���أ�destroy�����
    /// ȱ�㣺Ƶ��destroy���ܻῨ����ʹ���첽����
    /// </summary>
    /// <param name="panelName"></param>
    public void HideAndDestroyPanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            //����panel��HideMe����
            panelDic[panelName].HideMe();

            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// �õ�һ���Ѿ���ʾ����� ���ⲿʹ��
    /// </summary>
    public T GetPanel<T>(string name) where T: BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }

    /// <summary>
    /// ���ؼ�����Զ����¼�����
    /// </summary>
    /// <param name="control">�ؼ�����</param>
    /// <param name="type">�¼�����</param>
    /// <param name="callBack">�¼�����Ӧ����</param>
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
