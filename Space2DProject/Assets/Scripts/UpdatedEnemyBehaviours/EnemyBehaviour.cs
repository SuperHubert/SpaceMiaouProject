using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected enum State {Asleep, Awake, Dead};

    public bool isPerformingAction = false;

    [SerializeField] protected State currentState;

    protected int actionCdMax;
    protected int actionCd;

    public Vector3 spawnPoint;

    private GameObject wakeUpTrigger;
    private GameObject sleepTrigger;
    private GameObject respawnTrigger;
    private GameObject trigger;

    protected void InitVariables()
    {
        Debug.Log(transform.GetChild(0));
        (wakeUpTrigger = transform.GetChild(0).GetChild(0).gameObject).SetActive(true);
        (sleepTrigger = transform.GetChild(0).GetChild(1).gameObject).SetActive(false);
        (respawnTrigger = transform.GetChild(0).GetChild(2).gameObject).SetActive(false);
        (trigger = transform.parent.GetChild(1).gameObject).SetActive(false);
    }
    
    public virtual void WakeUp()
    {
        currentState = State.Awake;
        wakeUpTrigger.SetActive(false);
        sleepTrigger.SetActive(true);
    }

    public virtual void Sleep()
    {
        currentState = State.Asleep;
        isPerformingAction = false;
        wakeUpTrigger.SetActive(true);
        sleepTrigger.SetActive(false);
    }

    public virtual void Die()
    {
        currentState = State.Dead;
        isPerformingAction = false;
        wakeUpTrigger.SetActive(false);
        sleepTrigger.SetActive(false);
        respawnTrigger.SetActive(true);
        gameObject.SetActive(false);
    }

    public virtual void Respawn()
    {
        currentState = State.Asleep;
        gameObject.SetActive(true);
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
