using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Transform healthBarTransform;
    private GameObject healthBarObj;
    private Image healthBarFrontImg;
    private Image healthBarBackImg;
    private Camera cam;
    [SerializeField] private float healthBarLenght = 100f;
    [SerializeField] private float healthBarWidth = 20f;
    [SerializeField] private float healthBarOffset = 0.6f;

    public bool isBurning = false;
    public float burnRate = 1f;
    private Coroutine burnRoutine;

    public bool canTakeDamage = true;
    [SerializeField]private float maxHealth = 3;
    [SerializeField]private float currentHealth;
    private bool isDying = false;

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
        
                
        if (healthBarBackImg.fillAmount != healthBarFrontImg.fillAmount)
        {
            HealthDecreaseEffect();
        }

        
    }

    private void UpdateHealthBarPosition()
    {
        var hpPos = transform.position;

        hpPos.z = 0f;
        hpPos.y += healthBarOffset;

        hpPos = cam.WorldToScreenPoint(hpPos);
        
        healthBarObj.transform.position = hpPos;
    }
    
    public void InitEnemy()
    {
        healthBarFrontImg = healthBarTransform.GetChild(1).GetComponent<Image>();
        healthBarBackImg = healthBarTransform.GetChild(0).GetComponent<Image>();

        currentHealth = maxHealth;
        enemyBehaviour = transform.parent.gameObject.GetComponent<EnemyBehaviour>();

        healthBarObj = healthBarTransform.gameObject;
        
        healthBarObj.SetActive(false);

        isBurning = false;
        
        cam = Camera.main;

        isDying = false;
        
        ResizeHealthBar();
    }

    public void TakeDamage(float damage,bool stun = false, float duration = 1)
    {
        if (canTakeDamage)
        {
            ResizeHealthBar();
            
            enemyAnimator.SetTrigger("TakeDamage");
        
            currentHealth -= damage;
            
            healthBarObj.SetActive(currentHealth > 0);

            healthBarFrontImg.fillAmount = currentHealth / maxHealth;
            
            if (currentHealth <= 0 && !isDying)
            {
                Die();
            }
            else if(stun)
            {
                enemyBehaviour.Stun(duration);
            }
        
            enemyAnimator.ResetTrigger("TakeDamage");
        }
        else
        {
            //play sound and particle
        }
        
        enemyAnimator.ResetTrigger("TakeDamage");
    }
    
    private void ResizeHealthBar()
    {
        healthBarFrontImg.rectTransform.localScale = Vector3.one;
        healthBarFrontImg.rectTransform.sizeDelta = new Vector2(healthBarLenght, healthBarWidth);
        healthBarBackImg.rectTransform.localScale = Vector3.one;
        healthBarBackImg.rectTransform.sizeDelta = new Vector2(healthBarLenght, healthBarWidth);
    }
    
    public void Die(bool increaseScore = true)
    {
        isDying = true;

        if(increaseScore) UIManager.Instance.IncreaseScore((int)maxHealth);
        
        healthBarObj.SetActive(false);
        
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
    
    private void HealthDecreaseEffect()
    {
        healthBarBackImg.fillAmount = Mathf.Lerp (healthBarBackImg.fillAmount, healthBarFrontImg.fillAmount, 1f * Time.deltaTime);
        
    }

    public void KnockBack(Vector3 pos, float duration = 1f)
    {
        enemyBehaviour.KnockBack(pos,duration);
    }

}
