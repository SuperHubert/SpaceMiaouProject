using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //Health
    public int maxHealth = 3;
    public int currentHealth;
    
    //Animator
    public Animator enemyAnimator;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    
    public void TakeDamage(int damage)
    {
        //Damage
        currentHealth -= damage;
        
        //Hurt Animation
        enemyAnimator.SetTrigger("Hurt");
        
        //Death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        
        //Die Animation
        enemyAnimator.SetBool("IsDead", true);
        
        //Disable Enemy
        GetComponent<CircleCollider2D>().enabled = false;
        this.enabled = false;
        gameObject.SetActive(false);
    }
}
