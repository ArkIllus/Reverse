using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Rigidbody2DProxy : EntityComponent
{
    /// <summary>
    /// ͨ��������޸��ٶ�Ϊ�ӳٵ��޸�
    /// ����BeforePhysics��Ч���������յ��ٶȱ���Ϊ������ϵͳ��������ٶ�
    /// ��ҪĿ�����ö�̬�ľ�����ײ��ĸ��岻����������ײ�巢���ص������
    /// </summary>
    public new Rigidbody2D rigidbody;

    public Vector2 velocity => changeVelocityTrigger ? changedVelocity : rigidbody.velocity;

    public Vector2 changedVelocity;
    public bool changeVelocityTrigger;

    public Vector2 changedVelocity2;
    public bool changeVelocityTrigger2;

    public Vector2 beforePause;

    public bool isPaused { get; private set; }

    // ��ͣ�����ٶȸ���ϵͳ������¼����ͣǰ���ٶ�
    public void Pause(bool isNeedRecord = true)
    {
        if (isPaused) return;
        changeVelocityTrigger = false;
        changeVelocityTrigger2 = false;
        beforePause = isNeedRecord ? velocity : Vector2.zero;
        rigidbody.velocity = Vector2.zero;
        isPaused = true;
    }

    // �ָ������ٶȸ���ϵͳ�����ָ���ͣǰ���ٶ�
    public void Resume()
    {
        if (!isPaused) return;
        rigidbody.velocity = beforePause;
        isPaused = false;
    }

    public void PauseOrResume(bool pause)
    {
        if (pause) Pause();
        else Resume();
    }

    public void SetVelocityBeforePhysic(Vector2 newSpeed)
    {
        changedVelocity = newSpeed;
        changeVelocityTrigger = true;
    }

    public void SetXSpeedBeforePhysic(float xSpeed)
    {
        if (changeVelocityTrigger)
        {
            changedVelocity.x = xSpeed;
        }
        else
        {
            changedVelocity = rigidbody.velocity;
            changedVelocity.x = xSpeed;
            changeVelocityTrigger = true;
        }
    }

    public void SetYSpeedBeforePhysic(float ySpeed)
    {
        if (changeVelocityTrigger)
        {
            changedVelocity.y = ySpeed;
        }
        else
        {
            changedVelocity = rigidbody.velocity;
            changedVelocity.y = ySpeed;
            changeVelocityTrigger = true;
        }
    }


    public void SetVelocityAfterPhysic(Vector2 newSpeed)
    {
        changedVelocity2 = newSpeed;
        changeVelocityTrigger2 = true;
    }

    public void SetXSpeedAfterPhysic(float xSpeed)
    {
        if (changeVelocityTrigger2)
        {
            changedVelocity2.x = xSpeed;
        }
        else
        {
            changedVelocity2 = rigidbody.velocity;
            changedVelocity2.x = xSpeed;
            changeVelocityTrigger2 = true;
        }
    }

    public void SetYSpeedAfterPhysic(float ySpeed)
    {
        if (changeVelocityTrigger2)
        {
            changedVelocity2.y = ySpeed;
        }
        else
        {
            changedVelocity2 = rigidbody.velocity;
            changedVelocity2.y = ySpeed;
            changeVelocityTrigger2 = true;
        }
    }

    //����֮�� �ı�rigidbody�ٶ�
    public void S_AfterPhysics()
    {
        if (isPaused) return;

        if (changeVelocityTrigger2)
        {
            changeVelocityTrigger2 = false;
            rigidbody.velocity = changedVelocity2;
        }
    }

    //����֮ǰ �ı�rigidbody�ٶ�
    public void S_BeforePhysics()
    {
        if (isPaused) return;

        if (changeVelocityTrigger)
        {
            changeVelocityTrigger = false;
            rigidbody.velocity = changedVelocity;
        }
    }

    private void FixedUpdate()
    {
        S_BeforePhysics();
        DoAfterFixedUpdate();
    }

    protected override void AfterFixedUpdate()
    {
        S_AfterPhysics();
    }
}
