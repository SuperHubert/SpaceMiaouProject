using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class PlayerCombat : MonoBehaviour
{
    //Animations
    public Animator animPlayer;
    public bool isAttacking;

    // Attack
    
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    public float hitBoxRange;
    public float attackRange;
    
    //Damage
    public int attackDamage = 1;
    
    //Enemies
    public LayerMask enemyLayers;
    
    //Direction

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack") && !isAttacking)
            {
                AnimPlayer();
                isAttacking = true;

                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        //Enemies detection
        
        Collider2D[] enemiesHit = 
            Physics2D.OverlapCircleAll(transform.position + GetComponent<PlayerMovement>().lastDirection * hitBoxRange, attackRange, enemyLayers);

        //Enemies damage

        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            Debug.Log("Tuché");
        }
        
        Invoke("ResetAttack", 0.5f);

        //Attack animations
    }

    void AnimPlayer()
    {
        animPlayer.SetTrigger("ClawAttack");
    }
    
    void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + GetComponent<PlayerMovement>().lastDirection * hitBoxRange, attackRange);
    }
}
