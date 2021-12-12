using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    //Animations
    public Animator playerAnimator;
    
    //Attacks
    public bool isAttacking = false;
    public int force = 1;

    public bool isSpecialAttacking;
    public bool canSpecialAttack = true;
    public float specialAttackCoolDown = 2f;
    
    //Colliders
    public Collider2D baseRightCollider;
    public Collider2D baseLeftCollider;
    public Collider2D baseUpCollider;
    public Collider2D baseDownCollider;

    public List<Collider2D> hits;

    //Damage
    public float damage = 10;
    public float specialDamage = 10;

    public float sprayGainNormal = 15f;
    public float sprayGainSpecial = 20f;

    [HideInInspector] public bool rightAttack;
    [HideInInspector] public bool leftAttack;
    [HideInInspector] public bool uptAttack;
    [HideInInspector] public bool downAttack;
    [HideInInspector] public bool specialAttack;
    

    void Start()
    {
        
    }
    
    void Update()
    {
        BasicAttack();
        SpecialAttack();
    }
    
    void BasicAttack()
    {
        if (rightAttack && isAttacking == false)
        {
            isAttacking = true;
            Invoke(nameof(ResetAttack), 0.5f);

            Physics2D.OverlapCollider(baseRightCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += sprayGainNormal;
                    GetComponent<SprayAttack>().UpdateSprayBar();
                }
            }
        }
        
            
        if (leftAttack && isAttacking == false)
        {
            isAttacking = true;
            Invoke(nameof(ResetAttack), 0.5f);
            
            Physics2D.OverlapCollider(baseLeftCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += sprayGainNormal;
                    GetComponent<SprayAttack>().UpdateSprayBar();
                }
            }
        }
        
        
        if (uptAttack && isAttacking == false)
        {
            isAttacking = true;
            Invoke(nameof(ResetAttack), 0.5f);
            
            Physics2D.OverlapCollider(baseUpCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += sprayGainNormal;
                    GetComponent<SprayAttack>().UpdateSprayBar();
                }
            }
        }
        
        
        if (downAttack && isAttacking == false)
        {
            isAttacking = true;
            Invoke(nameof(ResetAttack), 0.5f);
            
            Physics2D.OverlapCollider(baseDownCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += sprayGainNormal;
                    GetComponent<SprayAttack>().UpdateSprayBar();
                }
            }
        }
    }

    void SpecialAttack()
    {
        if (specialAttack && !isAttacking && canSpecialAttack)
        {
            isSpecialAttacking = true;
            canSpecialAttack = false;
            
            GetComponent<PlayerMovement>().speed = 2;

            Invoke(nameof(WaveAttack), 0.3f);
            Invoke(nameof(WaveAttack), 0.6f);
            Invoke(nameof(ResetAttack), 0.9f);
            Invoke(nameof(ResetSpecialAttack), specialAttackCoolDown);
        }
    }

    public void WaveAttack()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.2f), 1.5f);
        
        foreach (Collider2D enemy in hit)
        {
            if (enemy.gameObject.layer == 7)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(specialDamage);
                enemy.GetComponent<Rigidbody2D>().AddForce( (enemy.transform.position - transform.position).normalized* force);
                GetComponent<SprayAttack>().currentSpray += sprayGainSpecial;
            }
        }
    }
    
    void ResetAttack()
    {
        isAttacking = false;
        GetComponent<PlayerMovement>().speed = 7;
    }

    void ResetSpecialAttack()
    {
        canSpecialAttack = true;
        isSpecialAttacking = false;
    }
}
