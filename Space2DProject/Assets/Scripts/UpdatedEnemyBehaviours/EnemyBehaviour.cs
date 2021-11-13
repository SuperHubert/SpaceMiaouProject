using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected enum State {Asleep, Awake, Dead};

    public bool isPerformingAction = false;

    [SerializeField] protected State currentState;

    [SerializeField] private bool respawn = true;
    
    protected int actionCdMax;
    protected int actionCd;
    
    private NavMeshAgent agent;
    private EnemyHealth health;
    
    private Transform enemy;
    private GameObject wakeUpTrigger;
    private GameObject sleepTrigger;
    private GameObject respawnTrigger;
    private GameObject trigger;
    

    protected void InitVariables()
    {
        agent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(transform.position);

        enemy = transform.GetChild(0);
        
        health = enemy.gameObject.GetComponent<EnemyHealth>();
        
        (wakeUpTrigger = enemy.GetChild(0).GetChild(0).gameObject).SetActive(true);
        (sleepTrigger = enemy.GetChild(0).GetChild(1).gameObject).SetActive(false);
        ( trigger= enemy.GetChild(0).GetChild(2).gameObject).SetActive(false);
        (respawnTrigger = transform.GetChild(1).gameObject).SetActive(false);
    }
    
    public virtual void WakeUp()
    {
        wakeUpTrigger.SetActive(false);
        sleepTrigger.SetActive(true);
        
        currentState = State.Awake;
    }

    public virtual void Sleep()
    {
        sleepTrigger.SetActive(false);
        wakeUpTrigger.SetActive(true);
        
        currentState = State.Asleep;
        isPerformingAction = false;
        
    }

    public virtual void Die()
    {
        wakeUpTrigger.SetActive(false);
        sleepTrigger.SetActive(false);
        respawnTrigger.SetActive(true);
        
        if (respawn && (LevelManager.Instance.Player().transform.position - respawnTrigger.transform.position).magnitude * 4 > respawnTrigger.transform.localScale.x)
        {
            Respawn();
            return;
        }
        
        enemy.gameObject.SetActive(false);
        
        currentState = State.Dead;
        isPerformingAction = false;
        
    }

    public virtual void Respawn()
    {
        if (!respawn) return;
        
        respawnTrigger.SetActive(false);

        enemy.position = respawnTrigger.transform.position;
        
        enemy.gameObject.SetActive(true);
        
        health.InitEnemy();
        
        currentState = State.Asleep;
        
    }

    public void ExecuteAction()
    {
        if (actionCd == 0)
        {
            Action();
        }
    }

    public virtual void Action()
    {
        Debug.Log("Action");
    }

}
