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
        Debug.Log("Action");
    }

    public override void Die()
    {
        base.Die();
        
        ConsoleManager.Instance.Print("Bravo, vous avez fini le jeu");
    }

    IEnumerator CloseAttack()
    {
        //play Angry Animation
        yield return new WaitForSeconds(2f);
        
        
    }
}