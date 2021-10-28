using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    private bool isAttacking;

    void Start()	
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        if (!isPlayerInRange() && !isAttacking)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            Attack();
        }
    }

    bool isPlayerInRange()
    {
        return false;
    }

    void Attack()
    {
        //recule en regardant le joueur puis dash en avant
        isAttacking = false;
    }
    
}
