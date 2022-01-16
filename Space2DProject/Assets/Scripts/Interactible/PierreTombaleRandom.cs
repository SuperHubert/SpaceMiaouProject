using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PierreTombaleRandom : MonoBehaviour,IInteractible
{
    public List<Dialogues> dialogues;
    private static List<Dialogues> usableDialogues = new List<Dialogues>();
    
    void Start()
    {
        if (usableDialogues.Count != 0) return;
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
        if (usableDialogues.Count == 0) RefillDialogues();
        var dialogue = usableDialogues[Random.Range(0, usableDialogues.Count)];
        usableDialogues.Remove(dialogue);
        gameObject.GetComponent<Collider2D>().enabled = false;
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
