using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBossBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject healthBarBack;
    [SerializeField] private GameObject rockAttackPrefab;
    private ObjectPooler pooler;

    [SerializeField] private GameObject closeRocks;

    public int phase = 0;

    public float rockAttackCdMax;
    private float rockAttackCd;

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
            if (phase > 2)
            {
                rockAttackCd = rockAttackCdMax;
                StartCoroutine(SimpleRocksAttack());
            }
        }

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
        StartCoroutine(CloseRocks());
    }

    public override void Die(bool destroy = false)
    {
        base.Die();
        healthBarBack.SetActive(false);
        LevelManager.Instance.Level().GetChild(3).position = new Vector3(0,5,0);
        LevelManager.Instance.Level().GetChild(3).GetComponent<Collider2D>().enabled = true;
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
    }

    public float value;
    IEnumerator CloseRocks()
    {
        closeRocks.SetActive(true);
        yield return new WaitForSeconds(value);
        closeRocks.SetActive(false);
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
        StartCoroutine(MultipleRocksAttack());
        Debug.Log("nextPhase");
    }
}
