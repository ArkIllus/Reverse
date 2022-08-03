using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : EntityComponent
{
    public bool isPaused;

    public int hInput;
    public int vInput;
    public float threshold = 0.1f;

    // 行走
    public int walkInt;

    // 跳跃
    public bool jumpBool;

    // 方向键（决定冲刺的方向）
    public Vector2Int direction;

    // 冲刺
    public bool dashBool;

    // 攀爬
    public bool climbBool;

    // 死亡
    public bool deathBool;

    public void CommandSystem() //HandleAllInputs()
    {
        // 暂停状态下，需要提供一个默认的输入
        if (isPaused)
        {
            jumpBool = false;
            walkInt = 0;
            direction = Vector2Int.zero;
            dashBool = false;
            climbBool = false;
            deathBool = false;
        }
        else
        {
            GetHorizontalInput();
            GetVerticalInput();
            walkInt = hInput;
            direction = new Vector2Int(walkInt, vInput);

            jumpBool = Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Joystick1Button0);
            //direction = Tool_Input.GetHVInput();
            dashBool = Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button1);
            climbBool = Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Joystick1Button4);
            deathBool = Input.GetKeyDown(KeyCode.Return);
        }
    }

    /// <summary>
    /// <returns>0, -1, 1</returns>
    /// </summary>
    public void GetHorizontalInput()
    {
        float tmp = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(tmp) > threshold)
        {
            hInput = tmp > 0 ? 1 : -1;
        }
        else
        {
            hInput = 0;
        }
    }

    /// <summary>
    /// <returns>0, -1, 1</returns>
    /// </summary>
    public void GetVerticalInput()
    {
        float tmp = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(tmp) > threshold)
        {
            vInput = tmp > 0 ? 1 : -1;
        }
        else
        {
            vInput = 0;
        }
    }
}
