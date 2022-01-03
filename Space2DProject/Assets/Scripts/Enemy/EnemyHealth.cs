using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject healthBarObj;
    private Image healthBar;
    [SerializeField] private float healthBarLenght = 100f;
    [SerializeField] private float healthBarWidth = 20f;
    [SerializeField] private float healthBarOffset = 0.6f;

    public bool isBurning = false;
    public float burnRate = 1f;
    private Coroutine burnRoutine;

    [SerializeField]private float maxHealth = 3;
    [SerializeField]private float currentHealth;

    [SerializeField]private bool moveHealthBar = true;

    public Animator enemyAnimator;
    private EnemyBehaviour enemyBehaviour;
    
    void Start()
    {
        InitEnemy();
    }

    private void Update()
    {
        if (enemyBehaviour.currentState != EnemyBehaviour.State.Dead && moveHealthBar)
        {
            UpdateHealthBarPosition();
        }
        
    }

    private void UpdateHealthBarPosition()
    {
        var hpPos = transform.position;

        hpPos.z = 0f;
        hpPos.y += healthBarOffset;

        hpPos = Camera.main.WorldToScreenPoint(hpPos);
        
        healthBarObj.transform.position = hpPos;
    }
    
    public void InitEnemy()
    {
        currentHealth = maxHealth;
        enemyBehaviour = transform.parent.gameObject.GetComponent<EnemyBehaviour>();
        
        healthBar = healthBarObj.GetComponent<Image>();
        healthBarObj.SetActive(false);

        isBurning = false;
        
        ResizeHealthBar();
    }

    public void TakeDamage(float damage)
    {
        ResizeHealthBar();
        
        healthBarObj.SetActive(true);
        
        enemyAnimator.SetTrigger("TakeDamage");
        
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;
        
        if (currentHealth <= 0)
        {
            Die();
        }
        
        enemyAnimator.ResetTrigger("TakeDamage");
    }
    
    private void ResizeHealthBar()
    {
        healthBar.rectTransform.localScale = Vector3.one;
        healthBar.rectTransform.sizeDelta = new Vector2(healthBarLenght, healthBarWidth);
    }
    
    private void Die()
    {
        enemyAnimator.SetTrigger("Dead");
        
        enemyBehaviour.Die();

        isBurning = false;
        
        healthBarObj.SetActive(false);
        
        if(burnRoutine != null) StopCoroutine(burnRoutine);
    }

    public void Burn(float burnDamage = 0.02f)
    {
        isBurning = true;
        if(burnRoutine != null) return;
        burnRoutine = StartCoroutine(BurnRoutine(burnDamage));
    }

    private IEnumerator BurnRoutine(float burnDamage = 0.02f)
    {
        while (isBurning)
        {
            Debug.Log("burn tick");
            TakeDamage(maxHealth * burnDamage);
            yield return new WaitForSeconds(burnRate);
        }
    }
}
