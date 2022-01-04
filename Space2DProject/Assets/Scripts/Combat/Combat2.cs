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
                Physics2D.OverlapCollider(baseRightCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("D");
                playerAnimator.Play("RightBaseAttack");
                Destroy(Instantiate(baseFX, transform.position+ new Vector3(0.5f,0,0), Quaternion.Euler(0,0,-90), gameObject.transform), 0.5f);
                
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
            else if (attackDirection.x < 0 && Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
            {
                Physics2D.OverlapCollider(baseLeftCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("G");
                playerAnimator.Play("LeftBaseAttack");
                Destroy(Instantiate(baseFX, transform.position+ new Vector3(-0.5f,0,0), Quaternion.Euler(0,0,90), gameObject.transform), 0.5f);
                
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
            else if (attackDirection.y > 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
            {
                Physics2D.OverlapCollider(baseUpCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("H");
                playerAnimator.Play("BackBaseAttack");
                Destroy(Instantiate(baseFX, transform.position +new Vector3(0,0.4f,0), Quaternion.identity, gameObject.transform), 0.5f);
                
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
            else if (attackDirection.y < 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
            {
                Physics2D.OverlapCollider(baseDownCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("B");
                playerAnimator.Play("FrontBaseAttack");
                Destroy(Instantiate(baseFX, transform.position+ new Vector3(0,-0.5f,0), Quaternion.Euler(0,0,180), gameObject.transform), 0.5f);
                
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
    }

    void SpecialAttack()
    {
        if (specialAttack && !isAttacking && canSpecialAttack)
        {
            isSpecialAttacking = true;
            canSpecialAttack = false;
            
            playerAnimator.Play("SpinAttack");
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
        Collider2D[] hit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 0.2f), 1.5f);

        foreach (Collider2D enemy in hit)
        {
            if (enemy.gameObject.layer == 7)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(specialDamage);
                enemy.GetComponent<Rigidbody2D>().AddForce( (enemy.transform.position - transform.position).normalized* force);
                GetComponent<SprayAttack>().currentSpray += sprayGainSpecial;
                GetComponent<SprayAttack>().UpdateSprayBar();
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
