using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 图片ietm
/// </summary>
public class CarouselItem : MonoBehaviour
{
    [SerializeField] float bezierT;
    //SpriteRenderer m_Img;
    Image m_Img;

    //封装
    public float BezierT { get => bezierT; set => bezierT = value; }

    /// <summary>
    /// 修改图片
    /// </summary>
    /// <param name="sprite"></param>
    public void SetImage(Sprite sprite)
    {
        //image为空时初始化
        if (m_Img == null)
        {
            m_Img = transform.GetComponent<Image>();
            //m_Img = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }


        //Debug.Log(m_Img); 
        //Debug.Log(m_Img.sprite);
        m_Img.sprite = sprite;
    }

}
