using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_MonoProxy : MonoBehaviour
{
    public Canvas canvas;
    public List<BasePanel> panelList = new List<BasePanel>();

    private void OnEnable()
    {
        UIManager._canvas = canvas;
        foreach (var panel in panelList)
        {
            UIManager.panelDic.Add(panel.gameObject.name, panel);
            Debug.Log("��� " + panel.gameObject.name);
        }
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
