using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/BlurVolumeComponent")]
public class BlurVolumeComponent : CustomVolumeComponent
{
    public ClampedFloatParameter BlurRadius = new ClampedFloatParameter(0, 0, 150); //ģ���뾶
    public FloatParameter TextureSize = new FloatParameter(640); //������С

    Material material;
    const string shaderName = "Learn/2DBlur";

    // ����λ�ã�Ĭ�ϣ�AfterPostProcess��

    public override bool IsActive()
    {
        //[ע]��������û�м�����Զ������������������д˷�����ÿ֡��
        //Debug.Log("IsActive = " + (material != null && BlurRadius.value > 0));
        return material != null && BlurRadius.value > 0;
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;
        //���ݲ�����shader
        material.SetFloat("_BlurRadius", BlurRadius.value);
        material.SetFloat("_TextureSize", TextureSize.value);
        cmd.Blit(source, destination, material);
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
