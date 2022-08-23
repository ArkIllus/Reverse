using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
public class Env_5_3 : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private bool LeftUp_in;
    [SerializeField] private bool LeftDown_in;
    [SerializeField] private bool RightUp_in;
    [SerializeField] private bool RightDown_in;
    private bool isIn;
    private bool isIn_last;
    private BoxCollider2D box;
    public BoxCollider2D box_Test;
    private Bounds bounds;
   

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        bounds = box_Test.bounds;
        _player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        Calculate1();
        Calculate();
    }
    private void Calculate1()
    {
        bounds = box_Test.bounds;
        RightDown_in = bounds.Contains(_player.RightDown);
        LeftUp_in = bounds.Contains(_player.LeftUp);
        RightUp_in = bounds.Contains(_player.RightUp);
        LeftDown_in = bounds.Contains(_player.LeftDown);

    }
    private void Calculate()
    {
        isIn = (RightDown_in || LeftDown_in || RightUp_in || LeftUp_in);

        if (RightDown_in || LeftDown_in || RightUp_in || LeftUp_in)
        {
            box.enabled = false;
        }
        if (!isIn && isIn_last)
        {
            box.enabled = true;
        }
        isIn_last = isIn;

    }
}
