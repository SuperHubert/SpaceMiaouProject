using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWalkTowardsBehaviour : EnemyBehaviour
{
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
        isPerformingAction = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        
        yield return new WaitForSeconds(0.5f);

        var ondeDeChoc = ObjectPooler.Instance.SpawnFromPool("Explosion", player.position, Quaternion.identity);
        yield return new WaitForSeconds(1.8f);
        isPerformingAction = false;
        ondeDeChoc.SetActive(false);
        animator.SetBool("isAttacking", false);
    }
    
    public override void Die(bool destroy = false)
    {
        StopAllCoroutines();
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