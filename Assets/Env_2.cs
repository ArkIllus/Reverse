using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Env_2 : Env_Manager
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
        Calculate();
    }

    private void Calculate()
    {
        if(_colUp_other || _colRight_other || _colLeft_other || _colDown_other)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.player.Die();
        }
    }
}
