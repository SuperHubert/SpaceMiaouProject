
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Combat2 : MonoBehaviour
{
    //Animations
    public Animator playerAnimator;
    
    //Attacks
    public bool isAttacking = false;
    public int force = 1;
    
    public bool isSpecialAttacking;
    public bool canSpecialAttack = true;
    public float specialAttackCoolDown = 2f;
    
    private Vector3 attackDirection;

    //Colliders
    public Collider2D baseRightCollider;
    public Collider2D baseLeftCollider;
    public Collider2D baseUpCollider;
    public Collider2D baseDownCollider;
    
    public List<Collider2D> hits;
    
    //Fx
    public GameObject specialFX;
    public GameObject baseFX;

    //Damage
    public float damage = 10;
    public float specialDamage = 10;

    public float sprayGainNormal = 15f;
    public float sprayGainSpecial = 20f;

    public bool baseAttack;
    public bool specialAttack;
    
    [HideInInspector] public bool leftAttack;
    [HideInInspector] public bool uptAttack;
    [HideInInspector] public bool downAttack;

    private AudioManager am;


    void Update()
    {
        BasicAttack();
        SpecialAttack();
    }

    void BasicAttack()
    {
        if (baseAttack && !isAttacking)
        {
            attackDirection = GetComponent<PlayerMovement>().lastDirection;
            isAttacking = true;
            playerAnimator.SetBool("IsAttacking", true);
            Invoke("ResetAttack", 0.5f);

            if (attackDirection.x > 0 && Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
            {
                NewAttack(baseRightCollider, "RightBaseAttack",new Vector3(0.7f,-0.3f,0),Quaternion.Euler(0,0,-90));
            }
            else if (attackDirection.x < 0 && Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
            {
                NewAttack(baseLeftCollider, "LeftBaseAttack",new Vector3(-0.7f,-0.3f,0),Quaternion.Euler(0,0,90));
            }
            else if (attackDirection.y > 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
            {
                NewAttack(baseUpCollider, "BackBaseAttack",new Vector3(-0.15f,0.45f,0),Quaternion.Euler(0,0,0));
            }
            else if (attackDirection.y < 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
            {
                NewAttack(baseDownCollider,"FrontBaseAttack",new Vector3(0,-0.65f,0),Quaternion.Euler(0,0,180));
            }
        }
    }

    void NewAttack(Collider2D col,string stateName,Vector3 pos, Quaternion rot)
    {
        Physics2D.OverlapCollider(col, new ContactFilter2D().NoFilter(), hits);
        Debug.Log("D");
        playerAnimator.Play(stateName);
        Destroy(Instantiate(baseFX, transform.position+ pos, rot, gameObject.transform), 0.5f);
                
        foreach (Collider2D enemy in hits)
        {
            if (enemy.gameObject.layer == 7)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage,true);
                GetComponent<SprayAttack>().currentSpray += sprayGainNormal;
                GetComponent<SprayAttack>().UpdateSprayBar();
            }
        }
    }

    void SpecialAttack()
    {
        if (specialAttack && !isAttacking && canSpecialAttack)
        {
            isSpecialAttacking = true;
            canSpecialAttack = false;
            
            playerAnimator.Play("SpinAttack");
            AudioManager.Instance.Play(14, true);
            playerAnimator.SetBool("IsAttacking", true);
            
            GetComponent<PlayerMovement>().speed = 2;
            
            Invoke(nameof(WaveAttack), 0.3f);
            Invoke(nameof(WaveAttack), 0.6f);
            Invoke(nameof(ResetAttack), 0.9f);
            Invoke(nameof(ResetSpecialAttack), specialAttackCoolDown);
        }
    }

    void WaveAttack()
    {
        Destroy(Instantiate(specialFX, transform.position, Quaternion.identity, gameObject.transform), 1f);
        Collider2D[] hit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.2f), 1.8f);

        foreach (Collider2D enemy in hit)
        {
            if (enemy.gameObject.layer == 7)
            {
                if(enemy.GetComponent<EnemyHealth>() != null) enemy.GetComponent<EnemyHealth>().TakeDamage(specialDamage,true,2f);
                //enemy.GetComponent<EnemyHealth>().KnockBack(enemy.transform.position + (enemy.transform.position - transform.position).normalized * 2);
                GetComponent<SprayAttack>().currentSpray += sprayGainSpecial;
                GetComponent<SprayAttack>().UpdateSprayBar();
            }
        }
        
        LifeManager.Instance.canTakeDamge = false;
    }
    
    void ResetAttack()
    {
        isAttacking = false;
        LifeManager.Instance.canTakeDamge = true;
        playerAnimator.SetBool("IsAttacking", false);
        GetComponent<PlayerMovement>().speed = 7;
    }

    void ResetSpecialAttack()
    {
        canSpecialAttack = true;
        isSpecialAttacking = false;
    }
}
