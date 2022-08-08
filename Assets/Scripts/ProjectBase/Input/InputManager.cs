using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用InputManager的输入控制模块
/// TODO:InputSystem
/// </summary>
public class InputManager : BaseManager<InputManager>
{
    private bool isChecking = false;

    //constructor
    public InputManager()
    {
        MonoManager.GetInstance().AddUpdateListener(UpdateAllInputs);
    }

    /// <summary>
    /// 开启或关闭输入检测
    /// </summary>
    public void StartOrEndCheck(bool isCheck)
    {
        isChecking = isCheck;
    }

    private void UpdateAllInputs()
    {
        if (!isChecking)
            return;
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
    }

    /// <summary>
    /// 检测键盘按键抬起、按下
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            EventCenter.GetInstance().EventTrigger("keyDown", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("keyUp", key);
        }
    }
}
