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
            actionTrigger.SetActive(true);
        }
        
        if (currentState == State.Awake && !isPerformingAction)
        {
            agent.SetDestination(player.position);
        }
    }

    protected override void Action()
    {
        base.Action();
        StartCoroutine(DashAttack());
    }

    private IEnumerator DashAttack()
    {
        agent.velocity = Vector3.zero;
        agent.acceleration = 100;
        agent.speed = 20;

        var currentPos = enemyTransform.position;
        var target = currentPos + (player.position - currentPos).normalized * 3;
        
        agent.SetDestination(currentPos + (player.position - currentPos).normalized * -1);
        
        yield return new WaitForSeconds(0.1f);

        agent.SetDestination(target);
    }
}