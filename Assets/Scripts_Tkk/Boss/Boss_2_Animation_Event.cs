using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2_Animation_Event : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hit1;
    public Boss_2 boss;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack_start_Ice()
    {

        hit1.SetActive(true);
        //laser_Aim.SetActive(true);
        boss.isStart = true;

    }
    public void Attack_start_End_Ice()
    {

       
        //laser_Aim.SetActive(false);
        //Collider.SetActive(true);
    }
}
