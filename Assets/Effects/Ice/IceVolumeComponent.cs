using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/IceVolumeComponent")]
public class IceVolumeComponent : CustomVolumeComponent
{
    //Shader����
    public FloatParameter Sides = new FloatParameter(1f);
    public FloatParameter Emission = new FloatParameter(1f);

    Material material;
    //shader������ ��·��
    const string shaderName = "Shader Graphs/ice";

    public override bool IsActive()
    {
        return material != null && isActive.value;
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;

        //���ݲ�����shader
        material.SetFloat("_Sides", Sides.value);
        material.SetFloat("_Emission", Emission.value);

        cmd.Blit(source, destination, material);
        //Render�����е�cmd.Blit֮����Կ��ǻ���CoreUtils.DrawFullScreen��ȫ�������Ρ�
    }

    public override void Setup()
    {
        if (material == null)
            material = CoreUtils.CreateEngineMaterial(shaderName);
        if (material == null)
            Debug.LogError("material = null!");
    }

    public override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        CoreUtils.Destroy(material);
    }
}
