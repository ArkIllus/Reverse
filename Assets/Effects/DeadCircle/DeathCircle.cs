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
    public float durationInToOut = 0.5f;
    public float durationOutToIn = 1f;

    //private bool outToIn; // true 从外向内播放圆圈
    //                      // false 从内向外播放圆圈
    //private bool isPlaying; //正在播放中

    void Start()
    {
        volume = GameManager.Instance.volume;

        ////[注]：不再需要放在第一个
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
    //    if (outToIn)// 从外向内播放圆圈
    //    {
    //        StartCoroutine(PlayCircleOutToIn());
    //        outToIn = false;
    //    }
    //    else
    //    {
    //        // 从内向外播放圆圈
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
        DOTween.To(() => circle.Radius.value, x => circle.Radius.value = x, minRadius, durationOutToIn);
        yield return new WaitForSeconds(durationOutToIn); //TODO优化
        //isPlaying = false;
    }

    public IEnumerator PlayCircleOutToIn(float _durationOutToIn)
    {
        Debug.Log("PlayCircleOutToIn");

        circle.isActive.SetValue(new BoolParameter(true));
        //circle.Center.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));//0~1
        circle.Center.SetValue(new Vector2Parameter(Camera.main.WorldToScreenPoint(transform.position))); //1920 1080

        //isPlaying = true;
        circle.Radius.SetValue(new FloatParameter(maxRadius));
        DOTween.To(() => circle.Radius.value, x => circle.Radius.value = x, minRadius, _durationOutToIn);
        yield return new WaitForSeconds(_durationOutToIn); //TODO优化
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
        DOTween.To(() => circle.Radius.value, x => circle.Radius.value = x, maxRadius, durationInToOut);
        yield return new WaitForSeconds(durationInToOut); //TODO优化
        //isPlaying = false;

        circle.isActive.SetValue(new BoolParameter(false));
    }

    public void SetCircleMin()
    {
        if (circle == null) //临时办法：防止为空
        {
            volume = GameManager.Instance.volume;
            ////[注]：不再需要放在第一个
            for (int i = 0; i < volume.profile.components.Count; ++i)
            {
                if (volume.profile.components[i].GetType() == typeof(CircleVolumeComponent))
                {
                    circle = volume.profile.components[i] as CircleVolumeComponent;
                }
            }
        }
        circle.Radius.SetValue(new FloatParameter(0));
    }
}
