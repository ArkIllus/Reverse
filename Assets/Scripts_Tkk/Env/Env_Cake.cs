using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Env_Cake : MonoBehaviour
{
    public bool isOn;
    private PlayerController player;
    public E_cake e_Cake;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (isOn)
        {
            if ((player._colDown && !GameManager.Instance.isReverse) || (player._colUp && GameManager.Instance.isReverse))
            {
                Invoke("Destroy_Cake", 0.8f);
                
                animator.SetTrigger("Die");
                isOn = false;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 10f * Time.deltaTime);
            }
                //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 5f * Time.deltaTime);
        }
    }

    private void Destroy_Cake()
    {
        //更新蛋糕成就
        GameManager_global.GetInstance().gameData_SO.UpdateCakes(e_Cake);
        MusicManager.GetInstance().PlaySound("collect", false);
        // MusicManager.GetInstance().PlaySound("collect", false);
        //MusicManager.GetInstance().PlaySound("collect", false);
        Destroy(gameObject);
    }
}
