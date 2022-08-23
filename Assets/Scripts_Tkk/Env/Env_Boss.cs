using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
public class Env_Boss : MonoBehaviour
{
   private PlayerController _player;
    [SerializeField] private bool LeftUp_in;
    [SerializeField] private bool LeftDown_in;
    [SerializeField] private bool RightUp_in;
    [SerializeField] private bool RightDown_in;
    public GameObject Boss;
    private bool isOn;
    private BoxCollider2D box;
    private Bounds bounds;

      void Start()
    {
        box = GetComponent<BoxCollider2D>();
        bounds = box.bounds;
        _player = GameManager.Instance.player;
    }
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

    private void Calculate(){
         if (RightDown_in || LeftDown_in || RightUp_in || LeftUp_in)
        {
            
            if (!isOn)
            {
                Boss.SetActive(true);
                _player.Boss = Boss;
                _player.Boss_Positon();
               isOn = true;
            }
           // Debug.Log("pp");
        }

    }
}
