using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    private TextMeshProUGUI speaker;
    private TextMeshProUGUI dialogueText;

    private Coroutine runningCoroutine;

    private Queue<string> sentences = new Queue<string>();

    [SerializeField] private bool instantDisplay = false;
    
    #region Singleton
    public static DialogueManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    void Start()
    {
        speaker = dialogueCanvas.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        dialogueText = dialogueCanvas.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void StartDialogue(Dialogues dialogue)
    {
        dialogueCanvas.SetActive(true);
        
        speaker.text = dialogue.characterName;
        
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
        
        
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = sentences.Dequeue();

        if (instantDisplay)
        {
            dialogueText.text = sentence;
            return;
        }
        
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        
        runningCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialogueCanvas.SetActive(false);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DisplayNextSentence();
        }
    }
}
