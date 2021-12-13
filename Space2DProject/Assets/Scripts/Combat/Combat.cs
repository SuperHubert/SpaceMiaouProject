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
    public int damage = 10;
    public int specialDamage = 10;
    

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
        if (Input.GetButtonDown("RightAttack") && isAttacking == false)
        {
            isAttacking = true;
            playerAnimator.SetBool("IsAttacking", true);
            playerAnimator.Play("RightBaseAttack");
            Invoke("ResetAttack", 0.5f);

            Physics2D.OverlapCollider(baseRightCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += 15;
                }
            }
        }
        
            
        if (Input.GetButtonDown("LeftAttack") && isAttacking == false)
        {
            isAttacking = true;
            playerAnimator.SetBool("IsAttacking", true);
            playerAnimator.Play("LeftBaseAttack");
            Invoke("ResetAttack", 0.5f);
            
            Physics2D.OverlapCollider(baseLeftCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += 15;
                }
            }
        }
        
        
        if (Input.GetButtonDown("UpAttack") && isAttacking == false)
        {
            isAttacking = true;
            playerAnimator.SetBool("IsAttacking", true);
            playerAnimator.Play("BackBaseAttack");
            Invoke("ResetAttack", 0.5f);
            
            Physics2D.OverlapCollider(baseUpCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += 15;
                }
            }
        }
        
        
        if (Input.GetButtonDown("DownAttack") && isAttacking == false)
        {
            isAttacking = true;
            playerAnimator.SetBool("IsAttacking", true);
            playerAnimator.Play("FrontBaseAttack");
            Invoke("ResetAttack", 0.5f);
            
            Physics2D.OverlapCollider(baseDownCollider, new ContactFilter2D().NoFilter(), hits);

            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.layer == 7)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    GetComponent<SprayAttack>().currentSpray += 15;
                }
            }
        }
    }

    void SpecialAttack()
    {
        if (Input.GetButtonDown("SpecialAttack") && !isAttacking && canSpecialAttack)
        {
            isSpecialAttacking = true;
            canSpecialAttack = false;
            
            playerAnimator.Play("SpinAttack");
            playerAnimator.SetBool("IsAttacking", true);
            
            GetComponent<PlayerMovement>().speed = 2;

            Invoke("WaveAttack", 0.3f);
            Invoke("WaveAttack", 0.6f);
            Invoke("ResetAttack", 0.9f);
            Invoke("ResetSpecialAttack", specialAttackCoolDown);
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
                GetComponent<SprayAttack>().currentSpray += 20;
            }
        }
    }
    
    void ResetAttack()
    {
        isAttacking = false;
        playerAnimator.SetBool("IsAttacking", false);
        GetComponent<PlayerMovement>().speed = 7;
    }

    void ResetSpecialAttack()
    {
        canSpecialAttack = true;
        isSpecialAttacking = false;
    }
}
