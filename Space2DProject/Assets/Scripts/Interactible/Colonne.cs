using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonne : MonoBehaviour,IInteractible
{
    private Animator animator;
    private int baseLayer;
    public List<Dialogues> columnDialoguesEditor;
    private static List<Dialogues> columnDialogues = new List<Dialogues>();
    private AudioManager am;
    private CombatManager cm;
    private bool triggered = false;

    private void Start()
    {
        if(columnDialogues.Count == 0) columnDialogues = columnDialoguesEditor;
        animator = gameObject.GetComponent<Animator>();
        baseLayer = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        am = AudioManager.Instance;
        cm = CombatManager.Instance;
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 10) return;
        StartCoroutine(Explode());
    }

    public void OnInteraction()
    {
        if (LoadingLevelData.columnDialogue && cm.IsEmpty())
        {
            DialogueManager.Instance.StartMultipleDialogues(columnDialogues);
            LoadingLevelData.columnDialogue = false;
        }
        else
        {
            StartCoroutine(Explode());
        }
    }
    
    private IEnumerator Explode()
    {
        if (triggered) yield break;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CanGoBehind>().enabled = false;
        animator.SetTrigger("Trigger");
        am.Play(35);
        triggered = true;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = baseLayer;

    }
}
