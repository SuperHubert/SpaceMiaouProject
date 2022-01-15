using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWalkTowardsBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject ondeDeChoc;

    private void Update()
    {
        if(currentState != State.Awake) return;
        
        
        if (actionCd > 0)
        {
            actionCd--;
        }
        else
        {
            actionTrigger.SetActive(hasAction);
        }
        
        
        if (currentState == State.Awake && !isPerformingAction)
        {
            if(agent.isOnNavMesh) agent.SetDestination(player.position);
        }
    }

    protected override void Action()
    {
        base.Action();
        
        StartCoroutine(Attack());
    }

    
    private IEnumerator Attack()
    {
        if(agent.isOnNavMesh) agent.SetDestination(enemyTransform.position);
        yield return new WaitUntil(() => !ondeDeChoc.activeSelf);
        //leve la tete et la frappe sur le sol
        
        // changer tout les animator. pour coller avec l'animator
        animator.enabled = true;
        animator.Rebind();
        animator.Update(0f);
        yield return new WaitForSeconds(0.833f);
        ondeDeChoc.transform.position = enemyTransform.position;
        ondeDeChoc.SetActive(true);
        isPerformingAction = false;
        animator.enabled = false;
        yield return new WaitForSeconds(2f);
        ondeDeChoc.SetActive(false);
    }
    
}