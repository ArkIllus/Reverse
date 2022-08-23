using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
public class Env_Boss_Re : MonoBehaviour
{
    [SerializeField] private bool LeftUp_in;
    [SerializeField] private bool LeftDown_in;
    [SerializeField] private bool RightUp_in;
    [SerializeField] private bool RightDown_in;
    [SerializeField] private BoxCollider2D box;
    private PlayerController _player;
    public GameObject Boss;
    public bool isOn;
    public Bounds bounds;
    // Start is called before the first frame update
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
          
        if (RightDown_in || LeftDown_in || RightUp_in || LeftUp_in)
        {
            
            if (!isOn)
            {
                Boss.SetActive(true);
                GameManager.Instance.player.Boss = Boss;
                GameManager.Instance.player.Boss_Positon();
               isOn = true;
            }
           // Debug.Log("pp");
        }
    }
}
