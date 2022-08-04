using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : EntityComponent //, I_OnDustDestroy
{
    // 是否处于死亡状态
    public bool isDead;

    // 死亡动画是否完成
    public bool isDeathAnimFinish;

    private Command command;
    private PlayerPause playerPause;
    private new Transform transform;
    private E_Player ePlayer;
    private ColliderCheckerItem sharpPointChecker;
    private C_Rigidbody2DProxy m_Rigidbody2DWrapper;
    //private BugColliderChecker bugColliderChecker;

    private void Awake()
    {
        command = GetComponentNotNull<Command>();
        playerPause = GetComponentNotNull<PlayerPause>();
        //transform = GetComponentNotNull<C_Transform2DProxy>().transform;
        ePlayer = GetComponentNotNull<E_Player>();
        sharpPointChecker = GetComponentNotNull<C_ColliderChecker>().GetChecker("Sharp Point Checker");
        m_Rigidbody2DWrapper = GetComponentNotNull<C_Rigidbody2DProxy>();
        //bugColliderChecker = GetComponentNotNull<BugColliderChecker>();
    }
    
    public void DeathSystem()
    {
        //死亡条件：
        //1.输入：紫砂重生
        //2.碰到尖刺
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

        // 死亡（rigidbody）无法移动，无需记录暂停前的速度
        m_Rigidbody2DWrapper.Pause(false); 
        isDead = true;

        // 隐藏Player(Sprite)
        playerPause.Hide();

        // 产生Death动画预制体（播放完后自动销毁）
        //var dust = S_Dust_Factory.Instance.CreateDust(transform.position);
        //dust.AddObserver(this);

        //TODO 完善死亡效果
        temp_DeathAndRebirth();

        // 相机抖动
        //S_MainCamera.Instance.Shake(C_CameraShake.ShakeType.Die);
    }

    public void temp_DeathAndRebirth()
    {
        OnDeathAnimFinish();

        OnRebirthAnimFinish();
    }

    public void OnDustDestroy()
    {
        if (!isDeathAnimFinish)
        {
            OnDeathAnimFinish();
        }
        else
        {
            OnRebirthAnimFinish();
        }
    }

    public void OnDeathAnimFinish()
    {
        Debug.Log("OnDeathAnimFinish");
        // 重新设置Player的位置
        ePlayer.SetPosition(
            GameManager_Main.Instance.playerRebirthPlace.position);
        m_Rigidbody2DWrapper.Resume();
        // 产生Rebirth动画预制体（播放完成后自动销毁）
        //var dust = S_Dust_Factory.Instance.CreateDust(transform.position);
        //dust.AddObserver(this);
        isDeathAnimFinish = true;
    }

    public void OnRebirthAnimFinish()
    {
        Debug.Log("OnRebirthAnimFinish");
        // 显示Player
        playerPause.Show();
        // 重设死亡状态
        isDead = false;
        isDeathAnimFinish = false;
    }
}
