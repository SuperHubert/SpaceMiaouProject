using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat2 : MonoBehaviour
{
    //Animations
    public Animator playerAnimator;
    
    //Attacks
    public Collider2D baseRightCollider;
    public Collider2D baseLeftCollider;
    public Collider2D baseUpCollider;
    public Collider2D baseDownCollider;
    
    public List<Collider2D> hits;

    public bool isAttacking = false;

    private Vector3 attackDirection;
    
    //Damage
    public int damage = 1;

 
    void Start()
    {
        
    }

   
    void Update()
    {
        attackDirection = GetComponent<PlayerMovement>().lastDirection;
        
        Attack();
    }

    void Attack()
    {
        if (Input.GetButtonDown("LeftAttack") && !isAttacking)
        {
            isAttacking = true;
            Invoke("ResetAttack", 0.5f);

            if (attackDirection.x > 0 && Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
            {
                Physics2D.OverlapCollider(baseRightCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("D");
            }
            else if (attackDirection.x < 0 && Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
            {
                Physics2D.OverlapCollider(baseLeftCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("G");
            }
            else if (attackDirection.y > 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
            {
                Physics2D.OverlapCollider(baseUpCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("H");
            }
            else if (attackDirection.y < 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
            {
                Physics2D.OverlapCollider(baseDownCollider, new ContactFilter2D().NoFilter(), hits);
                Debug.Log("B");
            }
            
            foreach (Collider2D enemy in hits)
            {
                if (enemy.gameObject.CompareTag("Enemy"))
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
        }
    }
    
    void ResetAttack()
    {
        isAttacking = false;
    }
}
