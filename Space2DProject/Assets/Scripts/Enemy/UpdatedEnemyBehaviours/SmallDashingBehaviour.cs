using System.Collections;
using UnityEngine;

public class SmallDashingBehaviour : EnemyBehaviour
{
    [SerializeField] private Animator animator;
    
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
            agent.stoppingDistance = 0f;
            agent.acceleration = 8;
            agent.speed = 3.5f;
            actionTrigger.SetActive(true);
        }
        
        if (currentState == State.Awake && !isPerformingAction)
        {
            agent.SetDestination(player.position);
            
            //animator direction
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", true);

            Vector2 orientation = new Vector2(player.position.x - transform.GetChild(0).position.x,
                player.position.y - transform.GetChild(0).position.y).normalized;
            
            animator.SetFloat("Horizontal",orientation.x);
            animator.SetFloat("Vertical",orientation.y);

            //look left or right
            //animator.SetInteger("Direction", player.position.x - transform.position.x > 0 ? 2 : 4);
        }
    }

    protected override void Action()
    {
        base.Action();
        StartCoroutine(DashAttack());
    }

    private IEnumerator DashAttack()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        
        agent.velocity = Vector3.zero;
        agent.acceleration = 100;
        agent.speed = 20;
        agent.stoppingDistance = 1f;

        var enemyTransformPosition = enemyTransform.position;
        var playerPos = player.position;
        var target = enemyTransformPosition + (playerPos - enemyTransformPosition).normalized * 5;
        
        Debug.DrawRay(enemyTransformPosition, (playerPos - enemyTransformPosition).normalized * 5, Color.green, 4, false);
        
        agent.SetDestination(enemyTransformPosition + (playerPos - enemyTransformPosition).normalized * -1);
        
        yield return new WaitForSeconds(0.1f);

        agent.SetDestination(target);
    }
}