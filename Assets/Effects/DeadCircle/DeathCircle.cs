using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class DeathCircle : MonoBehaviour
{
    private Volume volume;
    private CircleVolumeComponent circle;

    public float maxRadius = 1f;
    public float minRadius = 0f;
    public float duration = 0.5f;

    //private bool outToIn; // true �������ڲ���ԲȦ
    //                      // false �������ⲥ��ԲȦ
    //private bool isPlaying; //���ڲ�����

    void Start()
    {
        volume = GameManager.Instance.volume;

        ////[ע]��������Ҫ���ڵ�һ��
        for (int i = 0; i < volume.profile.components.Count; ++i)
        {
            if (volume.profile.components[i].GetType() == typeof(CircleVolumeComponent))
            {
                circle = volume.profile.components[i] as CircleVolumeComponent;
            }
        }
    }

    //public void CircleUpdate()
    //{
    //    if (outToIn)// �������ڲ���ԲȦ
    //    {
    //        StartCoroutine(PlayCircleOutToIn());
    //        outToIn = false;
    //    }
    //    else
    //    {
    //        // �������ⲥ��ԲȦ
    //        StartCoroutine(PlayCircleOutToIn());
    //        outToIn = true;
    //    }
    //}

    public IEnumerator PlayCircleOutToIn()
    {
        Debug.Log("PlayCircleOutToIn");

        circle.isActive.SetValue(new BoolParameter(true));
        //circle.Center.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));//0~1
        circle.Center.SetValue(new Vector2Parameter(Camera.main.WorldToScreenPoint(transform.position))); //1920 1080

        //isPlaying = true;
        circle.Radius.SetValue(new FloatParameter(maxRadius));
        DOTween.To(() => circle.Radius.value, x => circle.Radius.value = x, minRadius, duration);
        yield return new WaitForSeconds(duration); //TODO�Ż�
        //isPlaying = false;
    }
    public IEnumerator PlayCircleInToOut()
    {
        Debug.Log("PlayCircleInToOut");

        //circle.Center.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));//0~1
        circle.Center.SetValue(new Vector2Parameter(Camera.main.WorldToScreenPoint(transform.position))); //1920 1080
        Debug.Log("circle.Center = " + circle.Center);

        //isPlaying = true;
        circle.Radius.SetValue(new FloatParameter(minRadius));
        circle.isActive.SetValue(new BoolParameter(true));
        DOTween.To(() => circle.Radius.value, x => circle.Radius.value = x, maxRadius, duration);
        yield return new WaitForSeconds(duration); //TODO�Ż�
        //isPlaying = false;

        circle.isActive.SetValue(new BoolParameter(false));
    }

    public void SetCircleMin()
    {
        circle.Radius.SetValue(new FloatParameter(0));
    }
}