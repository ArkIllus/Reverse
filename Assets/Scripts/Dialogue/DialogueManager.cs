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
    public string[] dialogueLines;
    [SerializeField] private Talkable currentTalkable;
    [SerializeField] private int currntLine;
    [SerializeField] private bool isScrolling;

    protected override void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this as DialogueManager;
            DontDestroyOnLoad(this.gameObject); //optional
        }

        dialogueText.text = dialogueLines[currntLine];
    }

    private void Update()
    {
        if (!this.gameObject.activeInHierarchy || isScrolling)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            currntLine++;

            if (currntLine < dialogueLines.Length)
            {
                //dialogueText.text = dialogueLines[currntLine];
                StartCoroutine(ScrollingText());
            }
            else
            {
                dialogueBox.SetActive(false); // hide box
                GameManager.Instance.player.isTalking = false;
                if (currentTalkable != null)
                    currentTalkable.isCompleted = true;//对话已完成
            }
        }
    }

    public void ShowDialogue(Talkable talkable, string[] newLines, Sprite characterSprite, string characterName)
    {
        if (dialogueBox.activeInHierarchy)
            return;

        currentTalkable = talkable;
        if (talkable.isCompleted) //对话已完成，则播放最后一句话
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
        else //对话未完成，则播放所有话
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
