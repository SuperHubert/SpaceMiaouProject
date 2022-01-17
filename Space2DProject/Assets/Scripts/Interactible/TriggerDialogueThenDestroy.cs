using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueThenDestroy : MonoBehaviour,IInteractible
{
    public bool onTrigger = false;
    public bool destroy = true;
    public List<Dialogues> dialogues;
    private int index = 0;
    private bool started = false;
    private DialogueManager dialogueManager;
    
    void Start()
    {
        dialogueManager = DialogueManager.Instance;
    }

    void Update()
    {
        if(index >= dialogues.Count) return;
        if (!started || dialogueManager.dialogueCanvas.activeSelf) return;
        dialogueManager.StartDialogue(dialogues[index]);
        index++;
    }

    private void StartDialogue()
    {
        started = true;
        dialogueManager.StartDialogue(dialogues[0]);
        index++;
        if(destroy) gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!onTrigger) return;
        StartDialogue();
    }

    public void OnInteraction()
    {
        StartDialogue();
    }
}
