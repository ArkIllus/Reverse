using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : EntityComponent, I_OnDustDestroy
{
    // �Ƿ�������״̬
    public bool isDead;

    // ���������Ƿ����
    public bool isDeathAnimFinish;

    private Command command;
    //private PlayerPause playerPause;
    private new Transform transform;
    private E_Player ePlayer;
    private ColliderCheckerItem sharpPointChecker;
    private C_Rigidbody2DProxy m_Rigidbody2DWrapper;
    //private BugColliderChecker bugColliderChecker;

    private void Awake()
    {
        command = GetComponentNotNull<Command>();
        //playerPause = GetComponentNotNull<PlayerPause>();
        //transform = GetComponentNotNull<C_Transform2DProxy>().transform;
        ePlayer = GetComponentNotNull<E_Player>();
        sharpPointChecker = GetComponentNotNull<C_ColliderChecker>().GetChecker("Sharp Point Checker");
        m_Rigidbody2DWrapper = GetComponentNotNull<C_Rigidbody2DProxy>();
        //bugColliderChecker = GetComponentNotNull<BugColliderChecker>();
    }
    
    public void DeathSystem()
    {
        //����������
        //1.���룺��ɰ����
        //2.�������
        //3.TODO:...
        bool c1 = command.deathBool;
        bool c2 = sharpPointChecker.isHit;
        //bool c3 = bugColliderChecker.isHit;
        bool c3 = false;
        if (c1 || c2 || c3)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        Debug.Log("Die");

        // ������rigidbody���޷��ƶ��������¼��ͣǰ���ٶ�
        m_Rigidbody2DWrapper.Pause(false); 
        isDead = true;

        // ����Player
        //playerPause.Hide();

        // ����Death����Ԥ���壨��������Զ����٣�
        //var dust = S_Dust_Factory.Instance.CreateDust(transform.position);
        //dust.AddObserver(this);
        //S_MainCamera.Instance.Shake(C_CameraShake.ShakeType.Die);
    }

    public void OnDustDestroy()
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public void OnDeathAnimFinish()
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public void OnRebirthAnimFinish()
    {
        //TODO
        throw new System.NotImplementedException();
    }
}
