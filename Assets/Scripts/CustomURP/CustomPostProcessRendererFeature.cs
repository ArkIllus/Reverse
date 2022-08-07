using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessRendererFeature : ScriptableRendererFeature
{
    // ��ͬ������render pass
    CustomPostProcessRenderPass afterOpaqueAndSky;
    CustomPostProcessRenderPass beforePostProcess;
    CustomPostProcessRenderPass afterPostProcess;

    // �����Զ����VolumeComponent
    List<CustomVolumeComponent> components;

    // ����after PostProcess��render target
    RenderTargetHandle afterPostProcessTexture;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //Debug.Log("AddRenderPasses");

        //CustomPostProcessRenderPass�ж�����һ������activeComponents���洢��ǰ���õĵĺ��������
        //��Render Feature��AddRenderPasses�У���Ҫ���ж�Render Pass���Ƿ���������ڼ���״̬��
        //���û��һ����������ô��û��Ҫ������Render Pass��
        //���������ǰ������ж���õ�Setup������ʼ����������IsActive�ж����Ƿ��ڼ���״̬
        if (renderingData.cameraData.postProcessEnabled)
        {
            // Ϊÿ��render pass����render target
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
                // �����һ��Pass��FinalBlit���������������Ϊ_AfterPostProcessTexture
                source = renderingData.cameraData.resolveFinalTarget ? afterPostProcessTexture : source;
                afterPostProcess.Setup(source, source);
                renderer.EnqueuePass(afterPostProcess);
            }
        }
    }

    // ��ʼ��Feature��Դ��ÿ�����л�����ʱ�������
    public override void Create()
    {
        //Debug.Log("Create");
        // ��VolumeManager��ȡ�����Զ����VolumeComponent
        var stack = VolumeManager.instance.stack;
        components = VolumeManager.instance.baseComponentTypeArray
            .Where(t => t.IsSubclassOf(typeof(CustomVolumeComponent)) && stack.GetComponent(t) != null)
            .Select(t => stack.GetComponent(t) as CustomVolumeComponent)
            .ToList();

        // ��ʼ����ͬ������render pass
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

        //----------------------[����]----------------------
        var afterPostProcessComponents = components
            .Where(c => c.InjectionPoint == CustomPostProcessInjectionPoint.AfterPostProcess)
            .OrderBy(c => c.OrderInPass)
            .ToList();
        afterPostProcess = new CustomPostProcessRenderPass("Custom PostProcess after PostProcess", afterPostProcessComponents);
        // Ϊ��ȷ������Ϊ_AfterPostProcessTexture��������뵽AfterRendering������AfterRenderingPostProcessing
        afterPostProcess.renderPassEvent = RenderPassEvent.AfterRendering;

        // ��ʼ������after PostProcess��render target
        //����afterPostProcessTexture������Ŀ�ı���Ϊ���ܻ�ȡ��_AfterPostProcessTexture�����������Ⱦ������
        afterPostProcessTexture.Init("_AfterPostProcessTexture");
        //----------------------[����]----------------------
    }

    //��Դ�ͷ�
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

