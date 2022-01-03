using System.Collections;
using UnityEngine;

public class EvilTreeBehaviour : EnemyBehaviour
{
    //[SerializeField] private Animator animator;

    private void Update()
    {
        animator.SetBool("Awake", true);
        
        if (currentState != State.Awake) return;

        if (actionCd > 0)
        {
            actionCd--;
        }
    }

    protected override void Action()
    {
        base.Action();
        
        var targetPos = player.position;

        var spawnedObj = ObjectPooler.Instance.SpawnFromPool("Root", targetPos, Quaternion.identity);
        spawnedObj.GetComponent<Root>().Spawn();
    }
}