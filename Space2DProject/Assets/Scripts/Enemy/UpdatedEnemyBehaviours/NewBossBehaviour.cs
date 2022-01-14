using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBossBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject healthBarBack;
    [SerializeField] private GameObject rockAttackPrefab;


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
        StartCoroutine(RockAttack());
    }

    public override void Die(bool destroy = false)
    {
        base.Die();
        healthBarBack.SetActive(false);
        LevelManager.Instance.Level().GetChild(3).position = new Vector3(0,5,0);
        LevelManager.Instance.Level().GetChild(3).GetComponent<Collider2D>().enabled = true;
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
    }

    IEnumerator RockAttack()
    {
        GameObject rock = Instantiate(rockAttackPrefab, player.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(rock);
    }
}
