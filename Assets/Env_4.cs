using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Env_4 : MonoBehaviour
{
    public Rigidbody2D rig;

    //public GameManager manager;
    
    bool _active;
    private void Awake()
    {
        Invoke(nameof(Activate), 0.5f);
        rig = GetComponent<Rigidbody2D>();
        //box = GetComponent<BoxCollider2D>();

    }
    void Activate() => _active = true;
    private void Update()
    {
      
       
        Calculate1();
    }
    
    private void Calculate1()
    {
        rig.gravityScale = !GameManager.Instance.isReverse ? Mathf.Abs(rig.gravityScale) : Mathf.Abs(rig.gravityScale) * -1;
    }
}