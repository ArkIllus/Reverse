using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessRendererFeature : ScriptableRendererFeature
{
    // 不同插入点的render pass
    CustomPostProcessRenderPass afterOpaqueAndSky;
    CustomPostProcessRenderPass beforePostProcess;
    CustomPostProcessRenderPass afterPostProcess;

    // 所有自定义的VolumeComponent
    List<CustomVolumeComponent> components;

    // 用于after PostProcess的render target
    RenderTargetHandle afterPostProcessTexture;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //Debug.Log("AddRenderPasses");

        //CustomPostProcessRenderPass中定义了一个变量activeComponents来存储当前可用的的后处理组件，
        //在Render Feature的AddRenderPasses中，需要先判断Render Pass中是否有组件处于激活状态，
        //如果没有一个组件激活，那么就没必要添加这个Render Pass，
        //这里调用先前在组件中定义好的Setup方法初始化，随后调用IsActive判断其是否处于激活状态
        if (renderingData.cameraData.postProcessEnabled)
        {
            // 为每个render pass设置render target
            var source = new RenderTargetHandle(renderer.cameraColorTarget);
            if (afterOpaqueAndSky.SetupComponents())
            {
                afterOpaqueAndSky.Setup(source, source);
                renderer.EnqueuePass(afterOpaqueAndSky);
            }
            if (beforePostProcess.SetupComponents())
            {
                beforePostProcess.Setup(source, source);
                renderer.EnqueuePass(beforePostProcess);
            }
            if (afterPostProcess.SetupComponents())
            {
                // 如果下一个Pass是FinalBlit，则输入与输出均为_AfterPostProcessTexture
                source = renderingData.cameraData.resolveFinalTarget ? afterPostProcessTexture : source;
                afterPostProcess.Setup(source, source);
                renderer.EnqueuePass(afterPostProcess);
            }
        }
    }

    // 初始化Feature资源，每当序列化发生时都会调用
    public override void Create()
    {
        //Debug.Log("Create");
        // 从VolumeManager获取所有自定义的VolumeComponent
        var stack = VolumeManager.instance.stack;
        components = VolumeManager.instance.baseComponentTypeArray
            .Where(t => t.IsSubclassOf(typeof(CustomVolumeComponent)) && stack.GetComponent(t) != null)
            .Select(t => stack.GetComponent(t) as CustomVolumeComponent)
            .ToList();

        // 初始化不同插入点的render pass
        var afterOpaqueAndSkyComponents = components
            .Where(c => c.InjectionPoint == CustomPostProcessInjectionPoint.AfterOpaqueAndSky)
            .OrderBy(c => c.OrderInPass)
            .ToList();
        afterOpaqueAndSky = new CustomPostProcessRenderPass("Custom PostProcess after Opaque and Sky", afterOpaqueAndSkyComponents);
        afterOpaqueAndSky.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        var beforePostProcessComponents = components
            .Where(c => c.InjectionPoint == CustomPostProcessInjectionPoint.BeforePostProcess)
            .OrderBy(c => c.OrderInPass)
            .ToList();
        beforePostProcess = new CustomPostProcessRenderPass("Custom PostProcess before PostProcess", beforePostProcessComponents);
        beforePostProcess.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        //----------------------[核心]----------------------
        var afterPostProcessComponents = components
            .Where(c => c.InjectionPoint == CustomPostProcessInjectionPoint.AfterPostProcess)
            .OrderBy(c => c.OrderInPass)
            .ToList();
        afterPostProcess = new CustomPostProcessRenderPass("Custom PostProcess after PostProcess", afterPostProcessComponents);
        // 为了确保输入为_AfterPostProcessTexture，这里插入到AfterRendering而不是AfterRenderingPostProcessing
        afterPostProcess.renderPassEvent = RenderPassEvent.AfterRendering;

        // 初始化用于after PostProcess的render target
        //定义afterPostProcessTexture变量的目的便是为了能获取到_AfterPostProcessTexture，处理后再渲染到它。
        afterPostProcessTexture.Init("_AfterPostProcessTexture");
        //----------------------[核心]----------------------
    }

    //资源释放
    protected override void Dispose(bool disposing)
    {
        //Debug.Log("Dispose");
        base.Dispose(disposing);
        if (disposing && components != null)
        {
            foreach (var item in components)
            {
                item.Dispose();
            }
        }
    }
}

