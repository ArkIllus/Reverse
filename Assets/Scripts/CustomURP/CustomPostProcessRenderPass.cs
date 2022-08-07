using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//官方示例中，一个Renderer Feature对应一个自定义后处理效果，各个后处理相互独立，好处是灵活自由易调整；
//坏处也在此，相互独立意味着每个效果都可能要开临时RT，耗费资源比双缓冲互换要多，
//并且Renderer Feature在Renderer Data下，相对于场景中的Volume来说在代码中调用起来反而没那么方便。
//那么这里的思路便是将所有相同插入点的后处理组件放到同一个Render Pass下渲染，这样就可以做到双缓冲交换，又保持了Volume的优势。

public class CustomPostProcessRenderPass : ScriptableRenderPass
{
    List<CustomVolumeComponent> volumeComponents;   // 所有自定义后处理组件
    List<int> activeComponents; // 当前可用的组件下标

    string profilerTag;
    List<ProfilingSampler> profilingSamplers; // 每个组件对应的ProfilingSampler

    RenderTargetHandle source;  // 当前源与目标
    RenderTargetHandle destination;
    RenderTargetHandle tempRT0; // 临时RT
    RenderTargetHandle tempRT1;

    /// <param name="profilerTag">Profiler标识</param>
    /// <param name="volumeComponents">属于该RendererPass的后处理组件</param>
    public CustomPostProcessRenderPass(string profilerTag, List<CustomVolumeComponent> volumeComponents)
    {
        //Debug.Log("CustomPostProcessRenderPass");
        this.profilerTag = profilerTag;
        this.volumeComponents = volumeComponents;
        activeComponents = new List<int>(volumeComponents.Count);
        profilingSamplers = volumeComponents.Select(c => new ProfilingSampler(c.ToString())).ToList();

        tempRT0.Init("_TemporaryRenderTexture0");
        tempRT1.Init("_TemporaryRenderTexture1");
    }

    // 你可以在这里实现渲染逻辑。
    // 使用<c>ScriptableRenderContext</c>来执行绘图命令或Command Buffer
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // 你不需要手动调用ScriptableRenderContext.submit，渲染管线会在特定位置调用它。
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        Debug.Log("Execute");
        var cmd = CommandBufferPool.Get(profilerTag);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        // 获取Descriptor
        var descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.msaaSamples = 1;
        descriptor.depthBufferBits = 0;

        // 初始化临时RT
        RenderTargetIdentifier buff0, buff1;
        bool rt1Used = false;
        cmd.GetTemporaryRT(tempRT0.id, descriptor);
        buff0 = tempRT0.id;
        // 如果destination没有初始化，则需要获取RT，主要是destinaton为_AfterPostProcessTexture的情况
        if (destination != RenderTargetHandle.CameraTarget && !destination.HasInternalRenderTargetId())
        {
            cmd.GetTemporaryRT(destination.id, descriptor);
        }

        // [核心]执行每个组件的Render方法
        // 如果只有一个组件，则直接source -> buff0
        if (activeComponents.Count == 1)
        {
            int index = activeComponents[0];
            using (new ProfilingScope(cmd, profilingSamplers[index]))
            {
                //Debug.Log("Execute 1");
                volumeComponents[index].Render(cmd, ref renderingData, source.Identifier(), buff0);
            }
        }
        else
        {
            // 如果有多个组件，则在两个RT上左右横跳
            //Debug.Log("Execute 2");
            cmd.GetTemporaryRT(tempRT1.id, descriptor);
            buff1 = tempRT1.id;
            rt1Used = true;
            Blit(cmd, source.Identifier(), buff0);
            for (int i = 0; i < activeComponents.Count; i++)
            {
                int index = activeComponents[i];
                var component = volumeComponents[index];
                using (new ProfilingScope(cmd, profilingSamplers[index]))
                {
                    component.Render(cmd, ref renderingData, buff0, buff1);
                }
                CoreUtils.Swap(ref buff0, ref buff1);
            }
        }

        // 最后blit到destination
        Blit(cmd, buff0, destination.Identifier());

        // 释放
        cmd.ReleaseTemporaryRT(tempRT0.id);
        if (rt1Used)
            cmd.ReleaseTemporaryRT(tempRT1.id);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    /// <summary>
    /// 设置后处理组件
    /// </summary>
    /// <returns>是否存在有效组件</returns>
    public bool SetupComponents()
    {
        //Debug.Log("SetupComponents");
        activeComponents.Clear();
        for (int i = 0; i < volumeComponents.Count; i++)
        {
            volumeComponents[i].Setup();
            if (volumeComponents[i].IsActive())
            {
                activeComponents.Add(i);
            }
        }
        return activeComponents.Count != 0;
    }

    /// <summary>
    /// 设置渲染源和渲染目标
    /// </summary>
    public void Setup(RenderTargetHandle source, RenderTargetHandle destination)
    {
        Debug.Log("Setup");
        this.source = source;
        this.destination = destination;
    }
}

