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

    protected override void InitVariables()
    {
        UpdateAppearance();
        base.InitVariables();
    }

    protected override void Action()
    {
        base.Action();
        
        var targetPos = player.position;

        var spawnedObj = ObjectPooler.Instance.SpawnFromPool("Root", targetPos, Quaternion.identity);
        spawnedObj.GetComponent<Root>().Spawn();
    }

    public override void Die(bool destroy = false)
    {
        StartCoroutine(DeathAnim());
        base.Die(destroy);
    }

    IEnumerator DeathAnim()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(0.5f);
        base.Die();
    }

    private void UpdateAppearance()
    {
        var biome = LevelManager.Instance.GetBiome();
        
        switch (biome)
        {
            case 0:
                actionCdMax = 200;
                animator.SetLayerWeight (1, 1f);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (3, 0f);
                break;
            case 1:
                actionCdMax = 150;
                animator.SetLayerWeight (1, 0f);
                animator.SetLayerWeight (2, 1f);
                animator.SetLayerWeight (3, 0f);
                break;
            case 2:
                actionCdMax = 110;
                animator.SetLayerWeight (1, 0);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (3, 1f);
                break;
            default:
                animator.SetLayerWeight (1, 1f);
                animator.SetLayerWeight (2, 0f);
                animator.SetLayerWeight (3, 0f);
                break;
        }

    }
}