using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [SerializeField] private bool isEntered;
    public Sprite characterSprite;
    public string characterName;
    [TextArea]
    [Header("�Ի�����")]
    public string[] lines;

    [Header("�Ի���ʾ")]
    public GameObject tipUI_Prefab;
    [Header("�����ã���Զ�ɼ�")]
    public bool alwaysVisible;
    [Header("�Ի���ʾλ��")]
    public Transform tipPoint;
    private GameObject tipUI;
    private Transform cam;

    [Header("�Ի��Ƿ����")]
    public bool isCompleted; 

    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                //����tipUI
                tipUI = Instantiate(tipUI_Prefab, canvas.transform);
                tipUI.SetActive(alwaysVisible);
            }
        }
    }

    private void OnDisable()
    {
        Destroy(tipUI.gameObject);    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEntered = true;
            tipUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEntered = false;
            tipUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isEntered && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
        {
            tipUI.SetActive(false);
            DialogueManager.Instance.ShowDialogue(this, lines, characterSprite, characterName);
        }
    }

    private void LateUpdate()
    {
        if (tipUI.activeInHierarchy)
        {
            tipUI.transform.position = tipPoint.position;
            tipUI.transform.forward = -cam.forward;
        }
    }
}
