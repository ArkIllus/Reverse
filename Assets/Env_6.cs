using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_6 : MonoBehaviour
{
    public Transform[] pos;
    public GameObject MovingGround;
    public float Speed;
    private int i = 0;
    //public GameObject rt;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
   
        MovingGround.transform.position = Vector2.MoveTowards(MovingGround.transform.position, pos[i].position, Speed * Time.deltaTime);
        if (Vector2.Distance(MovingGround.transform.position, pos[i].position) < 0.1f)
        {
            i = i > 0 ? 0 : 1;
        }

    }
}
