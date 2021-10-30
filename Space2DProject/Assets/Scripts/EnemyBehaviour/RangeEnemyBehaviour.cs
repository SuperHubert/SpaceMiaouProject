using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyBehaviour : MonoBehaviour, IEnemy
{
    [SerializeField] private Transform targetTransform;
    private Vector3 target;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private bool isAttacking = false;
    private bool isRunning = false;

    void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchState();
        }
    }

    void StartRunningAway()
    {
        
    }

    void SwitchState()
    {
        obstacle.enabled = !obstacle.enabled;
        agent.enabled = !agent.enabled;
    }
    
    bool IsStopped()
    {
        if (agent.pathPending) return false;
        if (!(agent.remainingDistance <= agent.stoppingDistance)) return false;
        return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
    }
    
    public void OnTriggerZoneEnter()
    {
        StartRunningAway();
    }
}
