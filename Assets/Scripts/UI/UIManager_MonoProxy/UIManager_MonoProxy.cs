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
            Debug.Log("UIManager._canvas还未清空,先清空" + UIManager._canvas.name);
            //TODO：加载完成后Unload所有的UI（会有一瞬间出现2个EventSystem，会报warning）
            UIManager.GetInstance().UnloadAllUI();
        }
        UIManager._canvas = canvas;
        foreach (var panel in panelList)
        {
            if (UIManager.panelDic.ContainsKey(panel.gameObject.name)) //如果有同名的key，先remove(重新加载场景，gameobject已经没了)再添加
            {
                UIManager.panelDic.Remove(panel.gameObject.name);
            }
            UIManager.panelDic.Add(panel.gameObject.name, panel);
            Debug.Log("添加 " + panel.gameObject.name);
        }
        UIManager.GetInstance().LoadAllUIatStart();
    }

    //TODO:临时解决
    //private void OnDisable()
    //{
    //    //UIManager._canvas = null;
    //    foreach (var panel in panelList)
    //    {
    //        UIManager.panelDic.Remove(panel.gameObject.name);
    //        Debug.Log("删除 " + panel.gameObject.name);
    //    }
    //}
}
