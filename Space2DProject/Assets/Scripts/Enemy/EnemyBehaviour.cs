using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected enum State {Asleep, Awake, Dead};
    
    [SerializeField] protected State currentState;

    [SerializeField] protected bool respawn = true;

    [SerializeField] protected bool hasAction;
    [SerializeField] protected int actionCdMax;
    [SerializeField] protected int actionCd;
    protected bool isPerformingAction = false;

    protected NavMeshAgent agent;
    protected EnemyHealth health;
    protected Transform player;
    
    protected Transform enemy;
    protected GameObject wakeUpTrigger;
    protected GameObject sleepTrigger;
    protected GameObject respawnTrigger;
    protected GameObject actionTrigger;


    private void Start()
    {
        InitVariables();
    }

    protected virtual void InitVariables()
    {
        enemy = transform.GetChild(0);
        
        agent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(enemy.position);
        
        health = enemy.gameObject.GetComponent<EnemyHealth>();
        
        (wakeUpTrigger = enemy.GetChild(0).GetChild(0).gameObject).SetActive(true);
        (sleepTrigger = enemy.GetChild(0).GetChild(1).gameObject).SetActive(false);
        (actionTrigger= enemy.GetChild(0).GetChild(2).gameObject).SetActive(false);
        (respawnTrigger = transform.GetChild(1).gameObject).SetActive(false);

        player = LevelManager.Instance.Player().transform;

        if (!hasAction) return;
        
        actionCd = 0;
        isPerformingAction = false;
    }
    
    public virtual void WakeUp()
    {
        wakeUpTrigger.SetActive(false);
        sleepTrigger.SetActive(true);
        actionTrigger.SetActive(hasAction);
        
        currentState = State.Awake;
    }

    public virtual void Sleep()
    {
        sleepTrigger.SetActive(false);
        wakeUpTrigger.SetActive(true);
        actionTrigger.SetActive(false);

        agent.SetDestination(transform.position);
        
        currentState = State.Asleep;
        isPerformingAction = false;
        
    }

    public virtual void Die()
    {
        agent.SetDestination(transform.position);
        
        wakeUpTrigger.SetActive(false);
        sleepTrigger.SetActive(false);
        actionTrigger.SetActive(false);
        respawnTrigger.SetActive(true);
        
        isPerformingAction = false;
        
        if (respawn && (LevelManager.Instance.Player().transform.position - respawnTrigger.transform.position).magnitude * 4 > respawnTrigger.transform.localScale.x)
        {
            Respawn();
            return;
        }
        
        enemy.gameObject.SetActive(false);
        
        currentState = State.Dead;
    }

    public virtual void Respawn()
    {
        if (!respawn) return;
        
        enemy.gameObject.SetActive(true);
        enemy.position = respawnTrigger.transform.position;
        health.InitEnemy();
        
        currentState = State.Asleep;
        
    }

    public void ExecuteAction()
    {
        if (actionCd != 0 || !hasAction) return;
        isPerformingAction = true;
        actionCd = actionCdMax;
        Action();
    }

    public virtual void Action()
    {
        if (isPerformingAction) return;
        actionTrigger.SetActive(false);
    }

}