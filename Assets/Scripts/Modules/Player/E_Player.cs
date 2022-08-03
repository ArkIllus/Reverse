using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Player : Entity
{
    //// wrapper类通用组件
    //public C_Rigidbody2DProxy rigidbody2DWrapper;
    //public C_AnimatorProxy animatorProxy;
    //public C_Transform2DProxy transform2DProxy;

    //// physics类通用组件
    public C_HSpeedUp hSpeedUp;
    //public C_Gravity gravity;

    //// action
    //public C_Climb climb;
    //public C_ClimbEffect climbEffect;
    //public C_ClimbTimeBar climbTimeBar;

    //public Dash dash;
    //public DashCount dashCount;
    //public Dashing dashing;

    public Death death;
    //public Face face;
    public Jump jump;
    //public JumpOnWall jumpOnWall;
    public Walk walk;

    //// assist
    public Command command;
    public C_ColliderChecker colliderChecker;

    //// view
    //public PlayerAnimator playerAnimator;
    //public FootDust footDust;
    //public HairFlow hairFlow;
    //public HairSprite hairSprite;
    //public PlayerPause playerPause;

    //public PlatformCollider platformCollider;

    //public BugColliderChecker bugColliderChecker;

    private ColliderCheckerItem groundChecker;

    //public GrabMovePlatform grabMovePlatform;

    private void Awake()
    {
        groundChecker = colliderChecker.GetChecker("Ground Checker");
    }

    //// 瞬间移动（非物理移动）
    //// 物理移动应去改变Rigidbody的velocity
    //public void SetPosition(Vector2 position)
    //{
    //    transform2DProxy.pos = position;
    //    hairFlow.ResetPlace();
    //}

    //// 完全恢复冲刺速度
    //public bool ResumeDashCount()
    //{
    //    return dashCount.ResumeDashCount();
    //}

    //// 被直接赋予某个方向上的速度
    //public bool BeEjected(Vector2 velocity)
    //{
    //    rigidbody2DWrapper.SetVelocityAfterPhysic(velocity);
    //    dashing.AdvanceEnd(0);
    //    return true;
    //}

    private void FixedUpdate()
    {
        //hairFlow.HairFlowSystem();
    }

    private void Update()
    {
        // 玩家输入指令更新
        command.CommandSystem();

        // 更新碰撞相关的状态变量
        colliderChecker.ColliderCheckerSystem();

        // 跳跃（不在冲刺、攀墙状态时可进行），且方向键无向下输入
        //if (!climb.isSlidingOrClimbing && !dashing.isDashing)
        jump.JumpSystem();

        //行走（不在冲刺、攀墙状态时可进行）
        //if (!climb.isSlidingOrClimbing && !dashing.isDashing)
        walk.WalkSystem();

        // 水平速度控制（不在冲刺状态下可进行）
        //if (!dashing.isDashing)
        hSpeedUp.HSpeedUpSystem();

        // 触发玩家死亡系统
        death.DeathSystem();
    }
}
