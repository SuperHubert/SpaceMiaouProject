using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PierreTombaleRandom : MonoBehaviour,IInteractible
{
    public List<Dialogues> dialogues;
    private static List<Dialogues> usableDialogues = new List<Dialogues>();
    [SerializeField] private Collider2D col;
    private CombatManager cm;
    
    void Start()
    {
        cm = CombatManager.Instance;
        col = gameObject.GetComponent<Collider2D>();
        if (usableDialogues.Count != 0) return;
        RefillDialogues();
    }

    private void Update()
    {
        if(col.enabled == cm.IsEmpty()) return;
        col.enabled = cm.IsEmpty();
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
