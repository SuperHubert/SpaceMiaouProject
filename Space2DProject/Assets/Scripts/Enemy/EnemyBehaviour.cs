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

    public virtual void Die()
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

}
