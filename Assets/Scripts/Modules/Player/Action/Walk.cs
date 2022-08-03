using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : EntityComponent
{
    // 行走的最大速度
    public float maxSpeed = 8;

    // 加速度
    public float acceleration = 80;

    // 减速度
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
        //设置C_HSpeedUp的目标速度、加速度
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
