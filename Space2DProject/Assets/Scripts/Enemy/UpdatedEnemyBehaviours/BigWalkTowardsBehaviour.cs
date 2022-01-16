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
            
            animator.SetBool("isWalking", true);
            
            Vector2 orientation = new Vector2(player.position.x - transform.GetChild(0).position.x,
                player.position.y - transform.GetChild(0).position.y).normalized;
            
            animator.SetFloat("Horizontal",orientation.x);
            animator.SetFloat("Vertical",orientation.y);
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
        isPerformingAction = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        
        // changer tout les animator. pour coller avec l'animator
        //animator.enabled = true;
        //animator.Rebind();
        //animator.Update(0f);
        // yield return new WaitForSeconds(0.833f);
        ondeDeChoc.transform.position = enemyTransform.position;
        ondeDeChoc.SetActive(true);
        // animator.enabled = false;
        yield return new WaitForSeconds(1.2f);
        isPerformingAction = false;
        ondeDeChoc.SetActive(false);
        animator.SetBool("isAttacking", false);
    }
    
    public override void Die(bool destroy = false)
    {
        currentState = State.Dead;
        agent.velocity = Vector3.zero;
        if(agent.isOnNavMesh) agent.isStopped = true;
        StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        animator.Play("CreepDeath");
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);
        base.Die();
    }
    
    public override void Stun(float duration = 1)
    {
        animator.SetBool("Damage", true);
        base.Stun(duration);
    }
}