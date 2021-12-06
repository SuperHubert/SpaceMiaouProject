using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        int previousHP = lifeBar;
        lifeBar -= damages;
        if (lifeBar > maxHP)
        {
            lifeBar = maxHP;
        }
        else if (lifeBar < 0)
        {
            lifeBar = 0;
        }
        UIManager.Instance.UpdateHpUI(previousHP, lifeBar);
    } 
   
}
