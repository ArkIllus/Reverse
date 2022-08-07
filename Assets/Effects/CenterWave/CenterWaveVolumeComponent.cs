using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CenterWaveVolumeComponent : CustomVolumeComponent
{
    public FloatParameter MoveSpeed = new FloatParameter(0.8f);
    public FloatParameter MaxMoveDistance = new FloatParameter(0.3f);

    [HideInInspector]
    public FloatParameter MoveDistance = new FloatParameter(0);

    [HideInInspector]
    public Vector2Parameter CenterPos = new Vector2Parameter(new Vector2(0, 0));

    Material material;
    //shader的路径
    const string shaderName = "Shader Graphs/CenterWave";

    public override bool IsActive()
    {
        return material != null && isActive.value;
    }

    //[核心]Core
    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        Debug.Log("CenterWaveVolumeComponent Render");
        if (material == null)
            return;
        //传递参数给shader
        //material.SetTexture("_MainTex", )
        material.SetVector("_Center", CenterPos.value);
        material.SetFloat("_WaveRadius", MaxMoveDistance.value);
        material.SetFloat("_WaveMoveDistance", MoveDistance.value);
        //Debug.Log("CenterWaveVolumeComponent Render 2");

        //Debug.Log("source: " + source.ToString() + " destination: " + destination.ToString());

        cmd.Blit(source, destination, material);
    }

    public override void Setup()
    {
        //Debug.Log("Setup");
        //创建材料
        if (material == null)
            material = CoreUtils.CreateEngineMaterial(shaderName);
        //Debug.Log(material.shader.name);
    }

    public override void Dispose(bool disposing) {
        base.Dispose(disposing);
        CoreUtils.Destroy(material);
    }
}
