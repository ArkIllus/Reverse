using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/IceVolumeComponent")]
public class IceVolumeComponent : CustomVolumeComponent
{
    //Shader参数
    public FloatParameter Sides = new FloatParameter(1f);
    public FloatParameter Emission = new FloatParameter(1f);

    Material material;
    //shader的名称 非路径
    const string shaderName = "Shader Graphs/ice";

    public override bool IsActive()
    {
        return material != null && isActive.value;
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;

        //传递参数给shader
        material.SetFloat("_Sides", Sides.value);
        material.SetFloat("_Emission", Emission.value);

        cmd.Blit(source, destination, material);
        //Render方法中的cmd.Blit之后可以考虑换成CoreUtils.DrawFullScreen画全屏三角形。
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
