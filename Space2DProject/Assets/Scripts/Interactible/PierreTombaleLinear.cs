using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreTombaleLinear : MonoBehaviour,IInteractible
{
    public List<Dialogues> dialogues;
    public static List<Dialogues> usableDialogues = new List<Dialogues>();
    public static int index;

    // Start is called before the first frame update
    void Start()
    {
        if (usableDialogues.Count != 0) return;
        index = 0;
        RefillDialogues();
    }
    
    private void RefillDialogues()
    {
        usableDialogues.Clear();
        foreach (var dialogue in dialogues)
        {
            usableDialogues.Add(dialogue);
        }
    }

    public void OnInteraction()
    {
        if (index >= usableDialogues.Count) index = 0;
        gameObject.GetComponent<Collider2D>().enabled = false;
        DialogueManager.Instance.StartDialogue(usableDialogues[index]);
        index++;
        
        
    }
}

