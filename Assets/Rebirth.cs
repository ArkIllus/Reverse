using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
public class Rebirth : MonoBehaviour
{
    private PlayerController _player;
    private Bounds bounds;
    private BoxCollider2D box;
    public bool isReverse;
    [SerializeField] private bool LeftUp_in;
    [SerializeField] private bool LeftDown_in;
    [SerializeField] private bool RightUp_in;
    [SerializeField] private bool RightDown_in;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.player;
        box = GetComponent<BoxCollider2D>();
        bounds = box.bounds;
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
            _player.rebirth = transform;

            //保存玩家重生点
            Debug.Log("保存玩家重生点");
            GameManager_global.GetInstance().gameData_SO.rebirth_pos = _player.rebirth.position;
            GameManager_global.GetInstance().gameData_SO.rebirth_Reverse = isReverse;
        }
    }
}
