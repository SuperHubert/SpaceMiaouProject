using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    public EnemyBehaviour enemyBehaviour;
    
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    [SerializeField] private bool bossHealth = false;
    private NewBossBehaviour bossBehaviour;
    [SerializeField] private List<int> phaseThresholds = new List<int>();

    private AudioManager am;


    void Start()
    {
        InitEnemy();
        if (bossHealth) bossBehaviour = transform.parent.GetComponent<NewBossBehaviour>();
        am = AudioManager.Instance;
    }

    private void Update()
    {
        if(enemyBehaviour == null) return;
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
        healthBarBackImg.fillAmount = 1;
        healthBarFrontImg.fillAmount = 1;

        healthBarObj = healthBarTransform.gameObject;
        
        healthBarObj.SetActive(false);

        isBurning = false;
        
        cam = Camera.main;

        isDying = false;
        
        if(spriteRenderer != null) originalMaterial = spriteRenderer.material;
        
        ResizeHealthBar();
    }

    public void TakeDamage(float damage,bool stun = false, float duration = 1)
    {
        if (canTakeDamage)
        {
            if (bossHealth && bossBehaviour.arenaMode)
            {
                //playSound and particule
                return;
            }
            
            ResizeHealthBar();

            currentHealth -= damage;

            am.Play(20, true);
            
            healthBarObj.SetActive(currentHealth > 0);

            healthBarFrontImg.fillAmount = currentHealth / maxHealth;

            Flash();
            
            if(bossHealth) CheckPhase();
            
            if (currentHealth <= 0 && !isDying)
            {
                Die();
            }
            else if(stun)
            {
                enemyBehaviour.Stun(duration);
            }
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

        if (increaseScore)
        {
            UIManager.Instance.IncreaseScore((int)maxHealth);
            healthBarObj.SetActive(false);
        }
        else
        {
            enemyBehaviour.cleared = true;
            healthBarObj.SetActive(false);
        }

        enemyBehaviour.Die(!increaseScore);

        isBurning = false;

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
        if(healthBarBackImg != null) healthBarBackImg.fillAmount = Mathf.Lerp (healthBarBackImg.fillAmount, healthBarFrontImg.fillAmount, 1f * Time.deltaTime);
    }
    
    private void Flash()
    {
        if (spriteRenderer == null) return;
        if(flashRoutine != null) StopCoroutine(flashRoutine);
        if(bossHealth && currentHealth <= 0) spriteRenderer.material = originalMaterial;
        if(enemyBehaviour.gameObject.activeSelf) flashRoutine = StartCoroutine(FlashRoutine());
    }
   
    IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

    public void KnockBack(Vector3 pos, float duration = 1f)
    {
        enemyBehaviour.KnockBack(pos,duration);
    }

    public void CheckPhase()
    {
        if(bossBehaviour.phase >= phaseThresholds.Count) return;
        if (currentHealth < phaseThresholds[bossBehaviour.phase]) bossBehaviour.TriggerNextPhase();

}

}
