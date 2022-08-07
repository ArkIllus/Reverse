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
        ////[ע]��������Ҫ���ڵ�һ��
        for (int i = 0; i < volume.profile.components.Count; ++i)
        {
            if (volume.profile.components[i].GetType() == typeof(CenterWaveVolumeComponent))
            {
                centerWave = volume.profile.components[i] as CenterWaveVolumeComponent;
                //Debug.Log("found " + i);
            }
        }

        //TODO:��Ӱ
    }

    //TODO:���⣺ֻ����һ������
    public void DashUpdate(bool startDashEffect)
    {
        if (startDashEffect)
        {
            //��PlayerController�н���
            //Camera.main.DOShakePosition(0.5f, 0.1f);

            centerWave.isActive.SetValue(new BoolParameter(true));
            centerWave.CenterPos.SetValue(new Vector2Parameter(Camera.main.WorldToViewportPoint(transform.position)));

            isWave = true;
            //isDashing = true;

            //TODO:��Ӱ
            //StartCoroutine(ShowShadow());
            Debug.Log("dash centerWave Start!");
        }

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
                Debug.Log("dash centerWave End!");
            }
            centerWave.MoveDistance.SetValue(new FloatParameter(MoveDistance));
        }
    }

    //TODO:��Ӱ
    //IEnumerator ShowShadow()
    //{
    //}
}
