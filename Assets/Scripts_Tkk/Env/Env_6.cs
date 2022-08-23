using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_6 : MonoBehaviour
{
    public Transform[] pos;
    public GameObject MovingGround;
    public float Speed;
    private int i = 0;
    public float Stay_Time =0.6f;
    private float End_Time;
    //public GameObject rt;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
    Caculate();
       
    }

    private void Caculate(){
         MovingGround.transform.position = Vector2.MoveTowards(MovingGround.transform.position, pos[i].position, Speed * Time.deltaTime);
        if (Vector2.Distance(MovingGround.transform.position, pos[i].position) < 0.1f)
        {
            End_Time += Time.deltaTime;
            if(End_Time>Stay_Time){
                   i = i > 0 ? 0 : 1;
                   End_Time = 0;

            }

         
        }


    }
}
