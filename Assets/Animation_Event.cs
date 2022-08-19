using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Event : MonoBehaviour
{
    public Animator anim;

    private int reverseLayer;

    void Awake()
    {
        anim = GetComponent<Animator>();

        //baseLayer = anim.GetLayerIndex("Red");
        reverseLayer = anim.GetLayerIndex("Blue");
    }

    public void Reverse_1()
    {
        Debug.Log("Reverse_1");
        if (GameManager.Instance.isReverse)
        {
            //if (anim == null) Debug.Log("anim == null");
            anim.SetLayerWeight(reverseLayer, 1f);
        }
    }

    public void Reverse_2()
    {
        Debug.Log("Reverse_2");
        if (!GameManager.Instance.isReverse)
        {
            anim.SetLayerWeight(reverseLayer, 0f);
        }
    }
}
