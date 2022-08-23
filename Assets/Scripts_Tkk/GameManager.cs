using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using UnityEngine.Rendering;

public class GameManager : EntitySingleton<GameManager>
{
    // �Ƿ�ת
    public bool isReverse;

    // ���������
    //public Transform playerRebirthPlace;

    //�������
    public Volume volume;

    [HideInInspector]
    public PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        //����120֡
        //Application.targetFrameRate = 120;
        //����300֡
        //Application.targetFrameRate = 300;
        //���֡��
        //Application.targetFrameRate = -1;
        Time.timeScale = 1f;
        if (player == null)
            player = FindObjectOfType<PlayerController>();
        if (volume == null)
            volume = FindObjectOfType<Volume>();
    }
}
