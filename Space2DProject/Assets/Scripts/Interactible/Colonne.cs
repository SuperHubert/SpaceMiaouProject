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

    private void Start()
    {
        if(columnDialogues.Count == 0) columnDialogues = columnDialoguesEditor;
        am = AudioManager.Instance;
        animator = gameObject.GetComponent<Animator>();
        baseLayer = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 10) return;
        if(!gameObject.GetComponent<CanGoBehind>().enabled) return;
        StartCoroutine(Explode());
    }

    public void OnInteraction()
    {
        if (!LoadingLevelData.columnDialogue) return;
        DialogueManager.Instance.StartMultipleDialogues(columnDialogues);
        LoadingLevelData.columnDialogue = false;
        StartCoroutine(Explode());
    }
    
    private IEnumerator Explode()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CanGoBehind>().enabled = false;
        animator.SetTrigger("Trigger");
        am.Play(35);
        Debug.Log("sound");
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = baseLayer;

    }
}
