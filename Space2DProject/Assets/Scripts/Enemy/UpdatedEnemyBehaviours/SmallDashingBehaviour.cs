using System.Collections;
using UnityEngine;

public class SmallDashingBehaviour : EnemyBehaviour
{
    private void Update()
    {
        if(currentState != State.Awake) return;

        if (actionCd > 0)
        {
            actionCd--;
        }

        if (currentState != State.Awake || isPerformingAction) return;
        if(agent.isOnNavMesh) agent.SetDestination(player.position);
            
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", true);

        Vector2 orientation = new Vector2(player.position.x - transform.GetChild(0).position.x,
            player.position.y - transform.GetChild(0).position.y).normalized;
            
        animator.SetFloat("Horizontal",orientation.x);
        animator.SetFloat("Vertical",orientation.y);
    }

    protected override void InitVariables()
    {
        base.InitVariables();
        enemy.GetComponent<Collider2D>().enabled = true;
    }

    protected override void Action()
    {
        base.Action();
        StartCoroutine(DashAttack());
    }
    
    private IEnumerator DashAttack()
    {
        isPerformingAction = true;
        
        yield return null;
        
        agent.velocity = Vector3.zero;

        var enemyTransformPosition = enemyTransform.position;
        var playerPos = player.position;
        var backPos = enemyTransformPosition + (playerPos - enemyTransformPosition).normalized * -1;
        var dashPos = enemyTransformPosition + (playerPos - enemyTransformPosition).normalized * 5;
        
        if(agent.isOnNavMesh && enemy.activeSelf && currentState != State.Dead && !animator.GetBool("isDead")) agent.SetDestination(backPos);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => agent.velocity == Vector3.zero);
        
        am.Play(15,true);
        
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        agent.velocity = Vector3.zero;
        agent.acceleration = 100;
        agent.speed = 10;
        agent.stoppingDistance = 1.5f;
        
        if(agent.isOnNavMesh && enemy.activeSelf && currentState != State.Dead && !animator.GetBool("isDead")) agent.SetDestination(dashPos);
        
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => agent.velocity == Vector3.zero);
        
        agent.stoppingDistance = 0f;
        agent.acceleration = 8;
        agent.speed = 3.5f;

        actionCd = 0;

        isPerformingAction = false;

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    public override void Die(bool destroy = false)
    {
        enemy.GetComponent<Collider2D>().enabled = false;
        currentState = State.Dead;
        agent.stoppingDistance = 0f;
        agent.acceleration = 8;
        agent.speed = 3.5f;
        agent.velocity = Vector3.zero;
        if(agent.isOnNavMesh) agent.isStopped = true;
        StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        animator.Play("Poupi_Death");
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);
        base.Die();
    }
    
    public override void Stun(float duration = 1)
    {
        animator.SetBool("Damage", true);
        base.Stun(duration);
    }

    public override void Sleep()
    {
        base.Sleep();
        agent.stoppingDistance = 0f;
        agent.acceleration = 8;
        agent.speed = 3.5f;
    }
}