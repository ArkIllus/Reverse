using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject dialogueBox; //Display or Hide
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image characterImage;

    public float textWaitTime = 0.02f;
    [TextArea(1, 3)]
    public string[] dialogueLines = new string[3];
    [SerializeField] private Talkable currentTalkable;
    [SerializeField] private int currntLine;
    [SerializeField] private bool isScrolling;
    public bool justEnd; //���һ�仰�ս���

    protected override void Awake()
    {
        //if (instance != null)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    instance = this as DialogueManager;
        //    DontDestroyOnLoad(this.gameObject); //optional
        //}
        base.Awake();

        if (dialogueLines != null)
            dialogueText.text = dialogueLines[currntLine];
    }

    private void Update()
    {
        if (!dialogueBox.activeInHierarchy || isScrolling)
                return;

        if (Input.GetKeyDown(KeyCode.V) || Input.GetMouseButtonUp(0))
        //if (Input.GetMouseButtonUp(0))
        {
            currntLine++;

            if (currntLine < dialogueLines.Length)
            {
                Debug.Log("000");
                //dialogueText.text = dialogueLines[currntLine];
                StartCoroutine(ScrollingText());
            }
            else
            {
                justEnd = true;
                Debug.Log("111");
                dialogueBox.SetActive(false); // hide box
                GameManager.Instance.player.isTalking = false;
                if (currentTalkable != null)
                    currentTalkable.isCompleted = true;//�Ի������
            }
        }
    }

    public void ShowDialogue(Talkable talkable, string[] newLines, Sprite characterSprite, string characterName)
    {
        if (dialogueBox.activeInHierarchy)
            return;

        currentTalkable = talkable;
        if (talkable.isCompleted) //�Ի�����ɣ��򲥷����һ�仰
        {
            GameManager.Instance.player.isTalking = true;
            dialogueLines = newLines;
            currntLine = newLines.Length - 1;
            //dialogueText.text = dialogueLines[currntLine];
            StartCoroutine(ScrollingText());
            characterImage.sprite = characterSprite;
            nameText.text = characterName;
            dialogueBox.SetActive(true);
        }
        else //�Ի�δ��ɣ��򲥷����л����ӵ�һ�俪ʼ
        {
            GameManager.Instance.player.isTalking = true;
            dialogueLines = newLines;
            currntLine = 0;
            //dialogueText.text = dialogueLines[currntLine];
            StartCoroutine(ScrollingText());
            characterImage.sprite = characterSprite;
            nameText.text = characterName;
            dialogueBox.SetActive(true);
        }
    }

    private IEnumerator ScrollingText()
    {
        isScrolling = true;
        dialogueText.text = "";

        foreach (char ch in dialogueLines[currntLine].ToCharArray())
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(textWaitTime);
        }
        isScrolling = false;
    }
}
