using System.Collections;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject healthBarBack;
    [SerializeField] private GameObject closeAttack;
    
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
        StartCoroutine(CloseAttack());
    }

    public override void Die()
    {
        base.Die();
        healthBarBack.SetActive(false);
        LevelManager.Instance.Level().GetChild(3).position = new Vector3(0,5,0);
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
    }

    IEnumerator CloseAttack()
    {
        closeAttack.SetActive(true);
        closeAttack.GetComponent<CircleCollider2D>().enabled = false;
        closeAttack.GetComponent<SpriteRenderer>().color = Color.yellow;
        //play Angry Animation
        yield return new WaitForSeconds(2f);
        //play Attack Animation
        closeAttack.GetComponent<CircleCollider2D>().enabled = true;
        closeAttack.GetComponent<SpriteRenderer>().color = Color.red;
        closeAttack.GetComponent<CloseUpAttack>().canDamage = true;
        yield return new WaitForSeconds(1f);
        //back to normal
        closeAttack.SetActive(false);
        
    }

    private bool IsAlreadyAttacking()
    {
        return closeAttack.activeSelf;
    }
}