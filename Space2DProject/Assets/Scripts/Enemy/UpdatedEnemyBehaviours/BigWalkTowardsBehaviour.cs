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
            agent.SetDestination(player.position);
        }
    }
    
    public override void Action()
    {
        base.Action();
        //StartCoroutine(Attack());
    }

    /*
    private IEnumerator Attack()
    {
        
    }
    */
}