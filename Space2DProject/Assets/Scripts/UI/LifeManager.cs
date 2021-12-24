using System.Collections;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int lifeBar = 9;
    public int maxHP = 9;
    public bool isInGodMode = false;
    public bool isDying;

    public Animator anim;

    #region Singleton
    public static LifeManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
   public void TakeDamages(int damages)
    {
        if(isInGodMode) return;
        var previousHp = lifeBar;
        lifeBar -= damages;
        if (lifeBar > maxHP)
        {
            lifeBar = maxHP;
        }
        else if (lifeBar < 0)
        {
            lifeBar = 0;
            Die();
        }
        UIManager.Instance.UpdateHpUI(previousHp, lifeBar);

    }

   public void Die(bool instant = false)
   {
       isDying = true;
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
       isDying = false;
   }


}
