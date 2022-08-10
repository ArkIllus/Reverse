using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeadZone : MonoBehaviour
{
    //TODO:�Ż�UnityEvent
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    private void Awake()
    {
        OnTriggerEnterEvent.AddListener(() => { GameManager.Instance.player.Die(); });
    }

    //��Ҫ����rigidbody��collider���ܴ���OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            OnTriggerEnterEvent.Invoke();
        }
    }
}
