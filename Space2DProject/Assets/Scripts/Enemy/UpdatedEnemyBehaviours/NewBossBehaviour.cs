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
        if (am == null) am = AudioManager.Instance;
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
            if (!rockFalls) return;
            rockAttackCd = rockAttackCdMax;
            StartCoroutine(SimpleRocksAttack());
        }
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
        var portal = LevelManager.Instance.Level().GetChild(3).gameObject;
        portal.transform.position = Vector3.up;
        portal.GetComponent<SpriteRenderer>().enabled = false;
        portal.GetComponent<Animator>().SetTrigger("Spawn");
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
        animator.SetTrigger("Die");
        part1Triggers.SetActive(false);
        part2Triggers.SetActive(false);
        closeUpAttack.SetActive(false);
        rockFalls = false;
        
        CombatManager.Instance.Clear();

        LevelManager.Instance.Level().GetChild(3).GetComponent<Collider2D>().enabled = true;

        LoadingLevelData.bossDead = true;
        
        AudioManager.Instance.Stop(9, true);
        AudioManager.Instance.Play(5, true);
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
                am.Play(3, true);
                break;
                
            case 3:
                StartCoroutine(MultipleRocksAttack());
                am.Play(21, true);
                am.Play(4, true);
                break;
            case 4:
                animator.SetTrigger("EyeOFF");
                EnterArenaMode();
                part1Triggers.SetActive(false);
                am.Play(3, true);
                break;
            case 5:
                animator.SetTrigger("SpikesON");
                part1Triggers.SetActive(true);
                rockFalls = true;
                StartCoroutine(SimpleRocksAttack());
                am.Play(4, true);
                am.Play(21, true);
                break;
            case 6:
                part1Triggers.SetActive(false);
                part2Triggers.SetActive(true);
                am.Play(3, true);
                break;
            case 7:
                StartCoroutine(MultipleRocksAttack());
                am.Play(4);
                break;
            default:
                break;
        }
    }

    private void EnterArenaMode()
    {
        arena.SetActive(arenaMode = true);
        Transform parent = LevelManager.Instance.Level().GetChild(5);
        foreach (Transform child in arena.transform)
        {
            child.GetChild(0).GetComponent<EnemyHealth>().healthBarTransform.SetParent(parent);
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
