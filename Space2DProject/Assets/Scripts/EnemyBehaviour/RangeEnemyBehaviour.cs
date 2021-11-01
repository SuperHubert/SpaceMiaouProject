using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyBehaviour : MonoBehaviour, IEnemy
{
    [SerializeField] private Transform targetTransform;
    private Vector3 targetPosition;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private bool isActive = false;
    private bool isAttacking = true;
    private bool isRunning = false;

    [SerializeField] private int cooldown = 4;
    public int coolDownMax = 50;
    [SerializeField] float bulletSpeed = 10f;
    private ObjectPooler bulletPool;
    private GameObject bullet;
    

    void Start()
    {
        targetTransform = MoneyManager.Instance.playerTransform;
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        if (!isActive) return;
        
        LookAt(targetTransform);
        
        if (isAttacking)
        {
            if (cooldown > 0) 
            { 
                cooldown--;
            }
            else
            {
                bullet = ObjectPooler.Instance.SpawnFromPool("Enemy Bullets", transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = (targetTransform.position - transform.position) * bulletSpeed;
                bullet.layer = 10;
                cooldown = coolDownMax;
            }
        }
        else
        {
            if (IsStopped() && isRunning)
            {
                isAttacking = true;
                isRunning = false;
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchState();
        }
    }

    void LookAt(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void StartRunningAway()
    {
        isAttacking = false;
        isRunning = true;
        agent.SetDestination(transform.position + (transform.position - targetTransform.position).normalized * 3);
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

    public void Activate()
    {
        isActive = true;
    }
    
    public void DeActivate()
    {
        agent.SetDestination(transform.position);
        isActive = false;
    }
}
