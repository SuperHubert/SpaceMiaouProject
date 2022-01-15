using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBossBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject healthBarBack;
    [SerializeField] private GameObject rockAttackPrefab;
    private ObjectPooler pooler;
    
    [SerializeField] private GameObject closeUpAttack;
    [SerializeField] private GameObject part1Triggers;
    [SerializeField] private GameObject arena;
    [SerializeField] private GameObject part2Triggers;
    
    public int phase = 0;
    public bool arenaMode = false;

    public float rockAttackCdMax;
    private float rockAttackCd;
    private bool rockFalls = false;

    protected override void InitVariables()
    {
        base.InitVariables();
        pooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        if(currentState != State.Awake) return;

        if (rockAttackCd > 0)
        {
            rockAttackCd--;
        }
        else
        {
            if (rockFalls)
            {
                rockAttackCd = rockAttackCdMax;
                StartCoroutine(SimpleRocksAttack());
            }
        }
        
        if(arenaMode) return;

        //if(IsAlreadyAttacking()) return;
        //StartCoroutine(CloseAttack(ChooseRandomAttack()));

    }

    public override void WakeUp()
    {
        base.WakeUp();
        healthBarBack.SetActive(true);
        healthBarBack.transform.GetChild(0).gameObject.SetActive(true);
    }
    
    public override void Die(bool destroy = false)
    {
        base.Die();
        healthBarBack.SetActive(false);
        LevelManager.Instance.Level().GetChild(3).position = new Vector3(0,5,0);
        LevelManager.Instance.Level().GetChild(3).GetComponent<Collider2D>().enabled = true;
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
        animator.SetTrigger("Die");
        part1Triggers.SetActive(false);
        part2Triggers.SetActive(false);
        closeUpAttack.SetActive(false);
        rockFalls = false;
    }
    
    
    IEnumerator SimpleRocksAttack()
    {
        GameObject rock = pooler.SpawnFromPool("Rock", player.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        rock.SetActive(false);
    }
    
    IEnumerator MultipleRocksAttack()
    {
        GameObject rock = Instantiate(rockAttackPrefab, player.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Destroy(rock);
    }

    public void TriggerNextPhase()
    {
        phase++;
        switch (phase)
        {
            case 1:
                StartCoroutine(SimpleRocksAttack());
                part1Triggers.SetActive(true);
                closeUpAttack.SetActive(false);
                break;
            case 2:
                StartCoroutine(SimpleRocksAttack());
                break;
            case 3:
                StartCoroutine(MultipleRocksAttack());
                //spawn mob
                break;
            case 4:
                animator.SetTrigger("EyeOFF");
                arena.SetActive(arenaMode = true);
                part1Triggers.SetActive(false);
                break;
            case 5:
                animator.SetTrigger("SpikesON");
                part1Triggers.SetActive(true);
                rockFalls = true;
                StartCoroutine(SimpleRocksAttack());
                break;
            case 6:
                part1Triggers.SetActive(false);
                part2Triggers.SetActive(true);
                break;
            case 7:
                StartCoroutine(MultipleRocksAttack());
                break;
            default:
                break;
        }
    }

    public void ExitArenaMode()
    {
        arenaMode = false;
        animator.SetTrigger("EyeON");
        animator.SetTrigger("SpikesON");
        part1Triggers.SetActive(true);
        Debug.Log("Arena Completed");
    }
}
