using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class DashWave : MonoBehaviour
{
    public Volume volume;
    private CenterWaveVolumeComponent centerWave;

    private bool isWave;
    [SerializeField]
    private float MoveDistance;

    void Start()
    {
        ////[注]：不再需要放在第一个
        for (int i = 0; i < volume.profile.components.Count; ++i)
        {
            if (volume.profile.components[i].GetType() == typeof(CenterWaveVolumeComponent))
            {
                centerWave = volume.profile.components[i] as CenterWaveVolumeComponent;
                //Debug.Log("found " + i);
            }
        }

        //TODO:残影
    }

    //TODO:问题：只能有一个波纹
    public void DashUpdate(bool startDashEffect)
    {
        if (startDashEffect)
        {
            //在PlayerController中进行
            //Camera.main.DOShakePosition(0.5f, 0.1f);

            centerWave.isActive.SetValue(new BoolParameter(true));
            centerWave.CenterPos.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));

            isWave = true;
            //isDashing = true;

            //TODO:残影
            //StartCoroutine(ShowShadow());
            Debug.Log("dash centerWave Start!");
        }

        if (isWave)
        {
            //波纹移动距离<最大距离
            if (centerWave.MoveDistance.value < centerWave.MaxMoveDistance.value)
            {
                MoveDistance += centerWave.MoveSpeed.value * Time.deltaTime; //更新MoveDistance
            }
            else //波纹移动距离>=最大距离
            {
                isWave = false;
                MoveDistance = 0; //置零MoveDistance
                Debug.Log("dash centerWave End!");
            }
            centerWave.MoveDistance.SetValue(new FloatParameter(MoveDistance));
        }
    }

    //TODO:残影
    //IEnumerator ShowShadow()
    //{
    //}
}
