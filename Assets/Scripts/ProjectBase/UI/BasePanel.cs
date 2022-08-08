using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 面板基类
/// 1.找到所有自己面板下的控件对象并添加到字典
/// 2.提供显示或隐藏的行为
/// </summary>
public class BasePanel : MonoBehaviour
{
    //该面板上的所有UI控件（UIBehaviour）
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Slider>();
        FindChildrenControl<InputField>();
        // ...
    }

    //显示 时需要做的事（会在UIManger的ShowPanel中调用）
    public virtual void ShowMe()
    {

    }
    //隐藏 时需要做的事（会在UIManger的HidePanel中调用）
    public virtual void HideMe()
    {

    }

    //用于自动注册按钮onClick事件
    protected virtual void OnClick(string btnName)
    {

    }
    //用于自动注册Toggle的onValueChanged事件
    protected virtual void OnValueChanged_toggle(string toggleName, bool value)
    {

    }
    //InputField...

    //查找所有T类型的控件并添加到字典
    private void FindChildrenControl<T>() where T: UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }

            //如果是按钮控件，自动注册按钮onClick事件
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            else if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged_toggle(objName, value);
                });
            }
            else if (controls[i] is InputField)
            {
                //TODO
                //(controls[i] as InputField).onValueChanged.AddListener((value) =>
                //{
                //});
            }
            //...
        }
    }

    //获取T类型的名为controlName的控件
    protected T GetControl<T>(string controlName) where T: UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }
        return null;
    }
}
