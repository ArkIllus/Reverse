using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Player : Entity
{
    //// wrapper��ͨ�����
    //public C_Rigidbody2DProxy rigidbody2DWrapper;
    //public C_AnimatorProxy animatorProxy;
    //public C_Transform2DProxy transform2DProxy;

    //// physics��ͨ�����
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

    //// ˲���ƶ����������ƶ���
    //// �����ƶ�Ӧȥ�ı�Rigidbody��velocity
    //public void SetPosition(Vector2 position)
    //{
    //    transform2DProxy.pos = position;
    //    hairFlow.ResetPlace();
    //}

    //// ��ȫ�ָ�����ٶ�
    //public bool ResumeDashCount()
    //{
    //    return dashCount.ResumeDashCount();
    //}

    //// ��ֱ�Ӹ���ĳ�������ϵ��ٶ�
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
        // �������ָ�����
        command.CommandSystem();

        // ������ײ��ص�״̬����
        colliderChecker.ColliderCheckerSystem();

        // ��Ծ�����ڳ�̡���ǽ״̬ʱ�ɽ��У����ҷ��������������
        //if (!climb.isSlidingOrClimbing && !dashing.isDashing)
        jump.JumpSystem();

        //���ߣ����ڳ�̡���ǽ״̬ʱ�ɽ��У�
        //if (!climb.isSlidingOrClimbing && !dashing.isDashing)
        walk.WalkSystem();

        // ˮƽ�ٶȿ��ƣ����ڳ��״̬�¿ɽ��У�
        //if (!dashing.isDashing)
        hSpeedUp.HSpeedUpSystem();

        // �����������ϵͳ
        death.DeathSystem();
    }
}
