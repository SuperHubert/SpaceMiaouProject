using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonne : MonoBehaviour,IInteractible
{
    private Animator animator;
    private int baseLayer;
    public List<Dialogues> columnDialogues;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        baseLayer = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 10) return;
        StartCoroutine(Explode());
    }

    public void OnInteraction()
    {
        if (LoadingLevelData.columnDialogue) DialogueManager.Instance.StartMultipleDialogues(columnDialogues);
    }
    
    private IEnumerator Explode()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CanGoBehind>().enabled = false;
        animator.SetTrigger("Trigger");
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = baseLayer;

    }
}
