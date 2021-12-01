using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject healthBarObj;
    private Image healthBar;
    [SerializeField] private float healthBarLenght = 100f;
    [SerializeField] private float healthBarWidth = 20f;
    [SerializeField] private float healthBarOffset = 0.6f;

    [SerializeField]private int maxHealth = 3;
    [SerializeField]private int currentHealth;

    private Animator enemyAnimator;
    private EnemyBehaviour enemyBehaviour;
    
    void Start()
    {
        InitEnemy();
    }

    private void Update()
    {
        if (enemyBehaviour.currentState != EnemyBehaviour.State.Dead)
        {
            UpdateHealthBarPosition();
        }
        
    }

    private void UpdateHealthBarPosition()
    {
        var hpPos = transform.position;

        hpPos.z = 0f;
        hpPos.y += healthBarOffset;

        healthBarObj.transform.position = hpPos;
    }

    private void ResizeHealthBar()
    {
        healthBar.rectTransform.localScale = Vector3.one;
        healthBar.rectTransform.sizeDelta = new Vector2(healthBarLenght, healthBarWidth);
    }
    
    public void InitEnemy()
    {
        currentHealth = maxHealth;
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemyBehaviour = transform.parent.gameObject.GetComponent<EnemyBehaviour>();
        
        healthBar = healthBarObj.GetComponent<Image>();
        healthBarObj.SetActive(false);
        
        ResizeHealthBar();
    }

    public void TakeDamage(int damage)
    {
        ResizeHealthBar();
        
        healthBarObj.SetActive(true);
        
        currentHealth -= damage;

        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        enemyAnimator.SetBool("IsDead", true);
        
        enemyBehaviour.Die();
        
        healthBarObj.SetActive(false);
    }
}
