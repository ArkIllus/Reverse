using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Env_Water : MonoBehaviour
{
    public PlayerController _player;
    private BoxCollider2D box;
    void Start()
    {
       box = GetComponent<BoxCollider2D>();
    
    }

    // Update is called once per frame
    void Update()
    {
        Calculate();
        //Calculate1();
    }
    
    private void Calculate()
    {
        if (_player.enchant_cold)
        {
            box.enabled = true;
            
        }
        else
        {
            box.enabled = false;
        }
    }
}
