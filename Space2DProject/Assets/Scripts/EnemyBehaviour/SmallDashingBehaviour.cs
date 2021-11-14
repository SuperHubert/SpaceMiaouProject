using System.Collections;
using System.Collections.Generic;
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
        else
        {
            isPerformingAction = false;
            agent.acceleration = 8;
            agent.speed = 3.5f;
        }
        
        if (currentState == State.Awake && !isPerformingAction)
        {
            agent.SetDestination(player.position);
        }
    }
    
    public override void Action()
    {
        base.Action();
        StartCoroutine(DashAttack());
    }

    private IEnumerator DashAttack()
    {
        agent.velocity = Vector3.zero;
        agent.acceleration = 100;
        agent.speed = 20;
        
        Vector3 target = enemy.position + (player.position - enemy.position).normalized * 3;
        
        agent.SetDestination(enemy.position + (player.position - enemy.position).normalized * -1);
        
        yield return new WaitForSeconds(0.1f);

        agent.SetDestination(target);
    }
}