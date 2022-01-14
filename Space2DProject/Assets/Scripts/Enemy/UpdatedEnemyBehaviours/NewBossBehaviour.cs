using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBossBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject healthBarBack;
    [SerializeField] private GameObject rockAttackPrefab;
    private EnemyHealth health;
    private ObjectPooler pooler;

    public int phase = 0;

    private bool rockRoutineRunning;

    protected override void InitVariables()
    {
        base.InitVariables();
        pooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        if(currentState != State.Awake) return;
        
        if (actionCd > 0)
        {
            actionCd--;
        }
        else
        {
            isPerformingAction = false;
            actionTrigger.SetActive(true);
        }

        if (!rockRoutineRunning) StartCoroutine(SimpleRockAttack());

        //if(IsAlreadyAttacking()) return;
        //StartCoroutine(CloseAttack(ChooseRandomAttack()));

    }

    public override void WakeUp()
    {
        base.WakeUp();
        healthBarBack.SetActive(true);
        healthBarBack.transform.GetChild(0).gameObject.SetActive(true);
    }

    protected override void Action()
    {
        base.Action();
    }

    public override void Die(bool destroy = false)
    {
        base.Die();
        healthBarBack.SetActive(false);
        LevelManager.Instance.Level().GetChild(3).position = new Vector3(0,5,0);
        LevelManager.Instance.Level().GetChild(3).GetComponent<Collider2D>().enabled = true;
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
    }

    private void CheckPhase()
    {
        
    }

    IEnumerator SimpleRockAttack()
    {
        rockRoutineRunning = true;
        GameObject rock = pooler.SpawnFromPool("Rock", player.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        Destroy(rock);
        rockRoutineRunning = false;
    }
}
