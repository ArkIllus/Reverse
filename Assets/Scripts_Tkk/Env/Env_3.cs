using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TarodevController;

public class Env_3 : Env_Manager
{
    public Rigidbody2D rig;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
        Calculate1();
        Calculate();
    }

    private void Calculate()
    {
        rig.gravityScale = !GameManager.Instance.isReverse ? Mathf.Abs(rig.gravityScale) : Mathf.Abs(rig.gravityScale) * -1;
    }


    private void Calculate1()
    {
        if ((GameManager.Instance.player._colUp && _colUp_other) || (GameManager.Instance.player._colDown && _colDown_other))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.player.Die();
        }
    }
}
