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
    private bool isMoving;
    [SerializeField]
    private float MoveDistance;

    void Start()
    {
        //[注]：需要放在第一个
        centerWave = volume.profile.components[0] as CenterWaveVolumeComponent;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera.main.DOShakePosition(0.5f, 0.1f);
            centerWave.isActive.SetValue(new BoolParameter(true));
            centerWave.CenterPos.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));
            isWave = true;
            //isMoving = true;
            //StartCoroutine(ShowShadow());
        }
        //if (isMoving)
        //{
        //}
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
            }
            Debug.Log("dash centerWave");
            centerWave.MoveDistance.SetValue(new FloatParameter(MoveDistance));
        }
    }

    //TODO:残影
    //IEnumerator ShowShadow()
    //{
    //centerWave.CenterPos.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));
    //}
}
