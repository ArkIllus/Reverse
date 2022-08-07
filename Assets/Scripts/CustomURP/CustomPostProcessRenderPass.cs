using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//�ٷ�ʾ���У�һ��Renderer Feature��Ӧһ���Զ������Ч�������������໥�������ô�����������׵�����
//����Ҳ�ڴˣ��໥������ζ��ÿ��Ч��������Ҫ����ʱRT���ķ���Դ��˫���廥��Ҫ�࣬
//����Renderer Feature��Renderer Data�£�����ڳ����е�Volume��˵�ڴ����е�����������û��ô���㡣
//��ô�����˼·���ǽ�������ͬ�����ĺ�������ŵ�ͬһ��Render Pass����Ⱦ�������Ϳ�������˫���彻�����ֱ�����Volume�����ơ�

public class CustomPostProcessRenderPass : ScriptableRenderPass
{
    List<CustomVolumeComponent> volumeComponents;   // �����Զ���������
    List<int> activeComponents; // ��ǰ���õ�����±�

    string profilerTag;
    List<ProfilingSampler> profilingSamplers; // ÿ�������Ӧ��ProfilingSampler

    RenderTargetHandle source;  // ��ǰԴ��Ŀ��
    RenderTargetHandle destination;
    RenderTargetHandle tempRT0; // ��ʱRT
    RenderTargetHandle tempRT1;

    /// <param name="profilerTag">Profiler��ʶ</param>
    /// <param name="volumeComponents">���ڸ�RendererPass�ĺ������</param>
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

    // �����������ʵ����Ⱦ�߼���
    // ʹ��<c>ScriptableRenderContext</c>��ִ�л�ͼ�����Command Buffer
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // �㲻��Ҫ�ֶ�����ScriptableRenderContext.submit����Ⱦ���߻����ض�λ�õ�������
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        Debug.Log("Execute");
        var cmd = CommandBufferPool.Get(profilerTag);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        // ��ȡDescriptor
        var descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.msaaSamples = 1;
        descriptor.depthBufferBits = 0;

        // ��ʼ����ʱRT
        RenderTargetIdentifier buff0, buff1;
        bool rt1Used = false;
        cmd.GetTemporaryRT(tempRT0.id, descriptor);
        buff0 = tempRT0.id;
        // ���destinationû�г�ʼ��������Ҫ��ȡRT����Ҫ��destinatonΪ_AfterPostProcessTexture�����
        if (destination != RenderTargetHandle.CameraTarget && !destination.HasInternalRenderTargetId())
        {
            cmd.GetTemporaryRT(destination.id, descriptor);
        }

        // [����]ִ��ÿ�������Render����
        // ���ֻ��һ���������ֱ��source -> buff0
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
            // ����ж���������������RT�����Һ���
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

        // ���blit��destination
        Blit(cmd, buff0, destination.Identifier());

        // �ͷ�
        cmd.ReleaseTemporaryRT(tempRT0.id);
        if (rt1Used)
            cmd.ReleaseTemporaryRT(tempRT1.id);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    /// <summary>
    /// ���ú������
    /// </summary>
    /// <returns>�Ƿ������Ч���</returns>
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
    /// ������ȾԴ����ȾĿ��
    /// </summary>
    public void Setup(RenderTargetHandle source, RenderTargetHandle destination)
    {
        Debug.Log("Setup");
        this.source = source;
        this.destination = destination;
    }
}

