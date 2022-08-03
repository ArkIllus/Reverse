using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : EntityComponent
{
    // ���ߵ�����ٶ�
    public float maxSpeed = 8;

    // ���ٶ�
    public float acceleration = 80;

    // ���ٶ�
    public float deceleration = 80;

    private C_HSpeedUp hSpeedUp;
    private Command command;

    private void Awake()
    {
        hSpeedUp = GetComponentNotNull<C_HSpeedUp>();
        command = GetComponentNotNull<Command>();
    }

    public void WalkSystem()
    {
        //����C_HSpeedUp��Ŀ���ٶȡ����ٶ�
        int walkInt = command.walkInt;
        if (walkInt != 0)
        {
            //Debug.Log("Walk");
            hSpeedUp.targetSpeed = maxSpeed * walkInt;
            hSpeedUp.acceleration = acceleration;
        }
        else
        {
            hSpeedUp.targetSpeed = 0;
            hSpeedUp.acceleration = deceleration;
        }
    }
}
