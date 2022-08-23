using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
public class Env_cold : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private bool LeftUp_in;
    [SerializeField] private bool LeftDown_in;
    [SerializeField] private bool RightUp_in;
    [SerializeField] private bool RightDown_in;
    private bool isIn;
    private bool isIn_last;
    private BoxCollider2D box;
    private Bounds bounds;
    private float enchat_Time;
    private int enchat_Speed;

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        bounds = box.bounds;
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
        bounds = box.bounds;
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
            enchat_Speed = _player.enchant_Value>0? 2:1; 

            enchat_Time += Time.deltaTime;
            if (enchat_Time >= 0.1f)
            {
                _player.enchant_Value -= enchat_Speed;
                _player.enchant_Value = Mathf.Clamp(_player.enchant_Value, -100,100);
                _player.is_Enchant = true;
                enchat_Time = 0;
            }
        }
        if (!isIn && isIn_last)
        {
            enchat_Time = 0;
            _player.is_Enchant = false;
        }
        isIn_last = isIn;
    }
}
