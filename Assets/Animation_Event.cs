using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Event : MonoBehaviour
{
    public Animator anim;


    private int reverseLayer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        //baseLayer = anim.GetLayerIndex("Red");
        reverseLayer = anim.GetLayerIndex("Blue");
    }

    // Update is called once per frame
    void Update()
    {
        
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
