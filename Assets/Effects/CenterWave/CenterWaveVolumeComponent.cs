using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/CenterWaveVolumeComponent")]
public class CenterWaveVolumeComponent : CustomVolumeComponent
{
    //��Shader����
    public FloatParameter MoveSpeed = new FloatParameter(0.8f);
    [Header("MaxMoveDistance/MoveSpeed=���Ƴ���ʱ��")]
    public FloatParameter MaxMoveDistance = new FloatParameter(0.3f);
    //Shader����
    public FloatParameter WaveStrength = new FloatParameter(1f);
    public FloatParameter WaveWidth = new FloatParameter(3f);
    public FloatParameter WaveNum = new FloatParameter(80f);
    [Header("���ư뾶")]
    public FloatParameter WaveRadius = new FloatParameter(0.3f);
    public FloatParameter Attenuation = new FloatParameter(0.3f);
    [Header("�ֱ���1920/1080")]
    public Vector2Parameter Ratios = new Vector2Parameter(new Vector2(1.77777f, 1f));
    //�ڳ��ʱ����
    [HideInInspector]
    public FloatParameter MoveDistance = new FloatParameter(0);
    //�ڳ��ʱ����
    [HideInInspector]
    public Vector2Parameter CenterPos = new Vector2Parameter(new Vector2(0, 0));

    Material material;
    //shader������ ��·��
    const string shaderName = "Shader Graphs/CenterWave";

    //�ڳ��ʱ���ã�isActive��
    public override bool IsActive()
    {
        return material != null && isActive.value;
    }

    //[����]Core
    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        //Debug.Log("CenterWaveVolumeComponent Render");
        if (material == null)
            return;

        //���ݲ�����shader
        material.SetVector("_Center", CenterPos.value);
        material.SetFloat("_WaveRadius", MaxMoveDistance.value);
        material.SetFloat("_WaveMoveDistance", MoveDistance.value);

        //�Լ��ӵ�
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
        //Render�����е�cmd.Blit֮����Կ��ǻ���CoreUtils.DrawFullScreen��ȫ�������Ρ�
    }

    public override void Setup()
    {
        //Debug.Log("Setup");
        //��������
        //ʹ��CoreUtils.CreateEngineMaterial����Shader�������ʣ���Dispose����������
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
