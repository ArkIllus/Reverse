using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// 后处理插入位置
public enum CustomPostProcessInjectionPoint
{
    AfterOpaqueAndSky, //天空渲染之后
    BeforePostProcess, //内置后处理之前
    AfterPostProcess //内置后处理之后
}

public abstract class CustomVolumeComponent : VolumeComponent, IPostProcessComponent, IDisposable
{
    public BoolParameter isActive = new BoolParameter(false);

    // 插入位置
    public virtual CustomPostProcessInjectionPoint InjectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    //在同一个插入点可能会存在多个后处理组件，所以还需要一个排序编号来确定谁先谁后：
    // 在InjectionPoint中的渲染顺序
    public virtual int OrderInPass => 0;

    // 初始化，将在RenderPass加入队列时调用
    public abstract void Setup();

    //[核心]Core
    // 执行渲染
    public abstract void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination);

    #region IPostProcessComponent
    public abstract bool IsActive();    /// 返回当前组件是否处于激活状态

    public virtual bool IsTileCompatible() => false;
    #endregion

    #region IDisposable 由于渲染可能需要临时生成材质，在这里将它们释放
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing) { }
    #endregion
}


