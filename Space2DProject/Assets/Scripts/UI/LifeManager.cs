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

    [SerializeField] private GameObject deathObj;

    [SerializeField] private Material flashMaterial;
    public float flashDuration = 0.1f;
    private Camera mainCamera;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    public Animator anim;

    private AudioManager am;

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
        am = AudioManager.Instance;
    }

    public void TakeDamages(int damages)
    {
        if(isInGodMode || !canTakeDamge) return;
        lifeBar -= damages;
        if (damages > 0)
        {
            Flash();
            am.Play(13, true);
            StartCoroutine(InvulFrames());
        }
        if (lifeBar > maxHP)
        {
            lifeBar = maxHP;
        }
        else if (lifeBar <= 0)
        {
            lifeBar = 0;
            Die();
        }

        UIManager.Instance.UpdateHp((float)lifeBar/maxHP);
        

    }

   public void Die(bool instant = false)
   { 
       //LevelManager.Instance.Player().GetComponent<Combat2>().playerAnimator.SetTrigger("Dead");
       LevelManager.Instance.Player().SetActive(false);
       LoadingLevelData.Instance.score = UIManager.Instance.score;
       Time.timeScale = 1f;
       InputManager.canInput = false;
       
       StartCoroutine(PlayDyingAnimation());
   }

   IEnumerator PlayDyingAnimation(bool instant = false,int scene = 5)
   {
       if (!instant)
       {
           Instantiate(deathObj,LevelManager.Instance.Player().transform.position,Quaternion.identity);
            am.Play(16, true);
           yield return new WaitForSeconds(3.5f);
       }
       yield return new WaitForSeconds(0.5f);
       foreach (Transform child in LevelManager.Instance.Level().GetChild(5))
       {
           child.gameObject.SetActive(false);
       }
       UIManager.Instance.IncreaseScore(0);
       DialogueManager.Instance.EndDialogue();
       LoadingManager.Instance.UpdateLoading();
       LoadingLevelData.Instance.ResetData();
       LoadingManager.Instance.LoadScene(scene);
       am.StopAllSounds();
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

   public void CleanReturnToMenu()
   {
       //LevelManager.Instance.Player().GetComponent<Combat2>().playerAnimator.SetTrigger("Dead");
       LevelManager.Instance.Player().SetActive(false);
       LoadingLevelData.Instance.score = UIManager.Instance.score;
       Time.timeScale = 1f;
       InputManager.canInput = false;
       StartCoroutine(PlayDyingAnimation(true,0));
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
