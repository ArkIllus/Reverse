using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    private CinemachineConfiner confiner;
    private CinemachineImpulseListener listener;
    //TODO:优化UnityEvent
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;
   
    private PolygonCollider2D Poly;
    private Bounds bounds;

    public bool isLastRoom;
    public bool nextToLastRoom;
    public bool isBossStartRoom;
    public int index;

    private void Awake()
    {
        confiner = GetComponentInChildren<CinemachineConfiner>();
        listener = GetComponentInChildren<CinemachineImpulseListener>();

        confiner.gameObject.SetActive(false); 
        listener.gameObject.SetActive(false);

        OnTriggerEnterEvent.AddListener(() => { confiner.gameObject.SetActive(true); listener.gameObject.SetActive(true); });
        OnTriggerExitEvent.AddListener(() => { confiner.gameObject.SetActive(false); listener.gameObject.SetActive(false); });
        Poly = GetComponentInChildren<PolygonCollider2D>();
    }

    //需要具有rigidbody和collider才能触发OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bounds = Poly.bounds;
        Debug.Log("Enter this.name = " + this.name + ", collision.tag = " + collision.tag);
        if (collision.tag == "Player")
        {
            if (isLastRoom)
            {
                MusicManager.GetInstance().PlayBGM("End Room");
            }
            else if (index >= 1)
            {
                MusicManager.GetInstance().PlayBGM("Zero Lives OST WIP 5");
            }
            if (Level.Instance.isLastRoom && nextToLastRoom)
            {
                return;
            }
            OnTriggerEnterEvent.Invoke();
            Level.Instance.isLastRoom = isLastRoom;

            GameManager.Instance.player.enchant_Value = 0;
            GameManager.Instance.player.isSound = false;
            Invoke("Calculate", 0.3f);
            //快速消除过热/过冷屏幕效果
            FreezePanel.Instance.QuickFade();
            OverheatPanel.Instance.QuickFade();

            GameManager.Instance.player.Boss_min = bounds.min.x;
            GameManager.Instance.player.Boss_max = bounds.max.x;
            GameManager.Instance.player.Boss_End();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit this.name = " + this.name + ", collision.tag = " + collision.tag);
        if (collision.tag == "Player")
        {
            OnTriggerExitEvent.Invoke();
        }
    }
    private void Calculate()
    {
        GameManager.Instance.player.isSound = true;
    }
}
