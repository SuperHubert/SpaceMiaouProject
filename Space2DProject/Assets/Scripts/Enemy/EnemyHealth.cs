using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]private int maxHealth = 3;
    [SerializeField]private int currentHealth;

    private Animator enemyAnimator;
    private EnemyBehaviour enemyBehaviour;
    
    void Start()
    {
        InitEnemy();
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void InitEnemy()
    {
        currentHealth = maxHealth;
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemyBehaviour = transform.parent.gameObject.GetComponent<EnemyBehaviour>();
    }

    private void Die()
    {
        enemyAnimator.SetBool("IsDead", true);
        
        enemyBehaviour.Die();
    }
}
