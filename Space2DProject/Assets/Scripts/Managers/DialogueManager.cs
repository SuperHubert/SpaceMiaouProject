using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueCanvas;
    private GameObject nextButton;
    private TextMeshProUGUI speaker;
    private TextMeshProUGUI dialogueText;
    private Image portraitRender;
    private int audioSourceIndex = 0;

    private Coroutine typingCoroutine;
    
    private Queue<string> sentences = new Queue<string>();

    [SerializeField] private bool instantDisplay = false;
    [SerializeField] private float timeBetweenLetters = 0.005f;
    [SerializeField] private float timeBetweenBlinks = 0.5f;
    private AudioManager am;
    private bool isDoneTyping = true;
    private Coroutine soundCoroutine;
    private Coroutine nextBlinkRoutine;
    
    #region Singleton Don't Destroy On Load
    public static DialogueManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }
    #endregion
    
    void Start()
    {
        speaker = dialogueCanvas.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        dialogueText = dialogueCanvas.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();
        portraitRender = dialogueCanvas.transform.GetChild(0).GetChild(2).GetChild(0).gameObject
            .GetComponent<Image>();
        am = AudioManager.Instance;
        nextButton = dialogueCanvas.transform.GetChild(0).GetChild(5).gameObject;
    }

    public void StartDialogue(Dialogues dialogue)
    {
        dialogueCanvas.SetActive(true);
        
        speaker.text = dialogue.characterName;
        portraitRender.sprite = dialogue.characterImage;
        audioSourceIndex = dialogue.audioSourceIndex;

        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
        
        
    }

    public void DisplayNextSentence()
    {
        if (!dialogueCanvas.activeSelf)
        {
            dialogueCanvas.SetActive(true);
        }
        
        if(nextBlinkRoutine != null) StopCoroutine(nextBlinkRoutine);

        if (isDoneTyping)
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
        
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        
            if (soundCoroutine != null)
            {
                StopCoroutine(soundCoroutine);
            }
        
        
        
            typingCoroutine = StartCoroutine(TypeSentence(sentence));
            soundCoroutine = StartCoroutine(PlayTypingSound());
        }
        else
        {
            Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        }
        
        
    }
    
    private IEnumerator PlayTypingSound()
    {
        do
        {
            am.Play(audioSourceIndex);
            yield return new WaitForSeconds(0.2f);
        } while (!isDoneTyping);
        
    }
    
    private IEnumerator TypeSentence(string sentence)
    {
        if(nextBlinkRoutine != null) StopCoroutine(nextBlinkRoutine);
        nextButton.SetActive(false);
        dialogueText.text = "";
        isDoneTyping = false;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);;
        }
        isDoneTyping = true;
        yield return new WaitForSeconds(timeBetweenBlinks);
        nextBlinkRoutine = StartCoroutine(NextBlinkRoutine());
    }

    private IEnumerator NextBlinkRoutine()
    {
        while (isDoneTyping)
        {
            nextButton.SetActive(true);
            yield return new WaitForSeconds(timeBetweenBlinks);
            nextButton.SetActive(false);
            yield return new WaitForSeconds(timeBetweenBlinks);
        }
    }

    public void EndDialogue()
    {
        dialogueCanvas.SetActive(false);
    }

    public bool ToggleInstantTyping()
    {
        instantDisplay = !instantDisplay;
        return instantDisplay;
    }
}
