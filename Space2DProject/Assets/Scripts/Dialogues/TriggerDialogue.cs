using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public Dialogues dialogue;

    public void Trigger()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
