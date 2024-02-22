using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject healthBarBack;
    [SerializeField] private GameObject teleportedAttack;
    [SerializeField] private List<GameObject> attackList = new List<GameObject>();
    [SerializeField] private List<GameObject> usableAttacks = new List<GameObject>();


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
        
        if(IsAlreadyAttacking()) return;
        StartCoroutine(CloseAttack(ChooseRandomAttack()));
        
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
        StartCoroutine(CloseAttack(attackList[0]));
    }

    public override void Die(bool destroy = false)
    {
        base.Die();
        healthBarBack.SetActive(false);
        LevelManager.Instance.Level().GetChild(3).position = new Vector3(0,5,0);
        LevelManager.Instance.Level().GetChild(3).GetComponent<Collider2D>().enabled = true;
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
    }

    IEnumerator CloseAttack(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<Collider2D>().enabled = false;
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        obj.GetComponent<SpriteRenderer>().color = Color.yellow;
        //play Angry Animation
        yield return new WaitForSeconds(2f);
        //play Attack Animation
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        obj.GetComponent<Collider2D>().enabled = true;
        obj.GetComponent<SpriteRenderer>().color = Color.red;
        obj.GetComponent<DamageOnTriggerStay>().canDamage = true;
        yield return new WaitForSeconds(1f);
        //back to normal
        obj.SetActive(false);
        
    }

    private bool IsAlreadyAttacking()
    {
        bool returnValue = false;
        foreach (var attack in attackList)
        {
            if (attack.activeSelf) returnValue = true;
        }
        return returnValue;
    }

    private GameObject ChooseRandomAttack()
    {
        if (Random.Range(0, 2) == 1)
        {
            teleportedAttack.transform.position = LevelManager.Instance.Player().transform.position;
            return teleportedAttack;
        }
        else
        {
            while (true)
            {
                if (usableAttacks.Count != 0)
                {
                    GameObject returnObj = usableAttacks[Random.Range(0, usableAttacks.Count)];
                    usableAttacks.Remove(returnObj);
                    return returnObj;
                }
                else
                {
                    foreach (var attack in attackList)
                    {
                        usableAttacks.Add(attack);
                    }
                }
            }
        }
    }
}