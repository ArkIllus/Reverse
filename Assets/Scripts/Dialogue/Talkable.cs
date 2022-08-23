using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Talk
{
    normal, isGainReverse, isIceGuide, isEnd
}

[RequireComponent(typeof(Collider2D))]
public class Talkable : MonoBehaviour
{
    [SerializeField] private bool isEntered;
    public Sprite characterSprite;
    public string characterName;
    [TextArea]
    [Header("对话内容")]
    public string[] lines;

    [Header("对话提示")]
    public GameObject tipUI_Prefab;
    [Header("调试用：永远可见")]
    public bool alwaysVisible;
    [Header("对话提示位置")]
    public Transform tipPoint;
    private GameObject tipUI;
    private Transform cam;

    [Header("对话是否完成")]
    public bool isCompleted;

    [Header("对话的特殊功能")]
    public E_Talk e_talk;

    //自动对话功能
    //public bool auto;

    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                //生成tipUI
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
        if (DialogueManager.Instance.dialogueBox.activeInHierarchy)
            return;

        //if (auto)
        //{
        //TODO
        //}

        if (DialogueManager.Instance.justEnd && isEntered)
        {
            //对话的特殊功能(对话结束时触发)
            switch (e_talk)
            {
                case E_Talk.normal:
                    break;
                case E_Talk.isGainReverse:
                    GameManager_global.GetInstance().gameData_SO.gainReverse = true;
                    GameManager.Instance.player.gainReverse = true;
                    break;
                case E_Talk.isIceGuide:
                    GameManager_global.GetInstance().gameData_SO.ach_1_Firstmeet.UpdateMe(1);
                    break;
                case E_Talk.isEnd:
                    GameManager.Instance.player.EndOfGame();
                    break;
                default:
                    break;
            }
        }

        if (isEntered && (Input.GetKeyDown(KeyCode.V) || Input.GetMouseButtonDown(0)))
        {
            //对话的特殊功能(对话开始时触发)
            //switch (e_talk)
            //{
            //    case E_Talk.normal:
            //        break;
            //    case E_Talk.isGainReverse:
            //        GameManager_global.GetInstance().gameData_SO.gainReverse = true;
            //        GameManager.Instance.player.gainReverse = true;
            //        break;
            //    case E_Talk.isIceGuide:
            //        GameManager_global.GetInstance().gameData_SO.ach_1_Firstmeet.UpdateMe(1);
            //        break;
            //    case E_Talk.isEnd:

            //    default:
            //        break;
            //}
            if (DialogueManager.Instance.justEnd) //避免2次Input引起的问题
            {
                Debug.Log(333);
                DialogueManager.Instance.justEnd = false;
                return;
            }
            Debug.Log(444);
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
