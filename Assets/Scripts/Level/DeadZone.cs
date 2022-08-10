using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeadZone : MonoBehaviour
{
    //TODO:优化UnityEvent
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    private void Awake()
    {
        OnTriggerEnterEvent.AddListener(() => { GameManager.Instance.player.Die(); });
    }

    //需要具有rigidbody和collider才能触发OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            OnTriggerEnterEvent.Invoke();
        }
    }
}
