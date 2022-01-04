using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public bool triggerOnTriggerEnter = true;
    public Dialogues dialogue;
    private static bool isInDialogue = false;
    
    public void Trigger()
    {
        if (DialogueManager.Instance.dialogueCanvas.activeSelf)
        {
            DialogueManager.Instance.DisplayNextSentence();
        }
        else
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!triggerOnTriggerEnter) return;
        Trigger();
    }
}
