using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Env_9_1 : MonoBehaviour
{
   // Start is called before the first frame update
    private float EndTimer = 0;
    private PlayerController _player;
    public float Time_1;
    public float Time_2;
    public Color color_on;
    public Color color_off;
    private Vector3 pos;
    private Bounds bounds;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D box;
    public BoxCollider2D testBox;

    [SerializeField] private bool LeftUp_in;
    [SerializeField] private bool LeftDown_in;
    [SerializeField] private bool RightUp_in;
    [SerializeField] private bool RightDown_in;








    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        sr.color = color_off;
        bounds = testBox.bounds;
        _player = GameManager.Instance.player;

    }

    // Update is called once per frame
    void Update()
    {

        Calculate1();


        Calculate();
    }

    private void Calculate()
    {
        EndTimer += Time.deltaTime;
        if (EndTimer > Time_1 && EndTimer < Time_1 + Time_2)
        {
             if (!RightDown_in && !LeftUp_in && !RightUp_in && !LeftDown_in){
                  box.enabled = true;
            sr.color = color_on;

             }
          
        }
        if (EndTimer > Time_1 + Time_2)
        {


          
                box.enabled = false;


                sr.color = color_off;
            

            EndTimer = 0;
        }
    }

    private void Calculate1()
    {
        bounds = testBox.bounds;
        RightDown_in = bounds.Contains(_player.RightDown);
        LeftUp_in = bounds.Contains(_player.LeftUp);
        RightUp_in = bounds.Contains(_player.RightUp);
        LeftDown_in = bounds.Contains(_player.LeftDown);
    }







}
