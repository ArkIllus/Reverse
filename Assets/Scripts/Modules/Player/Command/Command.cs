using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : EntityComponent
{
    public bool isPaused;

    public int hInput;
    public int vInput;
    public float threshold = 0.1f;

    // ����
    public int walkInt;

    // ��Ծ
    public bool jumpBool;

    // �������������̵ķ���
    public Vector2Int direction;

    // ���
    public bool dashBool;

    // ����
    public bool climbBool;

    // ����
    public bool deathBool;

    public void CommandSystem() //HandleAllInputs()
    {
        // ��ͣ״̬�£���Ҫ�ṩһ��Ĭ�ϵ�����
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
