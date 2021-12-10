using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{
    public int lifeBar = 9;
    public int maxHP = 9;
    public bool isInGodMode = false;

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

   public void Die()
   {
       Time.timeScale = 1f;

       StartCoroutine(PlayDyingAnimation());

   }

   IEnumerator PlayDyingAnimation()
   {
       yield return new WaitForSeconds(0.5f);
       LoadingManager.Instance.UpdateLoading();
       LoadingLevelData.Instance.ResetData();
       LoadingManager.Instance.LoadScene(3);
   }


}
