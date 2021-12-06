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

    //Damage
    public int attackDamage = 1;

    //AttackColliders
    public String weaponEquipped;
    public GameObject clawCollider;
    public GameObject broomCollider;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("BaseAttack") && !isAttacking)
            {
                AnimPlayer();
                isAttacking = true;
                
                if (weaponEquipped == "Claws")
                {
                    Invoke("ResetAttack", 0.5f);
                    clawCollider.GetComponent<ClawAttack>().dealDamage();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
                
                if (weaponEquipped == "Broom")
                {
                    Invoke("ResetAttack", 0.8f);
                    broomCollider.GetComponent<BroomAttack>().dealDamage();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
                
            }
        }
    }

    void AnimPlayer()
    {
        if (weaponEquipped == "Claws")
        {
            animPlayer.SetTrigger("ClawAttack");
        }
        else if (weaponEquipped == "Broom")
        {
            animPlayer.SetTrigger("BroomAttack");
        }
    }
    
    void ResetAttack()
    {
        isAttacking = false;
    }
}
