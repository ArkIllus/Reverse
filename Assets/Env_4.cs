using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Env_4 : Env_Manager
{
    public Rigidbody2D rig;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
        Calculate();
        Calculate1();
    }
    private void Calculate()
    {
        if (_colUp_other || _colRight_other || _colLeft_other || _colDown_other)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.player.Die();
        }
    }
    private void Calculate1()
    {
        rig.gravityScale = !GameManager.Instance.isReverse ? Mathf.Abs(rig.gravityScale) : Mathf.Abs(rig.gravityScale) * -1;
    }
}
