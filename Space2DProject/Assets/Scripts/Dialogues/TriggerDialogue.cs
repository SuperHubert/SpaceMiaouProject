using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public Dialogues dialogue;

    public void Trigger()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Trigger();
    }
}
