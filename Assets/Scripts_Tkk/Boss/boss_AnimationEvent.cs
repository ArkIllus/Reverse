using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_AnimationEvent : MonoBehaviour
{
    private Animator animator;
    public GameObject hit1;
    public GameObject laser;
    public GameObject laser_Aim;
    public GameObject Collider;
    public GameObject Collider2;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void Attack_start()
    {
        
        hit1.SetActive(true);
        laser_Aim.SetActive(true);
        

    }
    public void Attack_start_End()
    {
     
        hit1.SetActive(false);
        laser_Aim.SetActive(false);
        Collider.SetActive(true);
        Collider2.SetActive(true);
    }

    public void Attack_loop()
    {
        laser.SetActive(true);
    }

    public void Attack_loop_End()
    {
        laser.SetActive(false);
    }

}
