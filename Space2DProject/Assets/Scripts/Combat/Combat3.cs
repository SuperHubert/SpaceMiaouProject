using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat3 : MonoBehaviour
{
    //Animations
    public Animator playerAnimator;
    
    public GameObject baseFX;
    
    //Attacks
    public bool isAttacking = false;
    public bool canAttack = true;

    public bool isSpecialAttacking = false;
    public bool canSpecialAttack = true;

    private Vector3 attackDirection;
    
    public float sprayGainNormal = 15f;
    public float sprayGainSpecial = 20f;

    public bool baseAttack;
    public bool specialAttack;

    public bool hitTrigger = false;
    
    private AudioManager am;
    private SprayAttack sprayAttack;
    private PlayerMovement playerMovement;
    
    [SerializeField] private Material flashMaterial;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        sprayAttack = gameObject.GetComponent<SprayAttack>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        am = AudioManager.Instance;
    }
    
    void Update()
    {
        BasicAttack();
        SpecialAttack();
    }

    private void BasicAttack()
    {
        if (!baseAttack || isAttacking || !canAttack) return;
        attackDirection = playerMovement.lastDirection;
        isAttacking = true;
        canAttack = false;
        AudioManager.Instance.Play(26, true);
        playerAnimator.SetBool("IsAttacking", true);
        playerMovement.speed = 0;

        if (attackDirection.x > 0 && Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            playerAnimator.Play("RightBaseAttack");
            Destroy(Instantiate(baseFX, transform.position+ new Vector3(0.7f,-0.3f,0),Quaternion.Euler(0,0,-90), gameObject.transform), 0.5f);
        }
        else if (attackDirection.x < 0 && Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            playerAnimator.Play("LeftBaseAttack");
            Destroy(Instantiate(baseFX, transform.position+ new Vector3(-0.7f,-0.3f,0),Quaternion.Euler(0,0,90), gameObject.transform), 0.5f);
        }
        else if (attackDirection.y > 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
        {
            playerAnimator.Play("BackBaseAttack");
            Destroy(Instantiate(baseFX, transform.position+ new Vector3(-0.15f,0.45f,0),Quaternion.Euler(0,0,0), gameObject.transform), 0.5f);
        }
        else if (attackDirection.y < 0 && Mathf.Abs(attackDirection.y) > Mathf.Abs(attackDirection.x))
        {
            playerAnimator.Play("FrontBaseAttack");
            Destroy(Instantiate(baseFX, transform.position+ new Vector3(0,-0.65f,0),Quaternion.Euler(0,0,180), gameObject.transform), 0.5f);
        }
        StartCoroutine(BaseAttackReset());
    }

    IEnumerator BaseAttackReset()
    {
        yield return new WaitForSeconds(0.3f);
        ResetAttack();
        if (hitTrigger)
        {
            canAttack = true;
            hitTrigger = false;
            yield break;
        }
        yield return new WaitForSeconds(0.1f);
        canAttack = true;
        hitTrigger = false;
    }
    
    private void SpecialAttack()
    {
        if (!specialAttack || isAttacking || !canSpecialAttack) return;
        isSpecialAttacking = true;
        canSpecialAttack = false;
        canAttack = false;

        playerAnimator.Play("SpinAttack");
        AudioManager.Instance.Play(14, true);
        playerAnimator.SetBool("IsAttacking", true);
            
        playerMovement.speed = 2;

        StartCoroutine(SpecialAttackReset());
    }

    IEnumerator SpecialAttackReset()
    {
        yield return new WaitForSeconds(0.1f);
        LifeManager.Instance.canTakeDamge = false;
        yield return new WaitForSeconds(0.75f);
        ResetAttack();
        canAttack = true;
        yield return new WaitForSeconds(1.15f);
        ResetSpecialAttack();

    }
    
    private void ResetAttack()
    {
        isSpecialAttacking = isAttacking = false;
        LifeManager.Instance.canTakeDamge = true;
        playerAnimator.SetBool("IsAttacking", false);
        playerMovement.speed = 7;
    }

    private void ResetSpecialAttack()
    {
        canSpecialAttack = true;
        isSpecialAttacking = false;
        if(flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }
    
    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.08f);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

    public void SprayGain(bool special)
    {
        sprayAttack.currentSpray += special ? sprayGainSpecial : sprayGainNormal;
        sprayAttack.UpdateSprayBar();
        playerMovement.dashCd = 0;
    }
}
