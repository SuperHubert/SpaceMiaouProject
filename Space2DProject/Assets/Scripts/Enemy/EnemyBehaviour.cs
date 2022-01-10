using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public enum State {Asleep, Awake, Dead};
    
    [SerializeField] public State currentState;

    [SerializeField] protected bool respawn = true;

    [SerializeField] protected bool hasAction;
    [SerializeField] protected int actionCdMax;
    [SerializeField] protected int actionCd;
    protected bool isPerformingAction = false;

    protected NavMeshAgent agent;
    protected EnemyHealth health;
    protected Transform player;

    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform enemyTransform;
    protected GameObject enemy;
    [SerializeField] protected Transform triggersTransform;
    [SerializeField] protected Transform respawnTriggerTransform;
    private GameObject wakeUpTrigger;
    private GameObject sleepTrigger;
    private GameObject respawnTrigger;
    protected GameObject actionTrigger;

    public bool stunned = false;


    private void Start()
    {
        InitVariables();
    }

    protected virtual void InitVariables()
    {
        agent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(enemyTransform.position);
        enemy = enemyTransform.gameObject;
        
        health = enemy.GetComponent<EnemyHealth>();

        //animator = enemy.GetComponent<Animator>();
        
        (wakeUpTrigger = triggersTransform.GetChild(0).gameObject).SetActive(true);
        (sleepTrigger = triggersTransform.GetChild(1).gameObject).SetActive(false);
        (actionTrigger= triggersTransform.GetChild(2).gameObject).SetActive(false);
        (respawnTrigger = respawnTriggerTransform.gameObject).SetActive(false);

        player = LevelManager.Instance.Player().transform;

        if (!hasAction) return;
        
        actionCd = 0;
        isPerformingAction = false;
    }
    
    public virtual void WakeUp()
    {
        if (currentState == State.Dead) return;
        
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

    public virtual void Die(bool destroy = false)
    {
        //animator.SetTrigger("Dead");

        agent.Warp(transform.position);
        
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
        
        enemyTransform.gameObject.SetActive(false);
        if (destroy)
        {
            Destroy(gameObject);
        }
        
        currentState = State.Dead;
    }

    public virtual void Respawn()
    {
        if (!respawn) return;
        
        animator.SetBool("isDead",false);
        
        enemyTransform.gameObject.SetActive(true);
        
        agent.isStopped = false;
        
        wakeUpTrigger.SetActive(true);
        sleepTrigger.SetActive(false);
        actionTrigger.SetActive(false);
        respawnTrigger.SetActive(false);
        
        enemyTransform.position = respawnTrigger.transform.position;
        health.InitEnemy();
        
        currentState = State.Asleep;
        
    }

    public void ExecuteAction()
    {
        if (currentState == State.Dead) return;
        if (actionCd != 0 || !hasAction) return;
        isPerformingAction = true;
        actionCd = actionCdMax;
        Action();
    }

    protected virtual void Action()
    {
        if (isPerformingAction) return;
        actionTrigger.SetActive(false);
    }

    public virtual void Stun(float duration = 1f)
    {
        if(!stunned) StartCoroutine(StunRoutine(duration));
    }

    public virtual IEnumerator StunRoutine(float duration = 1f)
    {
        stunned = true;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        agent.isStopped = false;
        stunned = false;
    }

    public virtual void KnockBack(Vector3 pos, float duration = 0.75f)
    {
        StartCoroutine(KnockBackRoutine(pos,duration));
    }

    public virtual IEnumerator KnockBackRoutine(Vector3 pos,float duration)
    {
        agent.velocity = Vector3.zero;
        agent.SetDestination(enemyTransform.position);
        yield return null;
        agent.acceleration = 100;
        agent.speed = 20;
        agent.stoppingDistance = 1f;
        agent.SetDestination(pos);
        yield return new WaitForSeconds(duration);
        agent.SetDestination(enemyTransform.position);
        
    }

}
