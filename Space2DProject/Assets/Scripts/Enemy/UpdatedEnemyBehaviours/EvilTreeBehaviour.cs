using System.Collections;
using UnityEngine;

public class EvilTreeBehaviour : EnemyBehaviour
{
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Die1 = Animator.StringToHash("Die");
    private SpriteRenderer spriteRender;

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
        spriteRender = animator.gameObject.GetComponent<SpriteRenderer>();
        base.InitVariables();
    }

    protected override void Action()
    {
        base.Action();
        animator.SetTrigger(Attack1);

        var targetPos = player.position;

        var spawnedObj = ObjectPooler.Instance.SpawnFromPool("Root", targetPos, Quaternion.identity);
        spawnedObj.GetComponent<Root>().Spawn();
    }

    public override void Die(bool destroy = false)
    {
        StartCoroutine(DeathAnim());
    }
    
    IEnumerator DeathAnim()
    {
        currentState = State.Dead;
        animator.SetTrigger(Die1);
        yield return new WaitUntil(() => spriteRender.color.a == 0);
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