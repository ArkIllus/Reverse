using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom Post-processing/CircleVolumeComponent")]
public class CircleVolumeComponent : CustomVolumeComponent
{
    public FloatParameter Radius = new FloatParameter(1); //���ڵİ뾶
    public Vector2Parameter Center = new Vector2Parameter(new Vector2(500f, 500f)); //����

    Material material;
    const string shaderName = "Learn/Circle";

    // ����λ�ã�Ĭ�ϣ�AfterPostProcess��

    public override bool IsActive()
    {
        //[ע]��������û�м�����Զ������������������д˷�����ÿ֡��
        return material != null && isActive.value;
    }

    public override void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
        if (material == null)
            return;
        //���ݲ�����shader
        material.SetFloat("_Radius", Radius.value);
        material.SetFloat("_CenterX", ((Vector2)Center).x);
        material.SetFloat("_CenterY", ((Vector2)Center).y);
        //Debug.Log("Center = " + (Vector2)Center);
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
