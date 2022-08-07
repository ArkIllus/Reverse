using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/CenterWaveVolumeComponent")]
public class CenterWaveVolumeComponent : CustomVolumeComponent
{
    //非Shader参数
    public FloatParameter MoveSpeed = new FloatParameter(0.8f);
    [Header("MaxMoveDistance/MoveSpeed=波纹持续时间")]
    public FloatParameter MaxMoveDistance = new FloatParameter(0.3f);
    //Shader参数
    public FloatParameter WaveStrength = new FloatParameter(1f);
    public FloatParameter WaveWidth = new FloatParameter(3f);
    public FloatParameter WaveNum = new FloatParameter(80f);
    [Header("波纹半径")]
    public FloatParameter WaveRadius = new FloatParameter(0.3f);
    public FloatParameter Attenuation = new FloatParameter(0.3f);
    [Header("分辨率1920/1080")]
    public Vector2Parameter Ratios = new Vector2Parameter(new Vector2(1.77777f, 1f));
    //在冲刺时设置
    [HideInInspector]
    public FloatParameter MoveDistance = new FloatParameter(0);
    //在冲刺时设置
    [HideInInspector]
    public Vector2Parameter CenterPos = new Vector2Parameter(new Vector2(0, 0));

    Material material;
    //shader的名称 非路径
    const string shaderName = "Shader Graphs/CenterWave";

    //在冲刺时设置（isActive）
    public override bool IsActive()
    {
        return material != null && isActive.value;
    }

    //[核心]Core
    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        //Debug.Log("CenterWaveVolumeComponent Render");
        if (material == null)
            return;

        //传递参数给shader
        material.SetVector("_Center", CenterPos.value);
        material.SetFloat("_WaveRadius", MaxMoveDistance.value);
        material.SetFloat("_WaveMoveDistance", MoveDistance.value);

        //自己加的
        material.SetFloat("_WaveStrength", WaveStrength.value);
        material.SetFloat("_WaveWidth", WaveWidth.value);
        material.SetFloat("_WaveNum", WaveNum.value);
        material.SetFloat("_WaveRadius", WaveRadius.value);
        material.SetFloat("_WaveRatios", WaveStrength.value);
        material.SetFloat("_Attenuation", Attenuation.value);
        material.SetVector("_Ratios", Ratios.value);

        //Debug.Log("CenterWaveVolumeComponent Render 2");

        //Debug.Log("source: " + source.ToString());
        //Debug.Log("destination: " + destination.ToString());

        cmd.Blit(source, destination, material);
        //Render方法中的cmd.Blit之后可以考虑换成CoreUtils.DrawFullScreen画全屏三角形。
    }

    public override void Setup()
    {
        //Debug.Log("Setup");
        //创建材料
        //使用CoreUtils.CreateEngineMaterial来从Shader创建材质，在Dispose中销毁它。
        if (material == null)
            material = CoreUtils.CreateEngineMaterial(shaderName);
        //Debug.Log(material.shader.name);
        if (material == null)
            Debug.LogError("material = null!");
    }

    public override void Dispose(bool disposing) {
        base.Dispose(disposing);
        CoreUtils.Destroy(material);
    }
}
