using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    //public enum TransitionType
    //{
    //    //同/不同场景传送
    //    SameScene, DifferentScene
    //}

    //[Header("Transition Info")]
    //public string sceneName;
    //public TransitionType transitionType;
    //public TransitionDestination.DestinationTag destinationTag;

    ////能否传送
    //private bool canTrans;

    //private void Update()
    //{
    //    //e键传送
    //    if (Input.GetKeyDown(KeyCode.E) && canTrans)
    //    {
    //        //SceneController传送
    //        SceneController.Instance.TransitionToDest(this);
    //    }
    //}

    ////OnTriggerEnter只有进入的一刻才调用，也可以
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
