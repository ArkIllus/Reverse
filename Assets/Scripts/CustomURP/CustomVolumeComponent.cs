using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// �������λ��
public enum CustomPostProcessInjectionPoint
{
    AfterOpaqueAndSky, //�����Ⱦ֮��
    BeforePostProcess, //���ú���֮ǰ
    AfterPostProcess //���ú���֮��
}

//����Ҫȷ���Զ���ĺ����������ʾ��Volume��Add Override�˵��У��Ķ�Դ���֪��
//���������������˵��в�û��ʲô����֮����ֻ��̳�VolumeComponent�ಢ�����VolumeComponentMenu���Լ��ɣ�
//��VolumeComponent��������һ��ScriptableObject��
public abstract class CustomVolumeComponent : VolumeComponent, IPostProcessComponent, IDisposable
{
    public BoolParameter isActive = new BoolParameter(false);

    // ����λ�ã�Ĭ�ϣ�AfterPostProcess��
    public virtual CustomPostProcessInjectionPoint InjectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    //��ͬһ���������ܻ���ڶ��������������Ի���Ҫһ����������ȷ��˭��˭��
    // ��InjectionPoint�е���Ⱦ˳��
    public virtual int OrderInPass => 0;

    // ��ʼ��������RenderPass�������ʱ����
    public abstract void Setup();

    //[����]Core
    // ִ����Ⱦ
    //��Ⱦ�����У���CommandBuffer��RenderingData����ȾԴ��Ŀ�궼���룺
    public abstract void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination);

    #region IPostProcessComponent
    public abstract bool IsActive();    /// ���ص�ǰ����Ƿ��ڼ���״̬

    public virtual bool IsTileCompatible() => false;
    #endregion

    #region IDisposable ������Ⱦ������Ҫ��ʱ���ɲ��ʣ������ｫ�����ͷ�
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing) { }
    #endregion
}


