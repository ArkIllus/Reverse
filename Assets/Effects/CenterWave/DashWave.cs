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
        //[ע]����Ҫ���ڵ�һ��
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
            //�����ƶ�����<������
            if (centerWave.MoveDistance.value < centerWave.MaxMoveDistance.value)
            {
                MoveDistance += centerWave.MoveSpeed.value * Time.deltaTime; //����MoveDistance
            }
            else //�����ƶ�����>=������
            {
                isWave = false;
                MoveDistance = 0; //����MoveDistance
            }
            Debug.Log("dash centerWave");
            centerWave.MoveDistance.SetValue(new FloatParameter(MoveDistance));
        }
    }

    //TODO:��Ӱ
    //IEnumerator ShowShadow()
    //{
    //centerWave.CenterPos.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));
    //}
}
