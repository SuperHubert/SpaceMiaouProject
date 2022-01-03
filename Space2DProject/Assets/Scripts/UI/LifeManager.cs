using System;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int lifeBar = 9;
    public int maxHP = 9;
    public bool isInGodMode = false;
    public bool canTakeDamge = true;
    public float invulTime = 1f;

    [SerializeField] private Material flashMaterial;
    public float flashDuration = 0.1f;
    private Camera mainCamera;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    public Animator anim;

    #region Singleton
    public static LifeManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        spriteRenderer = LevelManager.Instance.Player().GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        mainCamera = LevelManager.Instance.Camera();
    }

    public void TakeDamages(int damages)
    {
        if(isInGodMode || !canTakeDamge) return;
        lifeBar -= damages;
        if (damages > 0)
        {
            Flash();
            StartCoroutine(InvulFrames());
        }
        if (lifeBar > maxHP)
        {
            lifeBar = maxHP;
        }
        else if (lifeBar < 0)
        {
            lifeBar = 0;
            Die();
        }

        UIManager.Instance.SetHpUI(lifeBar, !(damages > 0));
        

    }

   public void Die(bool instant = false)
   {
       GetComponent<Combat2>().playerAnimator.SetTrigger("Dead");
       Time.timeScale = 1f;
       InputManager.canInput = false;
       StartCoroutine(PlayDyingAnimation(instant));
   }

   IEnumerator PlayDyingAnimation(bool instant = false)
   {
       if (!instant) yield return new WaitForSeconds(1.5f);
       yield return new WaitForSeconds(0.5f);
       LoadingManager.Instance.UpdateLoading();
       LoadingLevelData.Instance.ResetData();
       LoadingManager.Instance.LoadScene(3);
       InputManager.canInput = true;
   }

   IEnumerator InvulFrames()
   {
       canTakeDamge = false;
       yield return new WaitForSeconds(invulTime);
       canTakeDamge = true;
   }

   private void Flash()
   {
       if(flashRoutine != null) StopCoroutine(flashRoutine);
       mainCamera.DOShakePosition(0.05f,new Vector3(0.1f,0.4f,0),8,0,true);
       flashRoutine = StartCoroutine(FlashRoutine());
   }
   
   IEnumerator FlashRoutine()
   {
       spriteRenderer.material = flashMaterial;
       yield return new WaitForSeconds(0.1f);
       spriteRenderer.material = originalMaterial;
       flashRoutine = null;
   }

   //testing camera shake values
   /*
   public float duration;
   public Vector3 strength;
   public int vibration;
   public bool fadeOut;
   
   public void TestShake()
   {
       mainCamera.DOShakePosition(duration,strength,vibration,0,fadeOut);
   }
    */

}
