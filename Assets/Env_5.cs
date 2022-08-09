using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_5 : Env_Manager
{
    public BoxCollider2D box;
    public GameObject gm;
    private Vector3 v1 = new Vector3(0f,0f,0f);
    private Vector3 v2 = new Vector3(180f,0f,0f);
    public override void Update()
    {
        base.Update();
        CalculateRotation();
        Calculate1();
    }

    private void Calculate1()
    {
        if (!GameManager.Instance.isReverse)
        {
            if (_colDown_other)
            {
                box.enabled = false;
            }
            if (!_colLeft_other && !_colRight_other && !_colDown_other && !_colUp_other)
            {
                box.enabled = true;
            }
        }
        else
        {
            if (_colUp_other)
            {
                box.enabled = false;
            }
            if (!_colLeft_other && !_colRight_other && !_colDown_other && !_colUp_other)
            {
                box.enabled = true;
            }
        }
     
    }


    private void CalculateRotation()
    {
        gm.transform.localEulerAngles = !GameManager.Instance.isReverse ? v1 : v2;
    }

}
