using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public enum State {Asleep, Awake, Dead};
    
    [SerializeField] public State currentState;

    public bool respawn = true;

    [SerializeField] protected bool hasAction;
    [SerializeField] protected int actionCdMax;
    [SerializeField] protected int actionCd;
    protected bool isPerformingAction = false;

    protected NavMeshAgent agent;
    protected EnemyHealth health;
    protected Transform player;

    public Animator animator;
    [SerializeField] protected Transform enemyTransform;
    protected GameObject enemy;
    [SerializeField] protected Transform triggersTransform;
    [SerializeField] protected Transform respawnTriggerTransform;
    private GameObject wakeUpTrigger;
    private GameObject sleepTrigger;
    private GameObject respawnTrigger;
    protected GameObject actionTrigger;

    public bool stunned = false;

    public bool cleared = false;

    protected AudioManager am;

    private CombatManager cm;


    private void Start()
    {
        InitVariables();
    }

    protected virtual void InitVariables()
    {
        agent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if(agent.isOnNavMesh) agent.SetDestination(enemyTransform.position);
        enemy = enemyTransform.gameObject;
        
        health = enemy.GetComponent<EnemyHealth>();
        
        (wakeUpTrigger = triggersTransform.GetChild(0).gameObject).SetActive(true);
        (sleepTrigger = triggersTransform.GetChild(1).gameObject).SetActive(false);
        (actionTrigger= triggersTransform.GetChild(2).gameObject).SetActive(false);
        (respawnTrigger = respawnTriggerTransform.gameObject).SetActive(false);

        player = LevelManager.Instance.Player().transform;

        if (!hasAction) return;
        
        actionCd = 0;
        isPerformingAction = false;
        
        am = AudioManager.Instance;
        cm = CombatManager.Instance;
    }
    
    public virtual void WakeUp()
    {
        if (currentState == State.Dead) return;
        
        wakeUpTrigger.SetActive(false);
        sleepTrigger.SetActive(true);
        actionTrigger.SetActive(hasAction);
        
        cm.Add(gameObject);
        
        currentState = State.Awake;
    }

    public virtual void Sleep()
    {
        sleepTrigger.SetActive(false);
        wakeUpTrigger.SetActive(true);
        actionTrigger.SetActive(false);

        if(agent.isOnNavMesh) agent.SetDestination(transform.position);
        
        cm.Remove(gameObject);
        
        currentState = State.Asleep;
        isPerformingAction = false;
        
    }

    public virtual void Die(bool destroy = false)
    {
        if (destroy)
        {
            Destroy(gameObject);
        }
        
        cm.Remove(gameObject);

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
        if(cleared) return;
        
        animator.SetBool("isDead",false);
        
        enemyTransform.gameObject.SetActive(true);
        
        if(agent.isOnNavMesh) agent.isStopped = false;
        
        wakeUpTrigger.SetActive(true);
        sleepTrigger.SetActive(false);
        actionTrigger.SetActive(false);
        respawnTrigger.SetActive(false);

        enemyTransform.GetComponent<Collider2D>().enabled = true;
        
        enemyTransform.position = respawnTrigger.transform.position;
        health.InitEnemy();
        
        cm.Remove(gameObject);
        
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
        if(agent.isOnNavMesh) agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        if(agent.isOnNavMesh) agent.isStopped = false;
        stunned = false;
        animator.SetBool("Damage", false);
    }

    public virtual void KnockBack(Vector3 pos, float duration = 0.75f)
    {
        StartCoroutine(KnockBackRoutine(pos,duration));
    }

    public virtual IEnumerator KnockBackRoutine(Vector3 pos,float duration)
    {
        agent.velocity = Vector3.zero;
        if(agent.isOnNavMesh) agent.SetDestination(enemyTransform.position);
        yield return null;
        agent.acceleration = 100;
        agent.speed = 20;
        agent.stoppingDistance = 1f;
        if(agent.isOnNavMesh) agent.SetDestination(pos);
        yield return new WaitForSeconds(duration);
        if(agent.isOnNavMesh) agent.SetDestination(enemyTransform.position);
        
    }

}
