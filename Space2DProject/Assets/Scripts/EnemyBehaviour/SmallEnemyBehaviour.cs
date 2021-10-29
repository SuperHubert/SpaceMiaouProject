using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private Vector3 target;
    private CircleCollider2D col;
    private NavMeshAgent agent;
    private bool isAttacking = false;
    private bool isBacking = false;
    private bool isDashing = false;

    void Start()	
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        col = GetComponent<CircleCollider2D>();
    }
    
    void Update()
    {
        
        if (isAttacking)
        {
            if (IsStopped() && isBacking)
            {
                AttackMoveForward();
            }

            if (IsStopped() && isDashing)
            {
                ResetVariables();
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
        
    }

    bool IsStopped()
    {
        if (agent.pathPending) return false;
        if (!(agent.remainingDistance <= agent.stoppingDistance)) return false;
        return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
    }

    void ResetVariables()
    {
        isDashing = false;
        isAttacking = false;
        col.enabled = true;
        agent.acceleration = 8;
        agent.speed = 3.5f;
    }
    
    void AttackMoveBack()
    {
        isBacking = false;
        isBacking = true;
        target = targetTransform.position + ((targetTransform.position - transform.position).normalized * 3);
        agent.SetDestination(transform.position + (transform.position - targetTransform.position).normalized);
    }

    void AttackMoveForward()
    {
        isDashing = true;
        agent.acceleration = 100;
        agent.speed = 20;
        agent.SetDestination(target);
    }

    void MoveTowardsPlayer()
    {
        agent.SetDestination(targetTransform.position);
    }

    public void StartAttack()
    {
        if (isAttacking) return;
        isAttacking = true;
        AttackMoveBack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //takeDamage
        Debug.Log("Ouch");
    }
}
