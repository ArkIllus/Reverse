using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_MonoProxy : MonoBehaviour
{
    public Canvas canvas;
    public List<BasePanel> panelList = new List<BasePanel>();

    private void OnEnable()
    {
        if (UIManager._canvas != null)
        {
            Debug.Log("UIManager._canvas��δ���,�����" + UIManager._canvas.name);
            //TODO��������ɺ�Unload���е�UI������һ˲�����2��EventSystem���ᱨwarning��
            UIManager.GetInstance().UnloadAllUI();
        }
        UIManager._canvas = canvas;
        foreach (var panel in panelList)
        {
            if (UIManager.panelDic.ContainsKey(panel.gameObject.name)) //�����ͬ����key����remove(���¼��س�����gameobject�Ѿ�û��)�����
            {
                UIManager.panelDic.Remove(panel.gameObject.name);
            }
            UIManager.panelDic.Add(panel.gameObject.name, panel);
            Debug.Log("��� " + panel.gameObject.name);
        }
        UIManager.GetInstance().LoadAllUIatStart();
    }

    //TODO:��ʱ���
    //private void OnDisable()
    //{
    //    //UIManager._canvas = null;
    //    foreach (var panel in panelList)
    //    {
    //        UIManager.panelDic.Remove(panel.gameObject.name);
    //        Debug.Log("ɾ�� " + panel.gameObject.name);
    //    }
    //}
}
