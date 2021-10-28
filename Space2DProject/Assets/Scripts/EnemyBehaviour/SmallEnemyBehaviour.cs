using System;
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
        if (!isAttacking)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }
    
    IEnumerator Attack()
    {
        Debug.Log("Attacking");
        //recule en regardant le joueur puis dash en avant
        yield return new WaitForSeconds(2);
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isAttacking = true;
        StartCoroutine(Attack());
    }
}
