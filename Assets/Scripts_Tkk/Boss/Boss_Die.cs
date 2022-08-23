using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
public class Boss_Die : MonoBehaviour
{
   public PlayerController playerController;
    public float Speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.Instance.player;
        transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y,0);
    }
    private void OnEnable()
    {
        transform.position = playerController.transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position = new Vector3(playerController.transform.position.x, transform.position.y, 0);
    if (!GameManager.Instance.isReverse)
    {
        transform.Translate(Vector3.up * Time.deltaTime * Speed);
    }
    else
    {
        transform.Translate(Vector3.down * Time.deltaTime * Speed);
    }
     //transform.position = Vector2.MoveTowards(transform.position,playerController.transform.position, Speed * Time.deltaTime);
    
    }
}
