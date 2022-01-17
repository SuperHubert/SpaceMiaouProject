using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWhileDialogue : MonoBehaviour
{
    private bool stop = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = DialogueManager.Instance;
    }
    
    void Update()
    {
        if (stop && dialogueManager.dialogueCanvas.activeSelf)
        {
            InputManager.canInput = false;
        }
        else
        {
            InputManager.canInput = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        stop = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        stop = false;
    }
}
