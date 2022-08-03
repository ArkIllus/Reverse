using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : EntityComponent
{
    // �����ٶ�
    public float jumpSpeed = 20f;

    // ��Ծ�¼�
    public bool jumpEvent;

    private Command command;
    private ColliderCheckerItem groundChecker;
    private ColliderCheckerItem platformChecker;
    private C_Rigidbody2DProxy rigidbody2DWrapper;

    private void Awake()
    {
        command = GetComponentNotNull<Command>();
        groundChecker = GetComponentNotNull<C_ColliderChecker>().GetChecker("Ground Checker");
        platformChecker = GetComponentNotNull<C_ColliderChecker>().GetChecker("Platform Checker");
        rigidbody2DWrapper = GetComponentNotNull<C_Rigidbody2DProxy>();
    }

    public void JumpSystem()
    {
        jumpEvent = false;
        //if (platformChecker.isHit && command.directionVector2Int.y < 0) return;
        //if (command.jumpBool && groundChecker.isHit)
        if (command.jumpBool)
        {
            Debug.Log("Jump");
            rigidbody2DWrapper.SetYSpeedBeforePhysic(jumpSpeed);
            jumpEvent = true;
        }
    }
}
