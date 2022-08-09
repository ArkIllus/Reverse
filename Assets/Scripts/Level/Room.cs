using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    private CinemachineConfiner confiner;
    //TODO:优化UnityEvent
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    private void Awake()
    {
        confiner = GetComponentInChildren<CinemachineConfiner>();

        confiner.gameObject.SetActive(false);

        OnTriggerEnterEvent.AddListener(() => { confiner.gameObject.SetActive(true); });
        OnTriggerExitEvent.AddListener(() => { confiner.gameObject.SetActive(false); });
    }

    //需要具有rigidbody和collider才能触发OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("this.name = " + this.name + ", collision.tag = " + collision.tag);
        if (collision.tag == "Player")
        {
            OnTriggerEnterEvent.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("this.name = " + this.name + ", collision.tag = " + collision.tag);
        if (collision.tag == "Player")
        {
            OnTriggerExitEvent.Invoke();
        }
    }
}
