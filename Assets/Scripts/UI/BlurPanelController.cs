using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class BlurPanelController : MonoBehaviour
{
    public Volume volume;
    private BlurVolumeComponent blur;

    public float blurMaxRadius = 15f;
    public float blurMinRadius = 2f;

    void Start()
    {
        ////[注]：不再需要放在第一个
        for (int i = 0; i < volume.profile.components.Count; ++i)
        {
            if (volume.profile.components[i].GetType() == typeof(BlurVolumeComponent))
            {
                blur = volume.profile.components[i] as BlurVolumeComponent;
                //Debug.Log("found " + i);
            }
        }
        blur.BlurRadius.SetValue(new FloatParameter(0)); //模糊半径=0 不模糊
    }

    public IEnumerator BlurPanel()
    {
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").SetImage_normalMat();
        Debug.Log("BlurPanel");
        blur.BlurRadius.SetValue(new FloatParameter(2.0f));
        DOTween.To(() => blur.BlurRadius.value, x => blur.BlurRadius.value = x, blurMaxRadius, 1); //模糊半径最大
        yield return new WaitForSeconds(1f);
        yield break;
    }
    public IEnumerator DisBlurPanel()
    {
        UIManager.GetInstance().GetPanel<Init_BgPicPanel>("Init_BgPicPanel").SetImage_effectMat();
        Debug.Log("DisBlurPanel");
        DOTween.To(() => blur.BlurRadius.value, x => blur.BlurRadius.value = x, blurMinRadius, 1); //模糊半径=0 不模糊
        yield return new WaitForSeconds(1f);
        blur.BlurRadius.SetValue(new FloatParameter(0));
        yield break;
    }
}
