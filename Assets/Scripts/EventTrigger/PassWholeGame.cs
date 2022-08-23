using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PassWholeGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("通关");

            GameManager_global.GetInstance().gameData_SO.UpdateLevelRecords();

            GameManager_global.GetInstance().gameData_SO.ach_3_Pass.UpdateMe(1);
            //TODO 剧情演出
        }
    }
}
