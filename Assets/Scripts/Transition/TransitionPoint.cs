using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TransitionPoint : MonoBehaviour
{
    //public enum TransitionType
    //{
    //    //ͬ/��ͬ��������
    //    SameScene, DifferentScene
    //}

    //[Header("Transition Info")]
    //public string sceneName;
    //public TransitionType transitionType;
    //public TransitionDestination.DestinationTag destinationTag;

    ////�ܷ���
    //private bool canTrans;

    //private void Update()
    //{
    //    //e������
    //    if (Input.GetKeyDown(KeyCode.E) && canTrans)
    //    {
    //        //SceneController����
    //        SceneController.Instance.TransitionToDest(this);
    //    }
    //}

    ////OnTriggerEnterֻ�н����һ�̲ŵ��ã�Ҳ����
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canTrans = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canTrans = false;
    //    }
    //}

    [Header("Transition Info")]
    public string sceneName;
    public int currentLevelIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter TransitionPoint");
        if (other.CompareTag("Player"))
        {
            Debug.Log("To Next Level");

            GameManager_global.GetInstance().gameData_SO.lastLevel = currentLevelIndex + 1; //lastLevel from 0, currentLevelIndex from 1
            GameManager_global.GetInstance().gameData_SO.UpdateLevelRecords();

            //string num = System.Text.RegularExpressions.Regex.Replace(sceneName, @"[^0-9]+", "");
            //GameManager_global.GetInstance().gameData_SO.levelRecords[int.Parse(num)].isPass = true;
            Debug.Log("000");
            GameManager_global.GetInstance().gameData_SO.levelRecords[currentLevelIndex].isPass = true;
            Debug.Log("111");
            SceneMgr.GetInstance().LoadSceneAsync(sceneName);
        }
    }
}
