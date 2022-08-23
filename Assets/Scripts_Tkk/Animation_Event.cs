using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Event : MonoBehaviour
{
    public Animator anim;

    private int reverseLayer;

    void Awake()
    //void Start()
    {
        anim = GetComponent<Animator>();

        //baseLayer = anim.GetLayerIndex("Red");
        reverseLayer = anim.GetLayerIndex("Blue");
    }

    public void Reverse_1()
    {
        Debug.Log(1);

        if (GameManager.Instance.isReverse)
        {
            anim.SetLayerWeight(reverseLayer, 1f);
        }
        //anim.SetLayerWeight(reverseLayer, 1f);

    }

    public void Reverse_2()
    {
        Debug.Log(3);
        if (!GameManager.Instance.isReverse)
            anim.SetLayerWeight(reverseLayer, 0f);

    }
}
