using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class GameManager : EntitySingleton<GameManager>
{
    // �Ƿ�ת
    public bool isReverse;

    // ���������
    public Transform playerRebirthPlace;

    [HideInInspector]
    public PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        //����120֡
        Application.targetFrameRate = 120;
        Time.timeScale = 1f;

        player = FindObjectOfType<PlayerController>();
    }
}
