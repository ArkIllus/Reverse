using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter TransitionPoint");
        if (other.CompareTag("Player"))
        {
            Debug.Log("To Next Level");
            SceneMgr.GetInstance().LoadSceneAsync(sceneName);
        }
    }
}
