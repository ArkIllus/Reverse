using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/FireVolumeComponent")]
public class FireVolumeComponent : CustomVolumeComponent
{
    //Shader参数
    public FloatParameter FullscreenIntensity = new FloatParameter(0.1f);
    public FloatParameter VoronoiSpeed = new FloatParameter(5f);
    public FloatParameter VoronoiScale = new FloatParameter(5f);
    public FloatParameter VignettenIntensity = new FloatParameter(1.1f);
    public FloatParameter VignettleRadiusPower = new FloatParameter(7.5f);
    public FloatParameter VoronoiPower = new FloatParameter(2.3f);
    //public ColorParameter Color = new ColorParameter(Color.white); // ???

    Material material;
    //shader的名称 非路径
    const string shaderName = "Shader Graphs/fire";

    public override bool IsActive()
    {
        return material != null && isActive.value;
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;

        //传递参数给shader
        material.SetFloat("_FullscreenIntensity", FullscreenIntensity.value);
        material.SetFloat("_VoronoiSpeed", VoronoiSpeed.value);
        material.SetFloat("_VoronoiScale", VoronoiScale.value);
        material.SetFloat("_VignettenIntensity", VignettenIntensity.value);
        material.SetFloat("_VignettleRadiusPower", VignettleRadiusPower.value);
        material.SetFloat("_VoronoiPower", VoronoiPower.value);

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
